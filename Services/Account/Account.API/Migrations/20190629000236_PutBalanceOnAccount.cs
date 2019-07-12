using Microsoft.EntityFrameworkCore.Migrations;

namespace Account.API.Migrations
{
    public partial class PutBalanceOnAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "balance",
                table: "ACCOUNT",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "balance",
                table: "ACCOUNT");
        }
    }
}
