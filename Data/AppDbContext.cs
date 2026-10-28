using Microsoft.EntityFrameworkCore;
using SportConnect.Models;

namespace SportConnect.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Participation> Participations => Set<Participation>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Participation>()
            .HasKey(p => new { p.ActivityId, p.UserId });
    }
}
