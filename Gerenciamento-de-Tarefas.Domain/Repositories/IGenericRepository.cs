using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gerenciamento_de_Tarefas.Domain.Repositories
{
    public interface IGenericRepository<TEntity, Id> where TEntity : class
    {

        /// métodos básicos de CRUD
        Task<TEntity?> GetById(Id id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Update(TEntity entity, int id);
        Task Delete(int id);
        List<TEntity> List();
    }
}
