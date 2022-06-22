using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class DeleteAttachmentFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_attachment_tb_paper_paper_id",
                table: "tb_attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_attachment_tb_project_project_id",
                table: "tb_attachment");

            migrationBuilder.DropIndex(
                name: "IX_tb_attachment_paper_id",
                table: "tb_attachment");

            migrationBuilder.DropIndex(
                name: "IX_tb_attachment_project_id",
                table: "tb_attachment");

            migrationBuilder.DropColumn(
                name: "paper_id",
                table: "tb_attachment");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tb_attachment");

            migrationBuilder.AddColumn<int>(
                name: "attachment_id",
                table: "tb_paper",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_attachment_id",
                table: "tb_paper",
                column: "attachment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_paper_tb_attachment_attachment_id",
                table: "tb_paper",
                column: "attachment_id",
                principalTable: "tb_attachment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_paper_tb_attachment_attachment_id",
                table: "tb_paper");

            migrationBuilder.DropIndex(
                name: "IX_tb_paper_attachment_id",
                table: "tb_paper");

            migrationBuilder.DropColumn(
                name: "attachment_id",
                table: "tb_paper");

            migrationBuilder.AddColumn<int>(
                name: "paper_id",
                table: "tb_attachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "tb_attachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tb_attachment_paper_id",
                table: "tb_attachment",
                column: "paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_attachment_project_id",
                table: "tb_attachment",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_attachment_tb_paper_paper_id",
                table: "tb_attachment",
                column: "paper_id",
                principalTable: "tb_paper",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_attachment_tb_project_project_id",
                table: "tb_attachment",
                column: "project_id",
                principalTable: "tb_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
