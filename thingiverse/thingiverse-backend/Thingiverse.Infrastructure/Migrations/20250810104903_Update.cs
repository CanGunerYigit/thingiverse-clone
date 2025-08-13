using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thingiverse_backend.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rollere ait DeleteData ve InsertData kaldırıldı.

            // ItemImages tablosuna ImageData ve ContentType kolonları ekleniyor
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "ItemImages",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ItemImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollere ait DeleteData ve InsertData kaldırıldı.

            // Eklenen kolonlar kaldırılıyor
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ItemImages");
        }
    }
}
