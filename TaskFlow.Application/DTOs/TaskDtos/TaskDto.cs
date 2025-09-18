using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDtos
{
    public record TaskDto(Guid Id, string Name, string? Description, DateTime StartDate, DateTime EndDate, Guid ResponsibleUserId, int CompletionPercentage);
}
