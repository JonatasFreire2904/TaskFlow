using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDtos;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;

        public AuthController(IUserService users)
        {
            _users = users;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var token = await _users.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            try
            {
                var created = await _users.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] System.Guid id)
        {
            var u = await _users.GetByIdAsync(id);
            if (u == null) return NotFound();
            return Ok(u);
        }
    }
}
