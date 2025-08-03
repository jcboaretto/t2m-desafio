using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Enuns;
using Gerenciamento_de_Tarefas.Domain.Repositories;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<IEnumerable<TarefaDTO>> ListarTodasAsync()
        {
            var tarefas = await _tarefaRepository.ListarAsync();

            if (tarefas == null || !tarefas.Any())
            {
                return Enumerable.Empty<TarefaDTO>();
            }

            return tarefas.Select(t => new TarefaDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Status = t.Status.ToString()
            });
        }

        public async Task<TarefaDTO?> BuscarPorIdAsync(int id)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);

            if (tarefa == null)
                return null;

            return new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status.ToString()
            };
        }

        public async Task<TarefaDTO> AdicionarAsync(TarefaDTO dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = Enum.Parse<Status>(dto.Status)
            };

            await _tarefaRepository.AdicionarAsync(tarefa);

            return new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status.ToString()
            };
        }

        public async Task<bool> AtualizarAsync(int id, TarefaDTO dto)
        {
            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(id);

            if (tarefaExistente == null)
                return false;

            tarefaExistente.Titulo = dto.Titulo;
            tarefaExistente.Descricao = dto.Descricao;
            tarefaExistente.Status = Enum.Parse<Status>(dto.Status);

            await _tarefaRepository.AtualizarAsync(tarefaExistente);
            return true;
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);

            if (tarefa == null)
                return false;

            if (tarefa.Status == Status.Concluida)
                throw new InvalidOperationException("Não é possível cancelar uma tarefa já concluída.");

            if (tarefa.Status == Status.Cancelada)
                throw new InvalidOperationException("A tarefa já está cancelada.");

            await _tarefaRepository.CancelarAsync(id);
            return true;
        }
    }
}