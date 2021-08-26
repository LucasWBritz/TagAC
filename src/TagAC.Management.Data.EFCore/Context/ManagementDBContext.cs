using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagAC.Domain.Entities;
using TagAC.Domain.Events;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Data.EFCore.Context
{
    public class ManagementDBContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediatorHandler;

        public ManagementDBContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediatorHandler = mediator;
        }

        public DbSet<SmartLock> SmartLocks { get; set; }
        public DbSet<AccessControl> AccessCredentials { get; set; }
        public DbSet<AccessControlHistory> AccessControlHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                // Set to varchar 100 all the string properties that were not mapped. Avoid varchar(MAX)
                property.SetColumnType("varchar(100)");
            }

            // Automatically apply configurations.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDBContext).Assembly);
        }

        public int Commit()
        {
            var result = base.SaveChanges();

            if (result > 0) // success
            {
                _mediatorHandler.PublishDomainEvents(this).GetAwaiter().GetResult();
            }

            return result;
        }

        public async Task<int> CommitAsync()
        {
            var result = await base.SaveChangesAsync();
            if (result > 0) // success
            {
                await _mediatorHandler.PublishDomainEvents(this);
            }
            return result;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishDomainEvents<T>(this IMediator mediator, T context) where T : DbContext
        {
            var entitiesWithDomainEvents = context.ChangeTracker.Entries<IEntityWithDomainEvent>().Where(x => x.Entity.Events.Any());

            var domainEvents = entitiesWithDomainEvents.SelectMany(x => x.Entity.Events);

            //var tasks = domainEvents.Select(async (domainEvent) => await mediator.Publish(domainEvent));
            IEnumerable<Task> tasks = domainEvents.Select(dEvent => mediator.Publish(dEvent));

            await Task.WhenAll(tasks);

            // clear events on the entities
            entitiesWithDomainEvents.ToList().ForEach(e => e.Entity.ClearEvents());
        }
    }
}
