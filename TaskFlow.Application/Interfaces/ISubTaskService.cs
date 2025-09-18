using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.SubTaskDto;

namespace TaskFlow.Application.Interfaces
{
    public interface ISubTaskService
    {
        Task<SubTaskDto> CreateSubTaskAsync(CreateSubTaskDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<SubTaskDto>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
        Task CompleteSubTaskAsync(Guid subTaskId, Guid actorUserId, CancellationToken cancellationToken = default);
        Task<SubTaskDto?> GetByIdAsync(Guid subTaskId, CancellationToken cancellationToken = default);
    }
}
