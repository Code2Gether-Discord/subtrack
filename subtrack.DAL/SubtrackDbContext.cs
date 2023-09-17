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
        modelBuilder.Entity<SettingsBase>().HasDiscriminator<string>("settings_type")
            .HasValue<DateTimeSetting>(nameof(DateTimeSetting));

        modelBuilder.Entity<DateTimeSetting>().HasData(new DateTimeSetting { Id = DateTimeSetting.LastAutoPaymentTimeStampKey, Value = null });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.Property(e => e.BillingOccurrence).HasDefaultValue(BillingOccurrence.Month).ValueGeneratedNever();
            entity.Property(e => e.BillingInterval).HasDefaultValue(1).ValueGeneratedNever();
            entity.Property(e => e.BackgroundColor).HasDefaultValue("#282828").ValueGeneratedNever();
        });
    }
}
