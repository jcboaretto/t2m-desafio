using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class TarefaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [MinLength(4, ErrorMessage = "Título não pode ser vazio")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public string Status { get; set; } 
    }
}
