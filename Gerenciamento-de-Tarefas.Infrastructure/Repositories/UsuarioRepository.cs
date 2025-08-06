using Dapper;
using System.Data;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Application.DTOs;


namespace Gerenciamento_de_Tarefas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository 
    {
        private readonly IDbConnection _connection;

        public UsuarioRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CriarTabelaAsync()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id SERIAL PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    UserName TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL
                    
                )";
            await _connection.ExecuteAsync(sql);
        }
        
        public async Task RegistarAsync(Usuario usuario)
        {
            var sql = @"
            INSERT INTO Usuarios (Nome, UserName, Password)
            VALUES (@Nome, @UserName, @Password)";

            await _connection.ExecuteAsync(sql, usuario);
        }

        public async Task<Usuario?> BuscarPorUserNameAsync(string userName)
        {
            var sql = "SELECT * FROM Usuarios WHERE UserName = @UserName";
            return await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { UserName = userName });
        }

        public async Task<IEnumerable<Usuario>> ListarAsync()
        {
            var sql = "SELECT Id, Nome, UserName FROM Usuarios";
            var usuarioDTO = await _connection.QueryAsync<Usuario>(sql);
            return usuarioDTO.Select(u => new Usuario
            {
                Id = u.Id,
                Nome = u.Nome,
                UserName = u.UserName,
            }
              );
        }
    }
}
