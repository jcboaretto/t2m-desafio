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

        public async Task<IEnumerable<TarefaDTO>> ListarTarefasDoUsuarioAsync(int usuarioId)
        {
            var tarefas = await _tarefaRepository.ListarTarefasDoUsuarioAsync(usuarioId);

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


        public async Task<(bool sucesso, string mensagem)> AtualizarAsync(int id, UpdateTarefaDTO dto, int usuarioId)

        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);

            if (tarefa == null)
            {
                
                return (false, "tarefa não encontrada");
            }

          

            if (tarefa.UsuarioId != usuarioId)
            {

                return (false, "essa tarefa não é desse usuario");
            }

            if (tarefa.Status == Status.Concluida)
            {

                return (false, "a tarefa ja foi concluída.");
            }

            if (tarefa.Status == Status.Cancelada)
            {

                return (false, "a tarefa ja esta cancelada.");
            }

           


            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Status = dto.Status;

            await _tarefaRepository.AtualizarAsync(tarefa);

            return (true, null);
        }



        public async Task<(bool sucesso, string mensagem)> CancelarAsync(int id, int usuarioId)
        {
            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id);
            if (tarefa == null || tarefa.UsuarioId != usuarioId)
                return (false, "tarefa não encontrada ou nao pertence a esse usuario.");

            if (tarefa.Status == Status.Concluida)
            {

                return (false, "nao foi possivel cancelar pois a tarefa ja foi concluída.");
            }

            if (tarefa.Status == Status.Cancelada)
            {

                return (false, "a tarefa ja esta cancelada.");
            }
            var cancelarTarefa = await _tarefaRepository.CancelarAsync(id);
            return (true, "tarefa cancelada c sucesso!");
        }


    }
}