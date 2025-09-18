using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDtos;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, CancellationToken cancellationToken = default)
        {
            // validar usuário responsável exista
            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.ResponsibleUserId, cancellationToken);
            if (!userExists) throw new KeyNotFoundException("Usuário responsável não encontrado");

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ResponsibleUserId = dto.ResponsibleUserId,
                CompletionPercentage = 0
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return new TaskDto(task.Id, task.Name, task.Description, task.StartDate, task.EndDate, task.ResponsibleUserId, task.CompletionPercentage);
        }

        public async Task<TaskDto?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            var t = await _context.Tasks.FindAsync(new object[] { taskId }, cancellationToken);
            if (t == null) return null;
            return new TaskDto(t.Id, t.Name, t.Description, t.StartDate, t.EndDate, t.ResponsibleUserId, t.CompletionPercentage);
        }

        public async Task<IEnumerable<TaskDto>> GetTasksForUserAsync(Guid userId, bool onlyResponsible = false, CancellationToken cancellationToken = default)
        {
            // Se onlyResponsible == true, retorna tasks onde ResponsibleUserId == userId
            // Caso contrário, retorna tasks que contenham subtasks atribuídas ao user (ou todas se admin - lógica pode ser adaptada)
            if (onlyResponsible)
            {
                return await _context.Tasks
                    .Where(t => t.ResponsibleUserId == userId)
                    .Select(t => new TaskDto(t.Id, t.Name, t.Description, t.StartDate, t.EndDate, t.ResponsibleUserId, t.CompletionPercentage))
                    .ToListAsync(cancellationToken);
            }

            // retorna tasks que tenham subtasks atribuídas ao usuário OU onde ele seja responsável
            return await _context.Tasks
                .Where(t => t.ResponsibleUserId == userId || t.SubTasks.Any(st => st.ResponsibleUserId == userId))
                .Select(t => new TaskDto(t.Id, t.Name, t.Description, t.StartDate, t.EndDate, t.ResponsibleUserId, t.CompletionPercentage))
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateTaskAsync(TaskDto dto, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FindAsync(new object[] { dto.Id }, cancellationToken);
            if (task == null) throw new KeyNotFoundException("Task não encontrada");

            task.Name = dto.Name;
            task.Description = dto.Description;
            task.StartDate = dto.StartDate;
            task.EndDate = dto.EndDate;
            task.ResponsibleUserId = dto.ResponsibleUserId;
            // não atualizamos CompletionPercentage diretamente aqui; ele é controlado pelas subtasks
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FindAsync(new object[] { taskId }, cancellationToken);
            if (task == null) throw new KeyNotFoundException("Task não encontrada");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
