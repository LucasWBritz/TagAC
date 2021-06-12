using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Data.EFCore.Context
{
    public class ManagementDBContext : DbContext, IUnitOfWork
    {
        public ManagementDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SmartLock> SmartLocks { get; set; }
        public DbSet<AccessCredential> AccessCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            return base.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
