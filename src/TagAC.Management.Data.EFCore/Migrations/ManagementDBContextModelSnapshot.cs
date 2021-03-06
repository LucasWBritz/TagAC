// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TagAC.Management.Data.EFCore.Context;

namespace TagAC.Management.Data.EFCore.Migrations
{
    [DbContext(typeof(ManagementDBContext))]
    partial class ManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessControl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RFID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("SmartLockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SmartLockId", "RFID");

                    b.ToTable("AccessCredentials");
                });

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessControlHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RFID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("SmartLockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("SmartLockId", "RFID");

                    b.ToTable("AccessControlHistory");
                });

            modelBuilder.Entity("TagAC.Management.Domain.Entities.SmartLock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("SmartLocks");
                });

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessControl", b =>
                {
                    b.HasOne("TagAC.Management.Domain.Entities.SmartLock", "SmartLock")
                        .WithMany()
                        .HasForeignKey("SmartLockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SmartLock");
                });

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessControlHistory", b =>
                {
                    b.HasOne("TagAC.Management.Domain.Entities.SmartLock", "SmartLock")
                        .WithMany()
                        .HasForeignKey("SmartLockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SmartLock");
                });
#pragma warning restore 612, 618
        }
    }
}
