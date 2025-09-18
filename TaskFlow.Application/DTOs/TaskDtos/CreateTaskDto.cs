using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDtos
{
    public record CreateTaskDto(string Name, string? Description, DateTime StartDate, DateTime EndDate, Guid ResponsibleUserId);
}
