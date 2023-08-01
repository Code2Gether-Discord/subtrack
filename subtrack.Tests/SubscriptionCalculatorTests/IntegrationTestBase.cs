using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using subtrack.DAL;
using subtrack.DAL.Entities;
using Xunit;


public abstract class SqliteInMemoryControllerTest : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<SubtrackDbContext> _contextOptions;
    protected SubtrackDbContext _dbContext;

    public SqliteInMemoryControllerTest()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<SubtrackDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new SubtrackDbContext(_contextOptions);

        using var context = new SubtrackDbContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();

        }
    }

    public void Dispose() => _dbContext.Dispose();

}
