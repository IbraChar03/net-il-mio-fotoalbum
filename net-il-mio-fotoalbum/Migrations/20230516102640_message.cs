using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_il_mio_fotoalbum.Migrations
{
    /// <inheritdoc />
    public partial class message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_photo_PhotoId",
                table: "message");

            migrationBuilder.DropIndex(
                name: "IX_message_PhotoId",
                table: "message");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_message_PhotoId",
                table: "message",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_message_photo_PhotoId",
                table: "message",
                column: "PhotoId",
                principalTable: "photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
