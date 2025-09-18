using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDtos;

namespace TaskFlow.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task<string> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default); // retorna JWT
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
