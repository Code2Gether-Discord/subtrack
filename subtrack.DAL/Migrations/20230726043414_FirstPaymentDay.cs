using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace subtrack.DAL.Migrations
{
    public partial class FirstPaymentDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstPaymentDay",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPaymentDay",
                table: "Subscriptions");
        }
    }
}
