using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDtos;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email é obrigatório");
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Password é obrigatória");

            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken);
            if (exists) throw new InvalidOperationException("Email já cadastrado");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserType = Enum.Parse<Domain.Enums.UserType>(dto.UserType, true)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserDto(user.Id, user.Name, user.Email, user.UserType.ToString());
        }

        public async Task<string> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
            if (user == null) throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!valid) throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            // gerar JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user == null) return null;
            return new UserDto(user.Id, user.Name, user.Email, user.UserType.ToString());
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Select(u => new UserDto(u.Id, u.Name, u.Email, u.UserType.ToString()))
                .ToListAsync(cancellationToken);
        }
    }
}
