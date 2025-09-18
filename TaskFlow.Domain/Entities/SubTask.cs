using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class SubTask
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        public string Name { get; set; } = null!;


        public string? Description { get; set; }


        public Guid TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }


        public Guid ResponsibleUserId { get; set; }
        public User? ResponsibleUser { get; set; }


        public CompletionStatus CompletionStatus { get; set; } = CompletionStatus.Pending;


        public DateTime? DueDate { get; set; }
    }
}
