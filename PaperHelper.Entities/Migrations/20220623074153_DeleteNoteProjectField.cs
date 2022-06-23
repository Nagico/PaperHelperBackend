using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class DeleteNoteProjectField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_note_tb_project_project_id",
                table: "tb_note");

            migrationBuilder.RenameColumn(
                name: "project_id",
                table: "tb_note",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_note_project_id",
                table: "tb_note",
                newName: "IX_tb_note_ProjectId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "tb_note",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_note_tb_project_ProjectId",
                table: "tb_note",
                column: "ProjectId",
                principalTable: "tb_project",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_note_tb_project_ProjectId",
                table: "tb_note");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "tb_note",
                newName: "project_id");

            migrationBuilder.RenameIndex(
                name: "IX_tb_note_ProjectId",
                table: "tb_note",
                newName: "IX_tb_note_project_id");

            migrationBuilder.AlterColumn<int>(
                name: "project_id",
                table: "tb_note",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_note_tb_project_project_id",
                table: "tb_note",
                column: "project_id",
                principalTable: "tb_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
