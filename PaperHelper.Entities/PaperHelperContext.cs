using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities;

/// <summary>
/// 数据库管理类
/// </summary>
public class PaperHelperContext : DbContext
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Paper> Papers { get; set; }
    public DbSet<PaperReference> PaperReferences { get; set; }
    public DbSet<PaperTag> PaperTags { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserProject> UserProjects { get; set; }


    public PaperHelperContext(DbContextOptions<PaperHelperContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}