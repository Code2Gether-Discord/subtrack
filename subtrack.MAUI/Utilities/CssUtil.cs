namespace subtrack.MAUI.Utilities;

public static class CssUtil
{
    internal static readonly IEnumerable<string> AvailableBackgroundColors = new[]
    {
        "#282828",
        "#281439",
        "#34126d",
        "#38354a",
        "#22202c",
        "#4b013b",
        "#141c39",
        "#34424c",
        "#203d50",
        "#13516d",
        "#050505",
        "#242424",
        "#2e2e2e",
        "#2d0903"
    };
    public static string GetDueClass(int daysUntilDue) =>
        daysUntilDue switch
        {
            < 0 => "overdue",
            (>= 0) and (<= 2) => "text-warning",
            _ => string.Empty,
        };

}
