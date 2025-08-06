using Gerenciamento_de_Tarefas.Application.DTOs;


namespace Gerenciamento_de_Tarefas.Application.Interfaces
{
    public interface ITokenService
    {
       Task<string> GenerateToken(LoginDTO loginDTO);

    }
}
