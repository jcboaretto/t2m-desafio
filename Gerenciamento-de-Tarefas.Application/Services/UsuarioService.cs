using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Services;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<UsuarioDTO>> ListarTodosAsync()
        {
            var usuarios = await _usuarioRepository.ListarAsync();

            if (usuarios == null || !usuarios.Any())
            {
                throw new InvalidOperationException("Nenhum usuário registrado no sistema.");
            }

            return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                UserName = u.UserName
            });
        }

        public async Task<UsuarioDTO> BuscarPorUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Nome de usuário não pode ser vazio.", nameof(userName));
            }

            var usuario = await _usuarioRepository.BuscarPorUserNameAsync(userName);

            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                UserName = usuario.UserName
            };
        }

        public async Task<string> RegistrarAsync(UsuarioDTO dto)
        {
            
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Dados do usuário são obrigatórios.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                throw new ArgumentException("Nome é obrigatório.", nameof(dto.Nome));
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                throw new ArgumentException("Nome de usuário é obrigatório.", nameof(dto.UserName));
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Senha é obrigatória.", nameof(dto.Password));
            }

            //validacao username
            var usuarioExistente = await _usuarioRepository.BuscarPorUserNameAsync(dto.UserName);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Nome de usuário já cadastrado no sistema.");
            }

            //se der certo cria o usuario
            var usuarioNovo = new Usuario
            {
                Nome = dto.Nome,
                UserName = dto.UserName,
                Password = dto.Password 
            };

            //salvo no banco 
            await _usuarioRepository.RegistarAsync(usuarioNovo);

            return "Usuário registrado com sucesso.";
        }
    }
}