using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities.EntityConfigs;

public class UserProjectConfig : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder.ToTable("tb_user_project");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(x => x.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();

        builder.Property(x => x.IsOwner)
            .HasColumnName("is_owner")
            .IsRequired();
        
        builder.Property(x => x.AccessTime)
            .HasColumnName("access_time")
            .IsRequired();

        builder.Property(x => x.EditTime)
            .HasColumnName("edit_time");

        builder.Property(x => x.CreateTime)
            .HasColumnName("create_time");
        
        builder.HasOne(x => x.User)
            .WithMany(u => u.UserProjects)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(x => x.ProjectId);
    }
}