using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.SubTaskDto
{
    public record SubTaskDto(
    Guid Id,
    string Name,
    string? Description,
    Guid TaskItemId,
    Guid ResponsibleUserId,
    string CompletionStatus,
    DateTime? DueDate,
    DateTime CreatedAt);
}
