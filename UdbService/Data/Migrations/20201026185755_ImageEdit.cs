using Microsoft.EntityFrameworkCore.Migrations;

namespace UdbService.Data.Migrations
{
    public partial class ImageEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Service");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Service",
                type: "nvarchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Service");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
