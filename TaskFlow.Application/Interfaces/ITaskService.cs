using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDtos;

namespace TaskFlow.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, CancellationToken cancellationToken = default);
        Task<TaskDto?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskDto>> GetTasksForUserAsync(Guid userId, bool onlyResponsible = false, CancellationToken cancellationToken = default);
        Task UpdateTaskAsync(TaskDto dto, CancellationToken cancellationToken = default);
        Task DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
    }
}
