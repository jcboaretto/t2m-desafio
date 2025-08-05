

namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UsuarioResponseDTO Usuario { get; set; }
        
        public DateTime Expiracao { get; set; }
    }
}
