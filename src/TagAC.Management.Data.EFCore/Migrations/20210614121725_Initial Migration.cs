using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TagAC.Management.Data.EFCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartLocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartLocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessCredentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RFID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SmartLockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessCredentials_SmartLocks_SmartLockId",
                        column: x => x.SmartLockId,
                        principalTable: "SmartLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessCredentials_SmartLockId_RFID",
                table: "AccessCredentials",
                columns: new[] { "SmartLockId", "RFID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessCredentials");

            migrationBuilder.DropTable(
                name: "SmartLocks");
        }
    }
}
