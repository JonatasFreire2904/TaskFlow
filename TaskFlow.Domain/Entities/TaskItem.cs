using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Responsible employee for this task
        public Guid ResponsibleUserId { get; set; }
        public User? ResponsibleUser { get; set; }

        // 0 to 100
        public int CompletionPercentage { get; set; } = 0;
        public ICollection<SubTask> SubTasks { get; set; } = new
        List<SubTask>();
    }
}
