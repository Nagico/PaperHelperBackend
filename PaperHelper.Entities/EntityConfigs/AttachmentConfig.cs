using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class AttachmentConfig : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("tb_attachment");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Type)
            .HasColumnName("type");
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Ext)
            .HasColumnName("ext");

        builder.Property(x => x.Url)
            .HasColumnName("url");
        
        builder.Property(x => x.Md5)
            .HasColumnName("md5");
        
        builder.Property(x => x.Doi)
            .HasColumnName("doi");

        builder.Property(x => x.CreateTime)
            .HasColumnName("create_time");

        builder.Property(x => x.UpdateTime)
            .HasColumnName("update_time");
    }
}