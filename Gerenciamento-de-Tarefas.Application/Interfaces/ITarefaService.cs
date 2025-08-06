using Gerenciamento_de_Tarefas.Application.DTOs;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaDTO>> ListarTodasAsync();
        Task<TarefaDTO?> BuscarPorIdAsync(int id);
        Task<TarefaDTO> AdicionarAsync(CreateTarefaDTO newDTO, int usuarioId);
        Task AtualizarAsync(int id, UpdateTarefaDTO dto, int usuarioId);
        Task CancelarAsync(int id, int usuarioId);

    }
}