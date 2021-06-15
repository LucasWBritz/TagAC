using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TagAC.Management.Data.EFCore.Migrations
{
    public partial class AddAccessControlHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessControlHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SmartLockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessControlHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessControlHistory_SmartLocks_SmartLockId",
                        column: x => x.SmartLockId,
                        principalTable: "SmartLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessControlHistory_SmartLockId_RFID",
                table: "AccessControlHistory",
                columns: new[] { "SmartLockId", "RFID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessControlHistory");
        }
    }
}
