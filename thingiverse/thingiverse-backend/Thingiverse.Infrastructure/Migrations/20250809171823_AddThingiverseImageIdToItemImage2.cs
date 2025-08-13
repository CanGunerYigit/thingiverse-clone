using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace thingiverse_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddThingiverseImageIdToItemImage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9683501c-de9b-420f-8880-7b9185c7d9dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c44c208c-5396-4537-9969-0e134169f765");

            migrationBuilder.AddColumn<int>(
                name: "ThingiverseImageId",
                table: "ItemImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4dc278fd-0d55-4a9e-8239-b9ae40339a82", null, "Admin", "ADMIN" },
                    { "c5f474e0-bd8a-48db-9cc8-8ad61d865926", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dc278fd-0d55-4a9e-8239-b9ae40339a82");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5f474e0-bd8a-48db-9cc8-8ad61d865926");

            migrationBuilder.DropColumn(
                name: "ThingiverseImageId",
                table: "ItemImages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9683501c-de9b-420f-8880-7b9185c7d9dd", null, "User", "USER" },
                    { "c44c208c-5396-4537-9969-0e134169f765", null, "Admin", "ADMIN" }
                });
        }
    }
}
