using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public UserType UserType { get; set; }
        public ICollection<TaskItem> ResponsibleTasks { get; set; } = new
        List<TaskItem>();
        public ICollection<SubTask> ResponsibleSubTasks { get; set; } = new
        List<SubTask>();

    }
}
