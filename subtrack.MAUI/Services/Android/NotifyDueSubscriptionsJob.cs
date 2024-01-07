using Plugin.LocalNotification;
using Shiny.Jobs;

namespace subtrack.MAUI.Services.Android;

// the user might need to turn off battery optimizations for subtrack in settings->apps->subtrack->battery saver->no restrictions
public class NotifyDueSubscriptionsJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotifyDueSubscriptionsJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    private static async Task EnsureNotificationsAreEnabled()
    {
        var hasEnabledNotifications = await LocalNotificationCenter.Current.AreNotificationsEnabled();
        if (!hasEnabledNotifications)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
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

            // jobs are singletons, use this to create a scope for fetching transient/scoped dependencies
            using var scope = _serviceScopeFactory.CreateScope();

            await EnsureNotificationsAreEnabled();

            var notificationGroup = DateTime.Today.Day.ToString();
            SendNotification(1, "Netflix is due in 2 days", notificationGroup);
            SendNotification(2, "Missed payment for disney", notificationGroup);

            await Task.Delay(TimeSpan.FromHours(12), cancellationToken);
        }
    }
}
