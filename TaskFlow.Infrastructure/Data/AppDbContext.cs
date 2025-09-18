using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }


        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<SubTask> SubTasks { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


            modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.ResponsibleUser)
            .WithMany(u => u.ResponsibleTasks)
            .HasForeignKey(t => t.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<SubTask>()
            .HasOne(s => s.TaskItem)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(s => s.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SubTask>()
            .HasOne(s => s.ResponsibleUser)
            .WithMany(u => u.ResponsibleSubTasks)
            .HasForeignKey(s => s.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
