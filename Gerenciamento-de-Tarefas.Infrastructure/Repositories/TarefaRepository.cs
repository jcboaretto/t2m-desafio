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
            Status TEXT NOT NULL,
            UsuarioId INT NOT NULL,
            FOREIGN KEY(UsuarioId) REFERENCES Usuarios(Id)
        )";
            await _connection.ExecuteAsync(sql);
        }


        public async Task<IEnumerable<Tarefa>> ListarAsync()
        {
            var sql = "SELECT * FROM Tarefas";
            var tarefas = await _connection.QueryAsync<Tarefa>(sql);
            return tarefas;
        }


        public async Task<IEnumerable<Tarefa>> ListarTarefasDoUsuarioAsync(int usuarioId)
        {
            var sql = "SELECT * FROM Tarefas WHERE UsuarioId = @UsuarioId";
            var tarefasDTO = await _connection.QueryAsync<TarefaDTO>(sql, new { UsuarioId = usuarioId });

            return tarefasDTO.Select(t => new Tarefa
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = Enum.Parse<Status>(t.Status.ToString()),
                UsuarioId = usuarioId
            });
        }



        public async Task<Tarefa?> BuscarPorIdAsync(int id)
        {
            var sql = "SELECT * FROM Tarefas WHERE Id = @Id";
            var tarefa = await _connection.QueryFirstOrDefaultAsync<Tarefa>(sql, new { Id = id });

            if (tarefa == null)
            {
                return null;
            }
            return new Tarefa
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = Enum.Parse<Status>(tarefa.Status.ToString()),
                UsuarioId = tarefa.UsuarioId
            };
        }

        public async Task<Tarefa> AdicionarAsync(Tarefa tarefa)
        {

            var sql = @"
                INSERT INTO Tarefas (Titulo, Descricao, Status, UsuarioId)
                VALUES (@Titulo, @Descricao, @Status, @UsuarioId)
                RETURNING Id;
                ";

            var id = await _connection.ExecuteScalarAsync<int>(sql, new
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status.ToString(),
                UsuarioId = tarefa.UsuarioId
            });

            tarefa.Id = id;
            return tarefa;
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
            var sql = "UPDATE Tarefas SET Titulo = @Titulo, " +
                "Descricao = @Descricao, " +
                "Status = @Status " +
                "WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new
            {
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Descricao,
                Status = tarefa.Status.ToString() 
            });
        }


        public async Task<int> CancelarAsync(int id)
        {
            var sql = "UPDATE Tarefas SET Status = @Status WHERE Id = @Id";
            var cancelarTarefa = await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                Status = Status.Cancelada.ToString()
            });

            return cancelarTarefa;
        }

    }
}
