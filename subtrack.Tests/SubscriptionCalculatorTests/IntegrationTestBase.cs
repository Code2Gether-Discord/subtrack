using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using Xunit;

namespace EF.Testing.UnitTests;

public class SqliteInMemoryBloggingControllerTest : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<SubtrackDbContext> _contextOptions;

    public SqliteInMemoryBloggingControllerTest()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<SubtrackDbContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new SubtrackDbContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT *
FROM Subscriptions;";
            viewCommand.ExecuteNonQuery();
        }
        context.AddRange(
            new Subscription { Id = 1, Name = "water", Cost = 12.00m, IsAutoPaid = true, LastPayment = DateTime.Today });
        context.SaveChanges();
    }

    SubtrackDbContext CreateContext() => new SubtrackDbContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

}
