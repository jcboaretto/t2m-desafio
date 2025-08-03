using Gerenciamento_de_Tarefas.Domain.Entities;

namespace Gerenciamento_de_Tarefas.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task CriarTabelaAsync();
        Task RegistarAsync(Usuario usuario); 
        Task <Usuario?> BuscarPorUserNameAsync(string userName);
        Task<IEnumerable<Usuario>> ListarAsync();
    }
}
