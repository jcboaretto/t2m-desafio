using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using System;

namespace Gerenciamento_de_Tarefas.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task ListarTodosAsync_DeveRetornarUsuarios()
        {
            
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "José da Silva", UserName = "jose", Password = "123456" },
                new Usuario { Id = 2, Nome = "Maria Oliveira", UserName = "maria", Password = "abcdef" }
            };

            _usuarioRepositoryMock.Setup(r => r.ListarAsync()).ReturnsAsync(usuarios);

            
            var resultado = await _usuarioService.ListarTodosAsync();

            
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => Assert.Equal("José da Silva", item.Nome),
                item => Assert.Equal("Maria Oliveira", item.Nome));
        }

        [Fact]
        public async Task ListarTodosAsync_QuandoNaoHouverUsuarios_DeveLancarExcecao()
        {
            
            _usuarioRepositoryMock.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Usuario>());

            
            await Assert.ThrowsAsync<InvalidOperationException>(() => _usuarioService.ListarTodosAsync());
        }

        [Fact]
        public async Task BuscarPorUserNameAsync_QuandoEncontrar_DeveRetornarUsuario()
        {
            
            var usuario = new Usuario { Id = 1, Nome = "João", UserName = "joao", Password = "senha123" };
            _usuarioRepositoryMock.Setup(r => r.BuscarPorUserNameAsync("joao")).ReturnsAsync(usuario);

            
            var resultado = await _usuarioService.BuscarPorUserNameAsync("joao");

            
            Assert.Equal("João", resultado.Nome);
            Assert.Equal("joao", resultado.UserName);
        }

        [Fact]
        public async Task BuscarPorUserNameAsync_QuandoNaoEncontrar_DeveLancarExcecao()
        {
            
            _usuarioRepositoryMock.Setup(r => r.BuscarPorUserNameAsync("inexistente")).ReturnsAsync((Usuario?)null);

            
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usuarioService.BuscarPorUserNameAsync("inexistente"));
        }

        [Fact]
        public async Task RegistrarAsync_QuandoUsuarioForValido_DeveRegistrarComSucesso()
        {
            
            var novoUsuarioDTO = new UsuarioDTO
            {
                Nome = "Carlos",
                UserName = "carlos123",
                Password = "123456"
            };

            _usuarioRepositoryMock.Setup(r => r.BuscarPorUserNameAsync(novoUsuarioDTO.UserName))
                                  .ReturnsAsync((Usuario?)null);

            _usuarioRepositoryMock.Setup(r => r.RegistarAsync(It.IsAny<Usuario>()))
                                  .Returns(Task.CompletedTask);

            
            var resultado = await _usuarioService.RegistrarAsync(novoUsuarioDTO);

            
            Assert.Equal("Usuário registrado com sucesso.", resultado);
        }

        [Fact]
        public async Task RegistrarAsync_QuandoUserNameJaExistir_DeveLancarExcecao()
        {
            
            var dto = new UsuarioDTO
            {
                Nome = "Carlos",
                UserName = "carlos123",
                Password = "123456"
            };

            _usuarioRepositoryMock.Setup(r => r.BuscarPorUserNameAsync(dto.UserName))
                                  .ReturnsAsync(new Usuario());

            
            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => _usuarioService.RegistrarAsync(dto));
            Assert.Equal("Nome de usuário já cadastrado no sistema.", excecao.Message);
        }
    }
}
