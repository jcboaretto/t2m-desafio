using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Enuns;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Moq;
using Xunit;

public class TarefaServiceTests
{
    private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
    private readonly TarefaService _tarefaService;

    public TarefaServiceTests()
    {
        _tarefaRepositoryMock = new Mock<ITarefaRepository>();
        _tarefaService = new TarefaService(_tarefaRepositoryMock.Object);
    }

    [Fact]
    public async Task ListarTodasAsync()
    {
        var tarefas = new List<Tarefa>
        {
            new Tarefa { Id = 1, Titulo = "Teste", Descricao = "Descricao", Status = Status.Pendente, UsuarioId = 10 }
        };

        _tarefaRepositoryMock.Setup(repo => repo.ListarAsync()).ReturnsAsync(tarefas);

        var resultado = await _tarefaService.ListarTodasAsync();

        Assert.Single(resultado);
        Assert.Equal("Teste", resultado.First().Titulo);
    }

    [Fact]
    public async Task BuscarPorIdAsync()
    {
        var tarefa = new Tarefa { Id = 1, Titulo = "Titulo", Descricao = "Desc", Status = Status.Pendente, UsuarioId = 1 };
        _tarefaRepositoryMock.Setup(repo => repo.BuscarPorIdAsync(1)).ReturnsAsync(tarefa);

        var resultado = await _tarefaService.BuscarPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal("Titulo", resultado!.Titulo);
    }

    [Fact]
    public async Task AdicionarAsync_DeveRetornarTarefaDTOCriada()
    {
        var dto = new CreateTarefaDTO { Titulo = "Nova", Descricao = "Desc" };

        _tarefaRepositoryMock.Setup(repo => repo.AdicionarAsync(It.IsAny<Tarefa>()))
            .ReturnsAsync((Tarefa tarefa) =>
            {
                tarefa.Id = 99;
                return tarefa;
            });

        var resultado = await _tarefaService.AdicionarAsync(dto, 123);

        Assert.Equal(99, resultado.Id);
        Assert.Equal("Nova", resultado.Titulo);
        Assert.Equal(Status.Pendente, resultado.Status);
    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarErro_SeTarefaNaoEncontrada()
    {
        _tarefaRepositoryMock.Setup(repo => repo.BuscarPorIdAsync(1)).ReturnsAsync((Tarefa?)null);

        var (sucesso, mensagem) = await _tarefaService.AtualizarAsync(1, new UpdateTarefaDTO(), 10);

        Assert.False(sucesso);
        Assert.Equal("tarefa não encontrada", mensagem);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarTarefa_SeValido()
    {
        var tarefa = new Tarefa
        {
            Id = 1,
            UsuarioId = 123,
            Status = Status.Pendente,
            Titulo = "Antigo",
            Descricao = "Antiga desc"
        };

        var updateDto = new UpdateTarefaDTO
        {
            Titulo = "Novo título",
            Descricao = "Nova descrição",
            Status = Status.Concluida
        };

        _tarefaRepositoryMock.Setup(repo => repo.BuscarPorIdAsync(1)).ReturnsAsync(tarefa);
        _tarefaRepositoryMock.Setup(repo => repo.AtualizarAsync(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

        var (sucesso, mensagem) = await _tarefaService.AtualizarAsync(1, updateDto, 123);

        Assert.True(sucesso);
        Assert.Null(mensagem);
    }

    [Fact]
    public async Task CancelarAsync_DeveRetornarErro_SeNaoPertencerAoUsuario()
    {
        var tarefa = new Tarefa { Id = 1, UsuarioId = 999, Status = Status.Pendente };
        _tarefaRepositoryMock.Setup(repo => repo.BuscarPorIdAsync(1)).ReturnsAsync(tarefa);

        var (sucesso, mensagem) = await _tarefaService.CancelarAsync(1, 123);

        Assert.False(sucesso);
        Assert.Contains("nao pertence", mensagem);
    }

    [Fact]
    public async Task CancelarAsync_DeveRetornarSucesso_SeCancelada()
    {
        var tarefa = new Tarefa { Id = 1, UsuarioId = 123, Status = Status.Pendente };
        _tarefaRepositoryMock.Setup(repo => repo.BuscarPorIdAsync(1)).ReturnsAsync(tarefa);
        _tarefaRepositoryMock.Setup(repo => repo.CancelarAsync(1)).ReturnsAsync(1);

        var (sucesso, mensagem) = await _tarefaService.CancelarAsync(1, 123);

        Assert.True(sucesso);
        Assert.Equal("tarefa cancelada c sucesso!", mensagem);
    }
}
