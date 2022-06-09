using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_paper",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    @abstract = table.Column<string>(name: "abstract", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    keywords = table.Column<string>(type: "longtext", nullable: false, defaultValue: "[]")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    authors = table.Column<string>(type: "longtext", nullable: false, defaultValue: "[]")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    publication = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    volume = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pages = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    year = table.Column<int>(type: "int", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false),
                    day = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    doi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    update_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_paper", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tag", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_attachment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ext = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    project_id = table.Column<int>(type: "int", nullable: false),
                    paper_id = table.Column<int>(type: "int", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    update_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_attachment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_attachment_tb_paper_paper_id",
                        column: x => x.paper_id,
                        principalTable: "tb_paper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_attachment_tb_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tb_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_note",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    project_id = table.Column<int>(type: "int", nullable: false),
                    paper_id = table.Column<int>(type: "int", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    update_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_note", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_note_tb_paper_paper_id",
                        column: x => x.paper_id,
                        principalTable: "tb_paper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_note_tb_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tb_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_paper_reference",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    paper_id = table.Column<int>(type: "int", nullable: false),
                    ref_paper_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_paper_reference", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_paper_reference_tb_paper_paper_id",
                        column: x => x.paper_id,
                        principalTable: "tb_paper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_paper_reference_tb_paper_ref_paper_id",
                        column: x => x.ref_paper_id,
                        principalTable: "tb_paper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_paper_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    paper_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_paper_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_paper_tag_tb_paper_paper_id",
                        column: x => x.paper_id,
                        principalTable: "tb_paper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_paper_tag_tb_tag_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tb_tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_attachment_paper_id",
                table: "tb_attachment",
                column: "paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_attachment_project_id",
                table: "tb_attachment",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_note_paper_id",
                table: "tb_note",
                column: "paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_note_project_id",
                table: "tb_note",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_reference_paper_id",
                table: "tb_paper_reference",
                column: "paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_reference_ref_paper_id",
                table: "tb_paper_reference",
                column: "ref_paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_tag_paper_id",
                table: "tb_paper_tag",
                column: "paper_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_paper_tag_tag_id",
                table: "tb_paper_tag",
                column: "tag_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_attachment");

            migrationBuilder.DropTable(
                name: "tb_note");

            migrationBuilder.DropTable(
                name: "tb_paper_reference");

            migrationBuilder.DropTable(
                name: "tb_paper_tag");

            migrationBuilder.DropTable(
                name: "tb_paper");

            migrationBuilder.DropTable(
                name: "tb_tag");
        }
    }
}
