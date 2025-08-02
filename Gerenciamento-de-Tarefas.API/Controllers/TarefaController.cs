using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Enuns;
using Gerenciamento_de_Tarefas.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_de_Tarefas.API.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    public class TarefaController : Controller
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaController(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var tarefas = await _tarefaRepository.ListarAsync();
            if (tarefas == null)
            {
                return NotFound("Nenhuma tarefa registrada no sistema."); // não ta funcionando, revisar...

            }
            var response = tarefas.Select(t => new TarefaDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = t.Status.ToString()
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
            if (tarefa == null)
                return NotFound("Tarefa não encontrada.");

            var response = new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status.ToString()
            };

            return Ok(response);
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Adicionar([FromBody] TarefaDTO dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = Enum.Parse<Status>(dto.Status)
            };

            await _tarefaRepository.AdicionarAsync(tarefa);

            dto.Id = tarefa.Id; 
            return CreatedAtAction(nameof(BuscarPorId), new { id = dto.Id }, dto);
        }


        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TarefaDTO dto)
        {
            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(id);
            if (tarefaExistente == null)
            {

                return NotFound("Tarefa nao encontrada.");
            }

            tarefaExistente.Titulo = dto.Titulo;
            tarefaExistente.Descricao = dto.Descricao;
            tarefaExistente.Status = Enum.Parse<Status>(dto.Status);

            await _tarefaRepository.AtualizarAsync(tarefaExistente);
            return Ok ("Tarefa atualizada!");
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
            if (tarefa == null)
            {

                return NotFound("Tarefa não encontrada.");
            }

            if (tarefa.Status == Status.Concluida)
            {
                return BadRequest("Não é possível cancelar uma tarefa já concluída.");
            }

            if (tarefa.Status == Status.Cancelada)
            {
                return BadRequest("A tarefa já está cancelada.");
            }

            await _tarefaRepository.CancelarAsync(id);
            return Ok("Tarefa cancelada!");
        }
    }
}
