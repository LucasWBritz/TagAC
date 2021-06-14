﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TagAC.Management.Data.EFCore.Context;

namespace TagAC.Management.Data.EFCore.Migrations
{
    [DbContext(typeof(ManagementDBContext))]
    [Migration("20210614095920_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessCredential", b =>
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

            modelBuilder.Entity("TagAC.Management.Domain.Entities.AccessCredential", b =>
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