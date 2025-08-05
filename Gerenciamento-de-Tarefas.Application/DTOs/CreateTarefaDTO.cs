using System.ComponentModel.DataAnnotations;


namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class CreateTarefaDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [MinLength(4, ErrorMessage = "Título não pode ser vazio")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        
    }
}
