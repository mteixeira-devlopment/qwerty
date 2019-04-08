using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Account.API.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CUSTOMER",
                columns: table => new
                {
                    id = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    fullname = table.Column<string>(type: "VARCHAR(80)", nullable: false),
                    birthdate = table.Column<DateTime>(type: "DATE", nullable: false),
                    doc_text = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    doc_photo = table.Column<string>(type: "TEXT", nullable: true),
                    doc_verified = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    id = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    id_user = table.Column<string>(type: "VARCHAR(38)", nullable: false),
                    id_customer = table.Column<string>(type: "VARCHAR(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNT", x => x.id);
                    table.ForeignKey(
                        name: "FK_ACCOUNT_CUSTOMER_id_customer",
                        column: x => x.id_customer,
                        principalTable: "CUSTOMER",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACCOUNT_id_customer",
                table: "ACCOUNT",
                column: "id_customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCOUNT");

            migrationBuilder.DropTable(
                name: "CUSTOMER");
        }
    }
}
