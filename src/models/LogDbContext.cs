using Microsoft.EntityFrameworkCore;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }

    public DbSet<Entity> Logs { get; set; }
    
}