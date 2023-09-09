using subtrack.MAUI.Utilities;

namespace subtrack.Tests.UtilitiesTests;

public class GetDueClassTests
{
    [Theory]
    [InlineData(-1, ".overdue")]
    [InlineData(0, ".overdue")]
    [InlineData(1, ".text-warning")]
    [InlineData(2, ".text-warning")]
    [InlineData(3, "")]
    public void GetDueClass_ReturnsExpectedResult(int daysUntilDue, string expected)
    {
        // Arrange
        // No need to arrange anything as this is a pure function.

        // Act
        string result = CssUtil.GetDueClass(daysUntilDue);

        // Assert
        Assert.Equal(expected, result);
    }
}
