using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace thingiverse_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dc278fd-0d55-4a9e-8239-b9ae40339a82");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5f474e0-bd8a-48db-9cc8-8ad61d865926");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e812ed7-ac40-4cb7-9f8c-1d431b6cc930", null, "Admin", "ADMIN" },
                    { "879d91cd-9724-4061-b023-a02cfd17b94b", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e812ed7-ac40-4cb7-9f8c-1d431b6cc930");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "879d91cd-9724-4061-b023-a02cfd17b94b");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4dc278fd-0d55-4a9e-8239-b9ae40339a82", null, "Admin", "ADMIN" },
                    { "c5f474e0-bd8a-48db-9cc8-8ad61d865926", null, "User", "USER" }
                });
        }
    }
}
