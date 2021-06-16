using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TagAC.Domain.Entities;
using TagAC.Domain.Interfaces;
using TagAC.Management.Data.EFCore.Context;

namespace TagAC.Management.Data.EFCore.Repositories
{
    public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        protected readonly ManagementDBContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(ManagementDBContext dbContext)
        {
            Context = dbContext;
            DbSet = Context.Set<TEntity>();
        }

        public IUnitOfWork UnitOfWork => Context;

        public TEntity Create(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public TEntity GetById(TId id)
        {
            return DbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteById(TId id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }        
    }
}
