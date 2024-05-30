using Plugin.LocalNotification;

namespace subtrack.MAUI.Utilities;
public static class NotificationsUtil
{
    public static async Task EnsureNotificationsAreEnabled()
    {
        var hasEnabledNotifications = await LocalNotificationCenter.Current.AreNotificationsEnabled();
        if (!hasEnabledNotifications)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
    }

    public static async Task<bool> HasEnabledNotifications() => await LocalNotificationCenter.Current.AreNotificationsEnabled();
}
