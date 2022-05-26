using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tb_user");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Password)
            .HasColumnName("password")
            .HasMaxLength(32)
            .IsFixedLength()
            .IsRequired();
        
        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(11)
            .IsFixedLength()
            .IsRequired();

        builder.Property(x => x.LastLogin)
            .HasColumnName("last_login");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .ValueGeneratedOnAdd();
    }
}