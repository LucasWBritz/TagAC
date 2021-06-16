using System.Linq;
using System.Threading.Tasks;
using TagAC.Domain.Entities;

namespace TagAC.Domain.Interfaces
{
    public interface IRepository<TEntity, TId> 
        where TEntity : Entity<TId>
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity GetById(TId id);
        Task<TEntity> GetByIdAsync(TId id);
        IQueryable<TEntity> GetAll();

        TEntity Create(TEntity entity);
        Task<TEntity> CreateAsync(TEntity entity);
        TEntity Update(TEntity obj);
        void Delete(TEntity entity);
        void DeleteById(TId id);
    }
}
