//using Gerenciamento_de_Tarefas.Domain.Repositories;
//using Gerenciamento_de_Tarefas.Domain.Entities;
//using Gerenciamento_de_Tarefas.Domain.Enuns;
//using Gerenciamento_de_Tarefas.Application.DTOs;
//using Microsoft.AspNetCore.Mvc;

//namespace Gerenciamento_de_Tarefas.API.Controllers
//{
//    [ApiController]
//    [Route("api/tarefas")]
//    public class TarefaController : Controller
//    {
//        private readonly ITarefaRepository _tarefaRepository;

//        public TarefaController(ITarefaRepository tarefaRepository)
//        {
//            _tarefaRepository = tarefaRepository;
//        }

//        [HttpGet]
//        public async Task<IActionResult> ListarTodas()
//        {
//            var tarefas = await _tarefaRepository.ListarAsync();
//            if (tarefas == null)
//            {
//                return NotFound("Nenhuma tarefa registrada no sistema."); // não ta funcionando, revisar...

//            }
//            var response = tarefas.Select(t => new TarefaDTO
//            {
//                Id = t.Id,
//                Titulo = t.Titulo,
//                Descricao = t.Descricao,
//                Status = t.Status.ToString()
//            });

//            return Ok(response);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> BuscarPorId(int id)
//        {
//            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
//            if (tarefa == null)
//                return NotFound("Tarefa não encontrada.");

//            var response = new TarefaDTO
//            {
//                Id = tarefa.Id,
//                Titulo = tarefa.Titulo,
//                Descricao = tarefa.Descricao,
//                Status = tarefa.Status.ToString()
//            };

//            return Ok(response);
//        }

//        [HttpPost("criar")]
//        public async Task<IActionResult> Adicionar([FromBody] TarefaDTO dto)
//        {
//            var tarefa = new Tarefa
//            {
//                Titulo = dto.Titulo,
//                Descricao = dto.Descricao,
//                Status = Enum.Parse<Status>(dto.Status)
//            };

//            await _tarefaRepository.AdicionarAsync(tarefa);

//            dto.Id = tarefa.Id; 
//            return CreatedAtAction(nameof(BuscarPorId), new { id = dto.Id }, dto);
//        }


//        [HttpPut("atualizar/{id}")]
//        public async Task<IActionResult> Atualizar(int id, [FromBody] TarefaDTO dto)
//        {
//            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(id);
//            if (tarefaExistente == null)
//            {

//                return NotFound("Tarefa nao encontrada.");
//            }

//            tarefaExistente.Titulo = dto.Titulo;
//            tarefaExistente.Descricao = dto.Descricao;
//            tarefaExistente.Status = Enum.Parse<Status>(dto.Status);

//            await _tarefaRepository.AtualizarAsync(tarefaExistente);
//            return Ok ("Tarefa atualizada!");
//        }

//        [HttpPut("cancelar/{id}")]
//        public async Task<IActionResult> Cancelar(int id)
//        {
//            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
//            if (tarefa == null)
//            {

//                return NotFound("Tarefa não encontrada.");
//            }

//            if (tarefa.Status == Status.Concluida)
//            {
//                return BadRequest("Não é possível cancelar uma tarefa já concluída.");
//            }

//            if (tarefa.Status == Status.Cancelada)
//            {
//                return BadRequest("A tarefa já está cancelada.");
//            }

//            await _tarefaRepository.CancelarAsync(id);
//            return Ok("Tarefa cancelada!");
//        }
//    }
//}

using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Security.Claims;

namespace Gerenciamento_de_Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [SwaggerOperation(
        Summary = "Listar tarefas",
        Description = "Lista todas as tarefas registradas.")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarTodas()
        {
            var tarefas = await _tarefaService.ListarTodasAsync();

            if (!tarefas.Any())
            {
                return NotFound("Nenhuma tarefa registrada no sistema.");
            }

            return Ok(tarefas);
        }

        [SwaggerOperation(
        Summary = "Listar uma tarefa",
        Description = "Lista a tarefa do id desejado se ela pertencer ao usuario logado.")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var tarefa = await _tarefaService.BuscarPorIdAsync(id);

            if (tarefa == null)
                return NotFound("Tarefa não encontrada.");

            return Ok(tarefa);
        }

        [Authorize]
        [SwaggerOperation(
        Summary = "Criar uma tarefa",
        Description = "Cria uma tarefa.")]
        [HttpPost("criar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Adicionar(CreateTarefaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "id"
            );

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {

                return Unauthorized("ID do usuário não encontrado no token.");
            }

            var tarefaCriada = await _tarefaService.AdicionarAsync(dto, userId);

            return CreatedAtAction(nameof(BuscarPorId), new { id = tarefaCriada.Id }, tarefaCriada);
        }



        [SwaggerOperation(
        Summary = "Atualizar uma tarefa",
        Description = "atualizar uma tarefa pelo ID, se ela for do user logado")]
        [HttpPut("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Atualizar(int id, UpdateTarefaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var (sucesso, mensagem) = await _tarefaService.AtualizarAsync(id, dto, userId);

            if (!sucesso)
                return BadRequest(mensagem);

            return Ok(mensagem);
        }



        [SwaggerOperation(
        Summary = "Cancelar uma tarefa",
        Description = "Cancela uma tarefa pelo ID, se ela for do user logado")]
        [HttpPut("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Cancelar(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            try
            {
                var (sucesso, mensagem) = await _tarefaService.CancelarAsync(id, userId);

                if (!sucesso)
                    return BadRequest(mensagem);

                return Ok("Tarefa cancelada!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
