using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_il_mio_fotoalbum.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageEntryId",
                table: "photo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_photo_ImageEntryId",
                table: "photo",
                column: "ImageEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_photo_ImageEntries_ImageEntryId",
                table: "photo",
                column: "ImageEntryId",
                principalTable: "ImageEntries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_photo_ImageEntries_ImageEntryId",
                table: "photo");

            migrationBuilder.DropTable(
                name: "ImageEntries");

            migrationBuilder.DropIndex(
                name: "IX_photo_ImageEntryId",
                table: "photo");

            migrationBuilder.DropColumn(
                name: "ImageEntryId",
                table: "photo");
        }
    }
}
