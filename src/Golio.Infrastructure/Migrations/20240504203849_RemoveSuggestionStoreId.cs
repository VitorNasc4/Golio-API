using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSuggestionStoreId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Suggestions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Suggestions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
