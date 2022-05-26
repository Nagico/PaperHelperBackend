using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddUserAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "tb_user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "tb_user");
        }
    }
}
