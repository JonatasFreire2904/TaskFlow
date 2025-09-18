using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.SubTaskDto
{
    public record CreateSubTaskDto(string Name, string? Description, Guid TaskItemId, Guid ResponsibleUserId, DateTime? DueDate);
}
