using subtrack.MAUI.Utilities;

namespace subtrack.Tests.Utilities;

public class CssUtilTests
{
    [Theory]
    [InlineData(-1, "overdue")]
    [InlineData(0, "text-warning")]
    [InlineData(1, "text-warning")]
    [InlineData(2, "text-warning")]
    public void GetDueClass_ReturnsExpectedResult(int daysUntilDue, string expected)
    {
        string result = CssUtil.GetDueClass(daysUntilDue);

        Assert.Equal(expected, result);
    }
}
