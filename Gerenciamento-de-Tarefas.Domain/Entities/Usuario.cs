namespace Gerenciamento_de_Tarefas.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public String Nome { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    }
}
