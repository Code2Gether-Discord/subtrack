using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace subtrack.DAL;

internal class DbContextMigrationFactory : IDesignTimeDbContextFactory<SubtrackDbContext>
{
    public SubtrackDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SubtrackDbContext>();

        // database is located in \AppData\Local
        var dbConnectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "subtrack.db")}";
        builder.UseSqlite(dbConnectionString);
        return new SubtrackDbContext(builder.Options);
    }
}
