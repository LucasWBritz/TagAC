using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Data.EFCore.Configuration
{
    public class AccessControlHistoryConfiguration : IEntityTypeConfiguration<AccessControlHistory>
    {
        public void Configure(EntityTypeBuilder<AccessControlHistory> builder)
        {
            builder.ToTable("AccessControlHistory");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RFID)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SmartLockId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(x => x.Time)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.SmartLock);

            builder.HasIndex(entity => new { entity.SmartLockId, entity.RFID });
        }
    }
}
