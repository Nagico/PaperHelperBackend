using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class NoteConfig : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("tb_note");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Type)
            .HasColumnName("type");

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();
        
        builder.Property(x => x.ProjectId)
            .HasColumnName("project_id");
        
        builder.Property(x => x.PaperId)
            .HasColumnName("paper_id");

        builder.Property(x => x.CreateTime)
            .HasColumnName("create_time")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UpdateTime)
            .HasColumnName("update_time");
        
        builder.HasOne(x => x.Project)
            .WithMany(x => x.Notes)
            .HasForeignKey(x => x.ProjectId);
        
        builder.HasOne(x => x.Paper)
            .WithMany(x => x.Notes)
            .HasForeignKey(x => x.PaperId);

    }
}