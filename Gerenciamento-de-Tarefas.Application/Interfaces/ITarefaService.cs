
using Gerenciamento_de_Tarefas.Application.DTOs;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaDTO>> ListarTodasAsync();
        Task<TarefaDTO?> BuscarPorIdAsync(int id);
        Task<TarefaDTO> AdicionarAsync(TarefaDTO dto);
        Task<bool> AtualizarAsync(int id, TarefaDTO dto);
        Task<bool> CancelarAsync(int id);
    }
}