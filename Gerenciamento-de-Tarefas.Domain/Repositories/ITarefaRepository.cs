using Gerenciamento_de_Tarefas.Domain.Entities;

namespace Gerenciamento_de_Tarefas.Domain.Repositories;

public interface ITarefaRepository
{
    Task CriarTabelaAsync();
    Task<IEnumerable<Tarefa>> ListarAsync();
    Task<IEnumerable<Tarefa>> ListarTarefasDoUsuarioAsync(int usuarioId);
    Task<Tarefa?> BuscarPorIdAsync(int id);
    Task<Tarefa> AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    Task <int> CancelarAsync(int id);
}
