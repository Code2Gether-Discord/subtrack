using subtrack.DAL;
using Microsoft.EntityFrameworkCore;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using subtrack.DAL.Migrations;

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
        _ = builder.Services.AddSubtrackServices();
        using var sp = builder.Services.BuildServiceProvider();
        var db = sp.GetRequiredService<SubtrackDbContext>();
#if DEBUG
        SeedDb(db);
#else
            db.Database.Migrate();
#endif      
        sp.GetRequiredService<AutoPaymentHandler>()
            .ExecuteAsync()
            .RunSynchronously();

        return builder.Build();
    }

    public static IServiceCollection AddSubtrackServices(this IServiceCollection services)
    {
        var dbConnectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "subtrack.db")}";
        return
            services
            .AddDbContext<SubtrackDbContext>(opt => opt.UseSqlite(dbConnectionString))
            .AddScoped<ISubscriptionService, SubscriptionService>()
            .AddScoped<IDateProvider, DateProvider>()
            .AddScoped<ISubscriptionsCalculator, SubscriptionsCalculator>()
            .AddScoped<ISettingsService, SettingsService>()
            .AddScoped<AutoPaymentHandler>();
    }

    private static void SeedDb(SubtrackDbContext dbContext)
    {
        var todayLastMonth = DateTime.Now.AddMonths(-1);
        _ = dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        dbContext.Subscriptions.AddRange(
                  new DAL.Entities.Subscription() { Name = "paramount", LastPayment = todayLastMonth.AddDays(-1), FirstPaymentDay = todayLastMonth.AddDays(-1).Day, Cost = 3m, },
                  new DAL.Entities.Subscription() { Name = "Disney+", LastPayment = todayLastMonth, FirstPaymentDay = todayLastMonth.Day, Cost = 3m, },
                  new DAL.Entities.Subscription() { Name = "Netflix", LastPayment = DateTime.Now.AddDays(-1), FirstPaymentDay = DateTime.Now.AddDays(-1).Day, IsAutoPaid = true, Description = "family plan", Cost = 10 },
                  new DAL.Entities.Subscription() { Name = "hbo", LastPayment = DateTime.Now, FirstPaymentDay = DateTime.Now.Day, Cost = 1.5m, }
                  );


        dbContext.SaveChanges();
    }
}
