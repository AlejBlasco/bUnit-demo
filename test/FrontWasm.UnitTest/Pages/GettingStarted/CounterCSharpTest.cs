using FrontWasm.Pages.GettingStarted;

namespace FrontWasm.UnitTest.Pages.GettingStarted;

public class CounterCSharpTest : TestContext
{
    [Fact]
    public void Counter_should_start_at_zero()
    {
        // Arrange
        var actual = RenderComponent<Counter>();

        // Act

        // Assert
        actual.Find("p")
            .MarkupMatches("<p role=\"status\">Current count: 0</p>");
    }

    [Fact]
    public void OnClick_should_increment_number()
    {
        // Arrange
        var actual = RenderComponent<Counter>();

        // Act
        actual.Find("button")
            .Click();

        // Assert
        actual.Find("p")
            .MarkupMatches("<p role=\"status\">Current count: 1</p>");
    }
}
