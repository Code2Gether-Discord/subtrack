using Humanizer;
using Plugin.LocalNotification;
using Shiny.Jobs;
using subtrack.DAL.Entities;
using subtrack.MAUI.Services.Abstractions;
using subtrack.MAUI.Utilities;

namespace subtrack.MAUI.Services.Android;

// the user might need to turn off battery optimizations for subtrack in settings->apps->subtrack->battery saver->no restrictions
public class NotifyDueSubscriptionsJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotifyDueSubscriptionsJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    private static void SendNotification(int notificationId, string title, string group)
    {
        var sampleNotification = new NotificationRequest()
        {
            NotificationId = notificationId,
            Title = title,
            CategoryType = NotificationCategoryType.Reminder,
            Group = group,
        };

        LocalNotificationCenter.Current.Show(sampleNotification);
    }

    public async Task Run(JobInfo jobInfo, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            using var scope = _serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            var dateProvider = serviceProvider.GetRequiredService<IDateProvider>();

            var lastSubscriptionReminderTimeStamp = await settingsService.GetByIdAsync<DateTimeSetting>(DateTimeSetting.LastSubscriptionReminderTimeStampKey);

            var now = dateProvider.Now;
            var today = dateProvider.Today;

            if (lastSubscriptionReminderTimeStamp.Value?.Date != today)
            {
                var subscriptionService = serviceProvider.GetRequiredService<ISubscriptionService>();
                var subscriptionsCalculator = serviceProvider.GetRequiredService<ISubscriptionsCalculator>();

                var subscriptions = await subscriptionService.GetAllAsync();
                await NotifyUpcomingPayments(subscriptions, today, subscriptionsCalculator);
                await NotifyMissedPaymentsAsync(subscriptions, subscriptionsCalculator, today);

                lastSubscriptionReminderTimeStamp.Value = now;
                await settingsService.UpdateAsync(lastSubscriptionReminderTimeStamp);
            }

            var tomorrow5PM = today.AddDays(1).AddHours(17);
            var timeUntilTomorrow5PM = tomorrow5PM - now;

            await Task.Delay(timeUntilTomorrow5PM, cancellationToken);
        }
    }

    private static async Task NotifyUpcomingPayments(IEnumerable<Subscription> subscriptions, DateTime today, ISubscriptionsCalculator subscriptionsCalculator)
    {
        var subscriptionsWithNotificationsEnabled = subscriptions.Where(x => x.NotificationDays.HasValue).ToList();
        if (!subscriptionsWithNotificationsEnabled.Any())
        {
            return;
        }

        await NotificationsUtil.EnsureNotificationsAreEnabled();

        var notificationGroup = $"upcoming {today.Day}";
        subscriptionsWithNotificationsEnabled.ForEach(sub =>
        {
            var dueDate = subscriptionsCalculator.GetNextPaymentDate(sub);
            var timeUntilNextPayment = dueDate.Subtract(today);
            var dueDays = timeUntilNextPayment.Days;

            if (dueDays == sub.NotificationDays)
            {
                SendNotification(sub.Id, $"{sub.Name} is due {GetDueDaysText(dueDays)} ({dueDate.DayOfWeek.Humanize(LetterCasing.LowerCase)})", notificationGroup);
            }
        });
    }

    private static async Task NotifyMissedPaymentsAsync(IEnumerable<Subscription> subscriptions, ISubscriptionsCalculator subscriptionsCalculator, DateTime today)
    {
        if (!await NotificationsUtil.HasEnabledNotifications())
        {
            return;
        }

        foreach (var sub in subscriptions)
        {
            var dueDate = subscriptionsCalculator.GetNextPaymentDate(sub);
            if (dueDate.IsPastDate(today))
            {
                SendNotification(sub.Id, $"Missed payment for {sub.Name}", $"missed {today.Day}");
            }
        }
    }

    private static string GetDueDaysText(int dueDays)
    {
        return dueDays switch
        {
            0 => "today",
            1 => "tomorrow",
            _ => $"in {dueDays} days"
        };
    }
}
