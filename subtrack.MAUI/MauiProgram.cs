using subtrack.MAUI.Data;
using subtrack.DAL;
using Microsoft.EntityFrameworkCore;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Services;

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

        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

        using var sp = builder.Services.BuildServiceProvider();
        SeedDb(sp.GetRequiredService<SubtrackDbContext>());

        return builder.Build();
    }

    private static void SeedDb(SubtrackDbContext dbContext)
    {
        _ = dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        dbContext.Subscriptions.AddRange(
            new DAL.Entities.Subscription() { Name = "Netflix", IsAutoPaid = true, LastPayment = DateTime.Now.AddDays(-1), Description = "family plan", Cost = 10 },
            new DAL.Entities.Subscription() { Name = "hbo", LastPayment = DateTime.Now, Cost = 1.5m, }
            );

        dbContext.SaveChanges();
    }
}
