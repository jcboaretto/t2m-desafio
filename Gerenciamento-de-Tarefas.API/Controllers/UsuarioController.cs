//using Gerenciamento_de_Tarefas.Domain.Repositories;
//using Gerenciamento_de_Tarefas.Domain.Entities;
//using Gerenciamento_de_Tarefas.Domain.Enuns;
//using Gerenciamento_de_Tarefas.Application.DTOs;
//using Microsoft.AspNetCore.Mvc;

//namespace Gerenciamento_de_Tarefas.API.Controllers
//{
//    [ApiController]
//    [Route("api/usuarios")]
//    public class UsuarioController : Controller
//    {
//        private readonly IUsuarioRepository _usuarioRepository;

//        public UsuarioController(IUsuarioRepository usuarioRepository)
//        {
//            _usuarioRepository = usuarioRepository;
//        }

//        [HttpGet]  
//        public async Task<IActionResult> ListarTodos()
//        {
//            var usuarios = await _usuarioRepository.ListarAsync();
//            if (usuarios == null || !usuarios.Any())
//                return NotFound("Nenhum usuário registrado no sistema.");
//            var response = usuarios.Select(u => new UsuarioDTO
//            {
//                Id = u.Id,
//                Nome = u.Nome,
//                UserName = u.UserName
//            });
//            return Ok(response);
//        }

//        [HttpGet("{userName}")]
//        public async Task<IActionResult> BuscarPorUserName(string userName)
//        {
//            var usuario = await _usuarioRepository.BuscarPorUserNameAsync(userName);
//            if (usuario == null)
//            {

//                return NotFound("Usuário não encontrado.");
//            }
//            var response = new UsuarioDTO
//            {
//                Id = usuario.Id,
//                Nome = usuario.Nome,
//                UserName = usuario.UserName
//            };
//            return Ok(response);
//        }

//        [HttpPost("cadastrar")]
//        public async Task<IActionResult> RegistrarAsycn([FromBody] UsuarioDTO dto)
//        {
//            var usuario = await _usuarioRepository.BuscarPorUserNameAsync(dto.UserName);
//            if (usuario != null && usuario.UserName == dto.UserName)
//            {
//                return BadRequest("Nome de usuário já cadastrado no sistema.");
//            }

//            //criando usuario novo se deixar
//            var usuarioNovo = new Usuario
//            {
//                Nome = dto.Nome,
//                UserName = dto.UserName,
//                Password = dto.Password
//            };

//            //salvo no banco
//            await _usuarioRepository.RegistarAsync(usuarioNovo);
//            return Ok("Usuario registrado c sucesso.");
//        }
//    }
//}

using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_de_Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> ListarTodos()
        {
            try
            {
                var usuarios = await _usuarioService.ListarTodosAsync();
                return Ok(usuarios);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> BuscarPorUserName(string userName)
        {
            try
            {
                var usuario = await _usuarioService.BuscarPorUserNameAsync(userName);
                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> RegistrarAsync([FromBody] UsuarioDTO dto)
        {
            try
            {
                var resultado = await _usuarioService.RegistrarAsync(dto);
                return Ok(resultado);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}