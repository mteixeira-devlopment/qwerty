using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deposit.API.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHARGE",
                columns: table => new
                {
                    id = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    id_provider_charge = table.Column<int>(type: "INT", nullable: false),
                    value = table.Column<decimal>(type: "DECIMAL(19,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    status = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHARGE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DEPOSIT",
                columns: table => new
                {
                    id = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    id_account = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    id_charge = table.Column<string>(type: "VARCHAR(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPOSIT", x => x.id);
                    table.ForeignKey(
                        name: "FK_DEPOSIT_CHARGE_id_charge",
                        column: x => x.id_charge,
                        principalTable: "CHARGE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEPOSIT_id_charge",
                table: "DEPOSIT",
                column: "id_charge");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEPOSIT");

            migrationBuilder.DropTable(
                name: "CHARGE");
        }
    }
}
