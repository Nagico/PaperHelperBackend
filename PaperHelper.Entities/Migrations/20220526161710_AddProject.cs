using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "tb_user",
                newName: "create_time");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "tb_user",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_user_id = table.Column<int>(type: "int", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    update_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_project_tb_user_create_user_id",
                        column: x => x.create_user_id,
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_ProjectId",
                table: "tb_user",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_project_create_user_id",
                table: "tb_project",
                column: "create_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_project_ProjectId",
                table: "tb_user",
                column: "ProjectId",
                principalTable: "tb_project",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_project_ProjectId",
                table: "tb_user");

            migrationBuilder.DropTable(
                name: "tb_project");

            migrationBuilder.DropIndex(
                name: "IX_tb_user_ProjectId",
                table: "tb_user");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "tb_user");

            migrationBuilder.RenameColumn(
                name: "create_time",
                table: "tb_user",
                newName: "created_at");
        }
    }
}
