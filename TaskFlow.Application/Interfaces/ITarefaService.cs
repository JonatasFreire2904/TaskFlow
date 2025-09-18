using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDtos;

namespace TaskFlow.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<TaskDto> CriarTarefaAsync(TaskDto dto);
        Task<TaskDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<TaskDto>> ListarTodasAsync();
        Task<TaskDto> AtualizarTarefaAsync(int id, TaskDto dto);
        Task<bool> DeletarTarefaAsync(int id);
    }
}
