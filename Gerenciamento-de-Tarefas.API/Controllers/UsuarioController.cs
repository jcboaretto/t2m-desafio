using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [SwaggerOperation(
        Summary = "Listar Usuários")]
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

        [SwaggerOperation(
        Summary = "Buscar por username")]
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

        [SwaggerOperation(
        Summary = "Realizar cadastro")]
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