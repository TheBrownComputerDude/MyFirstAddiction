using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class VideoThumbnailLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailLocation",
                table: "Video",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailLocation",
                table: "Video");
        }
    }
}
