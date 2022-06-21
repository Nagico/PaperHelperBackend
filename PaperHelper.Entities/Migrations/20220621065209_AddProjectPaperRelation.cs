using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddProjectPaperRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "tb_paper",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_project_id",
                table: "tb_paper",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_paper_tb_project_project_id",
                table: "tb_paper",
                column: "project_id",
                principalTable: "tb_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_paper_tb_project_project_id",
                table: "tb_paper");

            migrationBuilder.DropIndex(
                name: "IX_tb_paper_project_id",
                table: "tb_paper");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tb_paper");
        }
    }
}
