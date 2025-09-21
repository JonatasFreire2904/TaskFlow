using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDtos;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _tasks;

        public TasksController(ITaskService tasks)
        {
            _tasks = tasks;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var created = await _tasks.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var t = await _tasks.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpGet("user/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetTasksForUser([FromRoute] Guid userId, [FromQuery] bool onlyResponsible = false)
        {
            var list = await _tasks.GetTasksForUserAsync(userId, onlyResponsible);
            return Ok(list);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _tasks.UpdateTaskAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _tasks.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
