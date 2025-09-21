using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public static class AppDbContextSeed
    {
        public static void Seed(AppDbContext ctx)
        {
            if (ctx.Users.Any()) return;

            var admin = new User
            {
                Id = Guid.NewGuid(),
                Name = "admin",
                Email = "admin@local",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                UserType = Domain.Enums.UserType.Admin
            };

            ctx.Users.Add(admin);
            ctx.SaveChanges();
        }
    }

}
