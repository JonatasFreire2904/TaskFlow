using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.SubTaskDto;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly AppDbContext _context;

        public SubTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubTaskDto> CreateSubTaskAsync(CreateSubTaskDto dto, CancellationToken cancellationToken = default)
        {
            // validações: Task e ResponsibleUser existem
            var taskExists = await _context.Tasks.AnyAsync(t => t.Id == dto.TaskItemId, cancellationToken);
            if (!taskExists) throw new KeyNotFoundException("Task pai não encontrada");

            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.ResponsibleUserId, cancellationToken);
            if (!userExists) throw new KeyNotFoundException("Usuário responsável não encontrado");

            var sub = new SubTask
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                TaskItemId = dto.TaskItemId,
                ResponsibleUserId = dto.ResponsibleUserId,
                CompletionStatus = CompletionStatus.Pending,
                DueDate = dto.DueDate
            };

            _context.SubTasks.Add(sub);
            await _context.SaveChangesAsync(cancellationToken);

            return new SubTaskDto(sub.Id, sub.Name, sub.Description, sub.TaskItemId, sub.ResponsibleUserId, sub.CompletionStatus.ToString(), sub.DueDate);
        }

        public async Task<IEnumerable<SubTaskDto>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _context.SubTasks
                .Where(s => s.TaskItemId == taskId)
                .Select(s => new SubTaskDto(s.Id, s.Name, s.Description, s.TaskItemId, s.ResponsibleUserId, s.CompletionStatus.ToString(), s.DueDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<SubTaskDto?> GetByIdAsync(Guid subTaskId, CancellationToken cancellationToken = default)
        {
            var s = await _context.SubTasks.FindAsync(new object[] { subTaskId }, cancellationToken);
            if (s == null) return null;
            return new SubTaskDto(s.Id, s.Name, s.Description, s.TaskItemId, s.ResponsibleUserId, s.CompletionStatus.ToString(), s.DueDate);
        }

        public async Task CompleteSubTaskAsync(Guid subTaskId, Guid actorUserId, CancellationToken cancellationToken = default)
        {
            // transação para manter consistência entre subtask e porcentagem da task
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var sub = await _context.SubTasks
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.Id == subTaskId, cancellationToken);

            if (sub == null) throw new KeyNotFoundException("SubTask não encontrada");

            var actor = await _context.Users.FindAsync(new object[] { actorUserId }, cancellationToken);
            if (actor == null) throw new UnauthorizedAccessException("Usuário não encontrado");

            // valida permissão: Admin ou responsável pela subtask
            if (actor.UserType != Domain.Enums.UserType.Admin && sub.ResponsibleUserId != actorUserId)
                throw new UnauthorizedAccessException("Você não tem permissão para concluir esta subtask");

            if (sub.CompletionStatus == CompletionStatus.Completed)
            {
                // já concluída — nada a fazer
                return;
            }

            sub.CompletionStatus = CompletionStatus.Completed;
            await _context.SaveChangesAsync(cancellationToken);

            // recalcula porcentagem da task pai usando agregação (evita leitura em memória)
            var stats = await _context.SubTasks
                .Where(s => s.TaskItemId == sub.TaskItemId)
                .GroupBy(s => s.TaskItemId)
                .Select(g => new { Total = g.Count(), Completed = g.Count(s => s.CompletionStatus == CompletionStatus.Completed) })
                .FirstOrDefaultAsync(cancellationToken);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == sub.TaskItemId, cancellationToken);
            if (task != null && stats != null)
            {
                task.CompletionPercentage = stats.Total == 0 ? 0 : (int)Math.Round(stats.Completed * 100.0 / stats.Total);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
    }
}
