using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;
namespace PaperHelper.Entities.EntityConfigs;

public class PaperReferenceConfig : IEntityTypeConfiguration<PaperReference>
{
    public void Configure(EntityTypeBuilder<PaperReference> builder)
    {
        builder.ToTable("tb_paper_reference");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x=>x.PaperId)
            .HasColumnName("paper_id");
        
        builder.Property(x=>x.RefPaperId)
            .HasColumnName("ref_paper_id");
        
        builder.HasOne(x=>x.Paper)
            .WithMany(x => x.ReferenceFrom)
            .HasForeignKey(x => x.PaperId);
        
        builder.HasOne(x=>x.RefPaper)
            .WithMany(x=>x.References)
            .HasForeignKey(x=>x.RefPaperId);
    }
}