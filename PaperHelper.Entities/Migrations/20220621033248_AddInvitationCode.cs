using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddInvitationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "invitation_code",
                table: "tb_project",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "invitation_code",
                table: "tb_project");
        }
    }
}
