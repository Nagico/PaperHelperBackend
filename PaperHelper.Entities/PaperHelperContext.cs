using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Entities;

public class PaperHelperContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public PaperHelperContext(DbContextOptions<PaperHelperContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}