using Gerenciamento_de_Tarefas.Domain.Entities;

namespace Gerenciamento_de_Tarefas.Domain.Repositories;

public interface ITarefaRepository
{
    Task CriarTabelaAsync();
    Task<IEnumerable<Tarefa>> ListarAsync();
    Task<Tarefa?> BuscarPorIdAsync(int id);
    Task AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    Task CancelarAsync(int id);
}
