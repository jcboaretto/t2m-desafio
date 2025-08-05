using System.ComponentModel.DataAnnotations;


namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "UserName é obrigatório")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }    
    }
}
