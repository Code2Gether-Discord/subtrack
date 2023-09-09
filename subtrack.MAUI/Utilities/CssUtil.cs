namespace subtrack.MAUI.Utilities;

public static class CssUtil
{
    public static string GetDueClass(int daysUntilDue) =>
        daysUntilDue switch
        {
            <= 0 => ".overdue",
            (> 0) and (<= 2) => ".text-warning",
            _ => string.Empty,
        };

}
