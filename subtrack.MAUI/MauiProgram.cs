using subtrack.DAL;
using Microsoft.EntityFrameworkCore;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace subtrack.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        var dbConnectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "subtrack.db")}";
        builder.Services.AddDbContext<SubtrackDbContext>(opt => opt.UseSqlite(dbConnectionString));

        builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddScoped<ISubscriptionsCalculator, SubscriptionsCalculator>();

        using var sp = builder.Services.BuildServiceProvider();
        var db = sp.GetRequiredService<SubtrackDbContext>();
#if DEBUG
        SeedDb(db);
#else
            db.Database.Migrate();
#endif      
        return builder.Build();
    }

    private static void SeedDb(SubtrackDbContext dbContext)
    {
        var todayLastMonth = DateTime.Now.AddMonths(-1);
        _ = dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        dbContext.Subscriptions.AddRange(
            new DAL.Entities.Subscription() { Name = "paramount", FirstPaymentDay = todayLastMonth.AddDays(-1).Day, LastPayment =todayLastMonth.AddDays(-1), Cost = 3m, },
            new DAL.Entities.Subscription() { Name = "Disney+", FirstPaymentDay = new DateTime(2023, 02, 28).Day, LastPayment = new DateTime(2023, 02, 28), Cost = 3m, },
            new DAL.Entities.Subscription() { Name = "Netflix",FirstPaymentDay= todayLastMonth.Day, LastPayment = todayLastMonth, IsAutoPaid = true, Description = "family plan", Cost = 10 },
            new DAL.Entities.Subscription() { Name = "hbo",FirstPaymentDay = new DateTime(2023,06,3).Day, LastPayment = new DateTime(2023, 06, 30), Cost = 1.5m, }
            );

        dbContext.SaveChanges();
    }
}
