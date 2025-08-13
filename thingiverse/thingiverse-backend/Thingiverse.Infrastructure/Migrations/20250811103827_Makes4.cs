using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thingiverse_backend.Migrations
{
    /// <inheritdoc />
    public partial class Makes4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Makes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreviewImage",
                table: "Makes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Makes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Makes");

            migrationBuilder.DropColumn(
                name: "PreviewImage",
                table: "Makes");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Makes");
        }
    }
}
