using Dapper;
using System.Data;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Domain.Enuns;

namespace Gerenciamento_de_Tarefas.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly IDbConnection _connection;

        public TarefaRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CriarTabelaAsync()
        {
            var sql = @"
        CREATE TABLE IF NOT EXISTS Tarefas (
            Id SERIAL PRIMARY KEY,
            Titulo TEXT NOT NULL,
            Descricao TEXT,
            Status TEXT NOT NULL
        )";
            await _connection.ExecuteAsync(sql);
        }

        //UsuarioId INT NOT NULL,
        //            FOREIGN KEY(UsuarioId) REFERENCES Usuarios(Id)
        public async Task<IEnumerable<Tarefa>> ListarAsync()
        {
            var sql = "SELECT * FROM Tarefas";
            var tarefasDTO = await _connection.QueryAsync<TarefaDTO>(sql);

            return tarefasDTO.Select(t => new Tarefa
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = Enum.Parse<Status>(t.Status)
            });
        }

        public async Task<Tarefa?> BuscarPorIdAsync(int id)
        {
            var sql = "SELECT * FROM Tarefas WHERE Id = @Id";
            var tarefaDTO = await _connection.QueryFirstOrDefaultAsync<TarefaDTO>(sql, new { Id = id });

            if (tarefaDTO == null)
            {
                return null;
            }
            return new Tarefa
            {
                Id = tarefaDTO.Id,
                Titulo = tarefaDTO.Titulo,
                Descricao = tarefaDTO.Descricao,
                Status = Enum.Parse<Status>(tarefaDTO.Status),
            };
        }

        public async Task AdicionarAsync(Tarefa tarefa)
        {
            var sql = "INSERT INTO Tarefas (Titulo, Descricao, Status) VALUES (@Titulo, @Descricao, @Status)";
            await _connection.ExecuteAsync(sql, new
            {
                tarefa.Titulo,
                tarefa.Descricao,
                Status = tarefa.Status.ToString()
            });
        }
        //, UsuarioId
        //    , @UsuarioId


        //public async Task<List<Tarefa>> BuscarPorUsuarioAsync(int usuarioId)
        //{
        //    var sql = "SELECT * FROM Tarefas WHERE UsuarioId = @UsuarioId";
        //    var result = await _connection.QueryAsync<Tarefa>(sql, new { UsuarioId = usuarioId });
        //    return result.ToList();
        //}

        public async Task AtualizarAsync(Tarefa tarefa)
        {
            var sql = "UPDATE Tarefas SET Titulo = @Titulo, Descricao = @Descricao, Status = @Status WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new
            {
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Descricao,
                Status = tarefa.Status.ToString()
            });
        }


        public async Task CancelarAsync(int id)
        {
            var sql = "UPDATE Tarefas SET Status = @Status WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                Status = Status.Cancelada.ToString()
            });
        }
    }
}
