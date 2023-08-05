using Microsoft.EntityFrameworkCore;
using subtrack.DAL.Entities;

namespace subtrack.DAL;

public class SubtrackDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SettingsBase> Settings { get; set; }

    public SubtrackDbContext(DbContextOptions<SubtrackDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SettingsBase>().HasDiscriminator<string>("settings_type").HasValue<DateTimeSetting>(nameof(DateTimeSetting));
    }
}
