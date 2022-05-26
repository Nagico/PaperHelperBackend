using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddUserProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_project_tb_user_create_user_id",
                table: "tb_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_project_ProjectId",
                table: "tb_user");

            migrationBuilder.DropIndex(
                name: "IX_tb_user_ProjectId",
                table: "tb_user");

            migrationBuilder.DropIndex(
                name: "IX_tb_project_create_user_id",
                table: "tb_project");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "tb_user");

            migrationBuilder.DropColumn(
                name: "create_user_id",
                table: "tb_project");

            migrationBuilder.CreateTable(
                name: "tb_user_project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    project_id = table.Column<int>(type: "int", nullable: false),
                    is_owner = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    access_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    edit_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_user_project_tb_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tb_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_user_project_tb_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_project_project_id",
                table: "tb_user_project",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_project_user_id",
                table: "tb_user_project",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_user_project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "tb_user",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "create_user_id",
                table: "tb_project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_ProjectId",
                table: "tb_user",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_project_create_user_id",
                table: "tb_project",
                column: "create_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_project_tb_user_create_user_id",
                table: "tb_project",
                column: "create_user_id",
                principalTable: "tb_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_project_ProjectId",
                table: "tb_user",
                column: "ProjectId",
                principalTable: "tb_project",
                principalColumn: "id");
        }
    }
}
