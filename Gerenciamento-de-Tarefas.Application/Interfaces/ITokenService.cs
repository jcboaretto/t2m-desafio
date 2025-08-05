using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Domain.Entities;


namespace Gerenciamento_de_Tarefas.Application.Interfaces
{
    public interface ITokenService
    {
       Task<string> GenerateToken(LoginDTO loginDTO);

    }
}
