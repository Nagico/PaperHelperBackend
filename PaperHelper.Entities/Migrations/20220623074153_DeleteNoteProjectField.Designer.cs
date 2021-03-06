// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaperHelper.Entities;

#nullable disable

namespace PaperHelper.Entities.Migrations
{
    [DbContext(typeof(PaperHelperContext))]
    [Migration("20220623074153_DeleteNoteProjectField")]
    partial class DeleteNoteProjectField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PaperHelper.Entities.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<string>("Ext")
                        .HasColumnType("longtext")
                        .HasColumnName("ext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_time");

                    b.Property<string>("Url")
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("tb_attachment", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<int>("PaperId")
                        .HasColumnType("int")
                        .HasColumnName("paper_id");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("title");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.HasIndex("PaperId");

                    b.HasIndex("ProjectId");

                    b.ToTable("tb_note", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Paper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Abstract")
                        .HasColumnType("longtext")
                        .HasColumnName("abstract");

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int")
                        .HasColumnName("attachment_id");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("[]")
                        .HasColumnName("authors");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<int>("Day")
                        .HasColumnType("int")
                        .HasColumnName("day");

                    b.Property<string>("Doi")
                        .HasColumnType("longtext")
                        .HasColumnName("doi");

                    b.Property<string>("Keywords")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("[]")
                        .HasColumnName("keywords");

                    b.Property<int>("Month")
                        .HasColumnType("int")
                        .HasColumnName("month");

                    b.Property<string>("Pages")
                        .HasColumnType("longtext")
                        .HasColumnName("pages");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("project_id");

                    b.Property<string>("Publication")
                        .HasColumnType("longtext")
                        .HasColumnName("publication");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_time");

                    b.Property<string>("Url")
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.Property<string>("Volume")
                        .HasColumnType("longtext")
                        .HasColumnName("volume");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("tb_paper", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.PaperReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("PaperId")
                        .HasColumnType("int")
                        .HasColumnName("paper_id");

                    b.Property<int>("RefPaperId")
                        .HasColumnType("int")
                        .HasColumnName("ref_paper_id");

                    b.HasKey("Id");

                    b.HasIndex("PaperId");

                    b.HasIndex("RefPaperId");

                    b.ToTable("tb_paper_reference", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.PaperTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("PaperId")
                        .HasColumnType("int")
                        .HasColumnName("paper_id");

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("tag_id");

                    b.HasKey("Id");

                    b.HasIndex("PaperId");

                    b.HasIndex("TagId");

                    b.ToTable("tb_paper_tag", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<string>("InvitationCode")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("invitation_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.ToTable("tb_project", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("tb_tag", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext")
                        .HasColumnName("avatar");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("last_login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("char(32)")
                        .HasColumnName("password")
                        .IsFixedLength();

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("char(11)")
                        .HasColumnName("phone")
                        .IsFixedLength();

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("tb_user", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.UserProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("AccessTime")
                        .IsRequired()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("access_time");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("edit_time");

                    b.Property<bool>("IsOwner")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_owner");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("project_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("tb_user_project", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Note", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Paper", "Paper")
                        .WithMany("Notes")
                        .HasForeignKey("PaperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaperHelper.Entities.Entities.Project", null)
                        .WithMany("Notes")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Paper");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Paper", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaperHelper.Entities.Entities.Project", "Project")
                        .WithMany("Papers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.PaperReference", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Paper", "Paper")
                        .WithMany("ReferenceFrom")
                        .HasForeignKey("PaperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaperHelper.Entities.Entities.Paper", "RefPaper")
                        .WithMany("References")
                        .HasForeignKey("RefPaperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paper");

                    b.Navigation("RefPaper");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.PaperTag", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Paper", "Paper")
                        .WithMany("Tags")
                        .HasForeignKey("PaperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaperHelper.Entities.Entities.Tag", "Tag")
                        .WithMany("Papers")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paper");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.UserProject", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Project", "Project")
                        .WithMany("Members")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaperHelper.Entities.Entities.User", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Paper", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("ReferenceFrom");

                    b.Navigation("References");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Project", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Notes");

                    b.Navigation("Papers");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Tag", b =>
                {
                    b.Navigation("Papers");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.User", b =>
                {
                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
