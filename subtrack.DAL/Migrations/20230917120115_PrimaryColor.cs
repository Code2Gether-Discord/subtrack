using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace subtrack.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundColor",
                table: "Subscriptions",
                newName: "PrimaryColor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrimaryColor",
                table: "Subscriptions",
                newName: "BackgroundColor");
        }
    }
}
