using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.SubTaskDto;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubTasksController : ControllerBase
    {
        private readonly ISubTaskService _subs;
        private readonly ITaskService _tasks;

        public SubTasksController(ISubTaskService subs, ITaskService tasks)
        {
            _subs = subs;
            _tasks = tasks;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateSubTaskDto dto)
        {
            // pega o id do usuário logado
            var actorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (actorIdClaim == null) return Unauthorized();
            if (!Guid.TryParse(actorIdClaim, out var actorId)) return Unauthorized();

            // verifica se a task pai existe
            var task = await _tasks.GetByIdAsync(dto.TaskItemId);
            if (task == null) return BadRequest(new { message = "Task pai não encontrada" });

            // Admin pode criar subtasks, ou o responsável da task também pode
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && task.ResponsibleUserId != actorId)
                return Forbid();

            // cria a subtask
            var created = await _subs.CreateSubTaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("task/{taskId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetByTask([FromRoute] Guid taskId)
        {
            var list = await _subs.GetByTaskIdAsync(taskId);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var s = await _subs.GetByIdAsync(id);
            if (s == null) return NotFound();
            return Ok(s);
        }

        // marcar como completa
        [HttpPost("{id:guid}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete([FromRoute] Guid id)
        {
            var actorIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (actorIdClaim == null) return Unauthorized();
            if (!Guid.TryParse(actorIdClaim, out var actorId)) return Unauthorized();

            await _subs.CompleteSubTaskAsync(id, actorId);
            return NoContent();
        }
    }
}
