using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Data.EFCore.Configuration
{
    public class SmartLockConfiguration : IEntityTypeConfiguration<SmartLock>
    {
        public void Configure(EntityTypeBuilder<SmartLock> builder)
        {
            builder.ToTable("SmartLocks");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar(200)")
                .IsRequired();
        }
    }
}
