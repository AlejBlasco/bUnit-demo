using FrontWasm.Pages.Interaction;
using System;
using static FrontWasm.Pages.Interaction.TriggeringCustomEvents;

namespace FrontWasm.UnitTest.Pages.Interaction;

public class TriggeringCustomEventsTest : TestContext
{
    public void CustomEvent_should_change_text()
    {
        // Arrange
        var actual = RenderComponent<TriggeringCustomEvents>();

        // Act
        actual.Find("input")
            .TriggerEvent("oncustompaste", new CustomPasteEventArgs
            {
                EventTimestamp = DateTime.Now,
                PastedData = "HELLO WORLD"
            });

        // Assert
        actual.Find("p").MarkupMatches("<p>You pasted: HELLO WORLD</p>");
    }

    [Fact]
    public void ClickEvent_should_reset_text()
    {
        // Arrange
        var actual = RenderComponent<TriggeringCustomEvents>();

        // Act
        actual.Find("button")
            .Click();

        // Assert
        actual.Find("p").MarkupMatches("<p></p>");
    }
}
