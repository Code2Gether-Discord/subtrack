using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace subtrack.DAL.Migrations
{
    /// <inheritdoc />
    public partial class BackgroundColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "#282828");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Subscriptions");
        }
    }
}
