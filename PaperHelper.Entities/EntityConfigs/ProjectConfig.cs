using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("tb_project");
        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description");
        builder.Property(x => x.InvitationCode)
            .HasColumnName("invitation_code")
            .IsRequired();

        builder.Property(x => x.CreateTime)
            .HasColumnName("create_time");

        builder.Property(x => x.UpdateTime)
            .HasColumnName("update_time");
        
        builder.HasMany(x => x.Papers)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId);

    }
}