using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class VideoLocationUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Video",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Video_Location",
                table: "Video",
                column: "Location",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Video_Location",
                table: "Video");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Video",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
