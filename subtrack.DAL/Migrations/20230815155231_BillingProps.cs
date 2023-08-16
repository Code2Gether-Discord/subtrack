using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace subtrack.DAL.Migrations
{
    public partial class BillingProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingInterval",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "BillingOccurrence",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingInterval",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BillingOccurrence",
                table: "Subscriptions");
        }
    }
}
