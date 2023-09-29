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

    internal static readonly IEnumerable<string> AvailableIcons = new[]
    {
        "fa-solid fa-music",
        "fa-solid fa-house",
        "fa-solid fa-lock",
        "fa-brands fa-spotify",
        "fa-solid fa-clapperboard",
        "fa-solid fa-shield",
        "fa-solid fa-shield-heart",
        "fa-solid fa-key",
        "fa-solid fa-credit-card",
        "fa-solid fa-mobile",
        "fa-solid fa-book",
        "fa-solid fa-lines-leaning",
        "fa-solid fa-gamepad",
        "fa-solid fa-cloud-arrow-up",
        "fa-solid fa-circle-play",
        "fa-solid fa-tv",
        "fa-solid fa-dumbbell",
        "fa-solid fa-wifi",
        "fa-solid fa-bolt",
        "fa-solid fa-droplet",
        "fa-solid fa-arrow-trend-up",
        "fa-solid fa-money-bill",
        "fa-solid fa-prescription-bottle-medical",
        "fa-solid fa-spa",
        "fa-solid fa-scissors",
        "fa-solid fa-bicycle"
    };

    public static string GetDueClass(int daysUntilDue) =>
        daysUntilDue switch
        {
            < 0 => "overdue",
            (>= 0) and (<= 2) => "text-warning",
            _ => string.Empty,
        };

}
