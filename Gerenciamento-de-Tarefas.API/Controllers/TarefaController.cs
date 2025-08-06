using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/tarefas")]
public class TarefaController : Controller
{
    private readonly ITarefaService _tarefaService;

    public TarefaController(ITarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }

    [SwaggerOperation(
        Summary = "Listar tarefas")]
    [HttpGet]
    public async Task<IActionResult> ListarTodas()
    {
        var tarefas = await _tarefaService.ListarTodasAsync();

        if (!tarefas.Any())
            return NotFound("Nenhuma tarefa registrada no sistema.");

        return Ok(tarefas);
    }

    [SwaggerOperation(
        Summary = "Buscar uma tarefa pelo ID")]
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var tarefa = await _tarefaService.BuscarPorIdAsync(id);

        if (tarefa == null)
            return NotFound("Tarefa não encontrada.");

        return Ok(tarefa);
    }

    [SwaggerOperation(
        Summary = "Registrar uma tarefa")]
    [HttpPost("criar")]
    public async Task<IActionResult> Adicionar(CreateTarefaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier || c.Type == "id"
        );

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("ID do usuário não encontrado no token.");

        var tarefaCriada = await _tarefaService.AdicionarAsync(dto, userId);

        return CreatedAtAction(nameof(BuscarPorId), new { id = tarefaCriada.Id }, tarefaCriada);
    }

    [SwaggerOperation(
        Summary = "Atualizar uma tarefa pelo ID")]
    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> Atualizar(int id, UpdateTarefaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        try
        {
            await _tarefaService.AtualizarAsync(id, dto, userId);
            return Ok("Tarefa atualizada com sucesso.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [SwaggerOperation(
        Summary = "Cancelar uma tarefa pelo ID")]
    [HttpPut("cancelar/{id}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        try
        {
            await _tarefaService.CancelarAsync(id, userId);
            return Ok("Tarefa cancelada com sucesso.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
