using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class PaperTagConfig : IEntityTypeConfiguration<PaperTag>
{
    public void Configure(EntityTypeBuilder<PaperTag> builder)
    {
        builder.ToTable("tb_paper_tag");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PaperId)
            .HasColumnName("paper_id");
        
        builder.Property(x=>x.TagId)
            .HasColumnName("tag_id");
        
        builder.HasOne(x => x.Paper)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.PaperId);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.Papers)
            .HasForeignKey(x => x.TagId);
    }
}