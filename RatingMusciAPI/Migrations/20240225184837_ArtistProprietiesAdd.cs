using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingMusciAPI.Migrations
{
    /// <inheritdoc />
    public partial class ArtistProprietiesAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalListeners",
                table: "Artists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "TotalListeners",
                table: "Artists");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "Songs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId");
        }
    }
}
