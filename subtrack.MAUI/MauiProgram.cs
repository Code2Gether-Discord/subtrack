﻿using subtrack.DAL;
using Microsoft.EntityFrameworkCore;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Services;
using System.Runtime.CompilerServices;
using subtrack.MAUI.Shared.JsInterop;
using subtrack.MAUI.Utilities;
using System.Globalization;
using Plugin.LocalNotification;

[assembly: InternalsVisibleTo("subtrack.Tests")]

namespace subtrack.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SetupCulture();
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.UseLocalNotification();
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
            .ExecuteAsync().Wait();

        return builder.Build();
    }

    private static void SetupCulture()
    {
        CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        customCulture.NumberFormat.CurrencyDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = customCulture;
    }

    public static IServiceCollection AddSubtrackServices(this IServiceCollection services)
    {
        var dbConnectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "subtrack.db")}";
        return
            services
            .AddDbContext<SubtrackDbContext>(opt => opt.UseSqlite(dbConnectionString))
            .AddScoped<ISubscriptionService, SubscriptionService>()
            .AddScoped<ISubscriptionsImporter, SubscriptionsImporter>()
            .AddScoped<IDateProvider, DateProvider>()
            .AddScoped<ISubscriptionsCalculator, SubscriptionsCalculator>()
            .AddScoped<ISettingsService, SettingsService>()
            .AddScoped<IMonthlyPageCalculator, MonthlyPageCalculator>()
            .AddScoped<AutoPaymentHandler>()
            .AddTransient<HighlightJsInterop>();
    }

    private static void SeedDb(SubtrackDbContext dbContext)
    {
        _ = dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        var todayLastMonth = DateTime.Now.AddMonths(-1);
        var availableBackgroundColors = CssUtil.AvailableBackgroundColors;
        dbContext.Subscriptions.AddRange(
                  new DAL.Entities.Subscription() { Name = "paramount", LastPayment = todayLastMonth.AddDays(-1), FirstPaymentDay = todayLastMonth.AddDays(-1).Day, Cost = 3m, BillingOccurrence = DAL.Entities.BillingOccurrence.Week, BillingInterval = 2, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6", Icon = "fa fa-tv" },
                  new DAL.Entities.Subscription() { Name = "Disney+", LastPayment = todayLastMonth, FirstPaymentDay = todayLastMonth.Day, Cost = 3m, BillingOccurrence = DAL.Entities.BillingOccurrence.Month, BillingInterval = 1, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6", Icon = "fa fa-circle-play" },
                  new DAL.Entities.Subscription() { Name = "Netflix Premium Plan", LastPayment = DateTime.Now.AddDays(-1), FirstPaymentDay = DateTime.Now.AddDays(-1).Day, IsAutoPaid = true, Description = "family plan", Cost = 1000, BillingOccurrence = DAL.Entities.BillingOccurrence.Month, BillingInterval = 1, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6" },
                  new DAL.Entities.Subscription() { Name = "Something Family Plan", LastPayment = DateTime.Now.AddDays(-150), FirstPaymentDay = DateTime.Now.AddDays(-150).Day, IsAutoPaid = false, Description = "family plan", Cost = 1000, BillingOccurrence = DAL.Entities.BillingOccurrence.Week, BillingInterval = 2, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6", Icon = "fa fa-shield" },
                  new DAL.Entities.Subscription() { Name = "hbo", LastPayment = DateTime.Now, FirstPaymentDay = DateTime.Now.Day, Cost = 1.5m, BillingOccurrence = DAL.Entities.BillingOccurrence.Year, BillingInterval = 1, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6" },
                  new DAL.Entities.Subscription() { Name = "hulu Standard plan", LastPayment = DateTime.Now.AddMonths(-2), FirstPaymentDay = 1, Cost = 1.5m, IsAutoPaid = true, BillingOccurrence = DAL.Entities.BillingOccurrence.Month, BillingInterval = 3, PrimaryColor = availableBackgroundColors.First(), SecondaryColor = "#2a9fd6", Icon = "fa fa-circle-play" }
                  );

        dbContext.SaveChanges();
    }
}
