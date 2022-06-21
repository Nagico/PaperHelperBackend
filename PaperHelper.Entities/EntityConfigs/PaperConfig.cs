using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class PaperConfig : IEntityTypeConfiguration<Paper>
{
    public void Configure(EntityTypeBuilder<Paper> builder)
    {
        builder.ToTable("tb_paper");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(x => x.Abstract)
            .HasColumnName("abstract");

        builder.Property(x => x.Keywords)
            .HasColumnName("keywords")
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(x => x.Authors)
            .HasColumnName("authors")
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(x => x.Publication)
            .HasColumnName("publication");
        
        builder.Property(x=>x.Volume)
            .HasColumnName("volume");
        
        builder.Property(x=>x.Pages)
            .HasColumnName("pages");

        builder.Property(x => x.Year)
            .HasColumnName("year");

        builder.Property(x=>x.Month)
            .HasColumnName("month");
        
        builder.Property(x=>x.Day)
            .HasColumnName("day");
        
        builder.Property(x=>x.Url)
            .HasColumnName("url");
        
        builder.Property(x=>x.Doi)
            .HasColumnName("doi");
        
        builder.Property(x => x.CreateTime)
            .HasColumnName("create_time")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UpdateTime)
            .HasColumnName("update_time");

    }
}