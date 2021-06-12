using TagAC.Domain.Entities;

namespace TagAC.Domain.Interfaces
{
    public interface IRepository<TEntity, TId> 
        where TEntity : Entity<TId>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
