using Gerenciamento_de_Tarefas.Application.DTOs;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> ListarTodosAsync();
        Task<UsuarioDTO> BuscarPorUserNameAsync(string userName);
        Task<string> RegistrarAsync(UsuarioDTO dto);
    }
}