using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(5, ErrorMessage = "Nome não pode ser vazio")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "UserName é obrigatório")]
        [MinLength(4, ErrorMessage = "UserName não pode ser vazio")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        public string Password { get; set; }
    }
}
