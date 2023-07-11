
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    
    public DbSet<EventInfo> EventInfos { get; set; } = default!;
    public DbSet<Participator> Participators { get; set; } = default!;
    public DbSet<EventParticipator> EventParticipators { get; set; } = default!;
    public DbSet<Person> Persons { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}