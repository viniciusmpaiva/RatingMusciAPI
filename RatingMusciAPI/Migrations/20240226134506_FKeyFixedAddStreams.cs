using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingMusciAPI.Migrations
{
    /// <inheritdoc />
    public partial class FKeyFixedAddStreams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Streams",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Streams",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Streams",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Streams",
                table: "Albums");
        }
    }
}
