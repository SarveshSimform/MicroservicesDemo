using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleEvent> ArticleEvents { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}