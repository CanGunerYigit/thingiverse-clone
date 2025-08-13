using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace thingiverse_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddThingiverseImageIdToItemImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fae5899b-f08a-408d-ac71-9eb92e41ac8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbfecbec-4bef-4ac6-b2bd-d57a77edf8d3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9683501c-de9b-420f-8880-7b9185c7d9dd", null, "User", "USER" },
                    { "c44c208c-5396-4537-9969-0e134169f765", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9683501c-de9b-420f-8880-7b9185c7d9dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c44c208c-5396-4537-9969-0e134169f765");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "fae5899b-f08a-408d-ac71-9eb92e41ac8c", null, "Admin", "ADMIN" },
                    { "fbfecbec-4bef-4ac6-b2bd-d57a77edf8d3", null, "User", "USER" }
                });
        }
    }
}
