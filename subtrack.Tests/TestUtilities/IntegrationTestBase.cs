using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using subtrack.DAL;
using subtrack.MAUI;

namespace subtrack.Tests.TestUtilities;

/// <summary>
/// Allows us simulate a real scenario with actual services used in the project and a in memory database
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
    protected SubtrackDbContext _dbContext;
    protected ServiceProvider _serviceProvider;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly SqliteConnection _dbConnection;

    protected IntegrationTestBase()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        _dbConnection = new SqliteConnection("Filename=:memory:");
        _dbConnection.Open();

        // adds services used in subtrack except the SQLite file database
        _ = _services
            .AddSubtrackServices()
            .RemoveAll<SubtrackDbContext>()
            .RemoveAll<DbContextOptions<SubtrackDbContext>>()
            .RemoveAll<DbContextOptions>()
            .AddDbContext<SubtrackDbContext>(opt => opt.UseSqlite(_dbConnection));

        OnServicesRegistered();
        _serviceProvider = _services.BuildServiceProvider();
        _dbContext = _serviceProvider.GetRequiredService<SubtrackDbContext>();
        _dbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        _dbConnection.Close();
    }

    /// <summary>
    /// Replace a real service with a decorator. Can be used when wanting to count the amount of times a method is called
    /// </summary>
    protected void AddDecorator<TKey, TImplementation, TDecorator>(ServiceLifetime scope = ServiceLifetime.Singleton)
        where TDecorator : class
        where TKey : class
    {
        _services.RemoveAll<TKey>();

        _services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), scope));
        _services.Add(new ServiceDescriptor(typeof(TKey), sp => Activator.CreateInstance(typeof(TDecorator), sp.GetRequiredService<TImplementation>()), scope));
    }

    /// <summary>
    /// Called before building the service provider. Can be used for overriding existing services with fakes or decorators
    /// </summary>
    public virtual void OnServicesRegistered() { }

    protected T[] SaveEntities<T>(params T[] entities)
    {
        foreach (var entity in entities)
            _dbContext.Add(entity);

        _dbContext.SaveChanges();
        return entities;
    }

    /// <summary>
    /// Helper to ensure that creating a sub does not cause a sql exception
    /// </summary>
    protected Subscription CreateSubscription(DateTime lastPayment, int? firstPaymentDay = null, decimal cost = 0, bool autoPaid = false, string name = null, string description = null)
    {
        return new Subscription()
        {
            Name = name ?? Guid.NewGuid().ToString(),
            LastPayment = lastPayment,
            FirstPaymentDay = firstPaymentDay ?? lastPayment.Day,
            Description = description,
            Cost = cost,
            IsAutoPaid = autoPaid,
            BillingInterval = 1,
            BillingOccurrence = BillingOccurrence.Month,
            BackgroundColor = "#fff"
        };
    }
}
