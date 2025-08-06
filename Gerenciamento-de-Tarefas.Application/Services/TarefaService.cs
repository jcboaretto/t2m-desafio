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
                Status = t.Status,
                UsuarioId = t.UsuarioId
                
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
                Status = tarefa.Status,
                UsuarioId = tarefa.UsuarioId
            };
        }

        public async Task<TarefaDTO> AdicionarAsync(CreateTarefaDTO newDTO, int usuarioId)
        {
            var tarefa = new Tarefa
            {
                Titulo = newDTO.Titulo,
                Descricao = newDTO.Descricao,
                Status = Status.Pendente,
                UsuarioId = usuarioId
            };

            await _tarefaRepository.AdicionarAsync(tarefa);

            return new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                UsuarioId = tarefa.UsuarioId
            };
        }


        public async Task AtualizarAsync(int id, UpdateTarefaDTO dto, int usuarioId)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);

            if (tarefa == null)
            {
                throw new KeyNotFoundException("tarefa não encontrada.");
            }

            if (tarefa.UsuarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("esta tarefa não pertence ao usuário.");
            }

            if (tarefa.Status == Status.Concluida)
            {
                throw new InvalidOperationException("a tarefa já foi concluída.");
            }

            if (tarefa.Status == Status.Cancelada)
            {
                throw new InvalidOperationException("a tarefa já foi cancelada.");
            }

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Status = dto.Status;

            await _tarefaRepository.AtualizarAsync(tarefa);
        }




        public async Task CancelarAsync(int id, int usuarioId)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
            if (tarefa == null)
            {
                throw new KeyNotFoundException("tarefa não encontrada.");
            }

            if (tarefa.UsuarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("essa tarefa não pertence ao usuário.");
            }

            if (tarefa.Status == Status.Concluida)
            {
                throw new InvalidOperationException("a tarefa já foi concluída.");
            }

            if (tarefa.Status == Status.Cancelada)
            {
                throw new InvalidOperationException("a tarefa já foi cancelada.");
            }
        
             await _tarefaRepository.CancelarAsync(id);
        }


    }
}