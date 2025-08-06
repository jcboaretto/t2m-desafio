using Gerenciamento_de_Tarefas.Domain.Enuns;

namespace Gerenciamento_de_Tarefas.Domain.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public String Titulo { get; set; } = string.Empty;
        public String? Descricao { get; set; }
        public Status Status { get; set; }
        
        public int UsuarioId { get; set; }

       



    }
}
