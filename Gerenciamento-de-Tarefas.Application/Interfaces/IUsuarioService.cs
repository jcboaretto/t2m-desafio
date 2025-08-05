using Gerenciamento_de_Tarefas.Application.DTOs;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponseDTO>> ListarTodosAsync();
        Task<UsuarioResponseDTO> BuscarPorUserNameAsync(string userName);
        Task<string> RegistrarAsync(UsuarioDTO dto);

        
    }
}