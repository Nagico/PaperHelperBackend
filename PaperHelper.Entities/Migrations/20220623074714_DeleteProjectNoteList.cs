using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class DeleteProjectNoteList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_note_tb_project_ProjectId",
                table: "tb_note");

            migrationBuilder.DropIndex(
                name: "IX_tb_note_ProjectId",
                table: "tb_note");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "tb_note");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "tb_note",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_note_ProjectId",
                table: "tb_note",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_note_tb_project_ProjectId",
                table: "tb_note",
                column: "ProjectId",
                principalTable: "tb_project",
                principalColumn: "id");
        }
    }
}
