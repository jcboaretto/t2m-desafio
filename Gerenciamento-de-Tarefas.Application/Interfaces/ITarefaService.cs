
using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Domain.Entities;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public interface ITarefaService
    {
        Task<IEnumerable<TarefaDTO>> ListarTodasAsync();
        Task<TarefaDTO?> BuscarPorIdAsync(int id);
        Task<TarefaDTO> AdicionarAsync(CreateTarefaDTO newDTO, int usuarioId);
        Task<(bool sucesso, string mensagem)> AtualizarAsync(int id, UpdateTarefaDTO dto, int usuarioId);
        Task<(bool sucesso, string mensagem)> CancelarAsync(int id, int usuarioId);
        Task<IEnumerable<TarefaDTO>> ListarTarefasDoUsuarioAsync(int usuarioId);
    }
}