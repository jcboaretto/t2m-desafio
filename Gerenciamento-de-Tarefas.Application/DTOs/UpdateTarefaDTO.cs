using Gerenciamento_de_Tarefas.Domain.Enuns;

namespace Gerenciamento_de_Tarefas.Application.DTOs
{
    public class UpdateTarefaDTO
    {
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public Status Status { get; set; } 
    }
}
