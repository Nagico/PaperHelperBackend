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
    [Migration("20220526161710_AddProject")]
    partial class AddProject
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PaperHelper.Entities.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("create_user_id");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.HasIndex("CreateUserId");

                    b.ToTable("tb_project", (string)null);
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
                        .ValueGeneratedOnAdd()
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

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("tb_user", (string)null);
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Project", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.User", "CreateUser")
                        .WithMany()
                        .HasForeignKey("CreateUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreateUser");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.User", b =>
                {
                    b.HasOne("PaperHelper.Entities.Entities.Project", null)
                        .WithMany("Members")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("PaperHelper.Entities.Entities.Project", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
