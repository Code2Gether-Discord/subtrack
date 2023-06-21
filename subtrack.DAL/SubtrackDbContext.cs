using Microsoft.EntityFrameworkCore;
using subtrack.DAL.Entities;

namespace subtrack.DAL;

public class SubtrackDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }

    public SubtrackDbContext(DbContextOptions<SubtrackDbContext> options) : base(options)
    {
    }
}
