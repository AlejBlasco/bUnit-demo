using FluentAssertions;
using FrontWasm.Pages.ProvidingInput;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace FrontWasm.UnitTest.Pages.ProvidingInput;

public class EventCallbackParamsTest : TestContext
{
    [Fact]
    public void Callback_not_throw_any_exception()
    {
        // Arrange
        Action<MouseEventArgs> onClickHandler = _ => { };

        var actual = RenderComponent<EventCallbackParams>(parameters => parameters
          .Add(p => p.OnClickHandler, onClickHandler)
        );

        // Act
        var act = () =>
        {
            actual.Find("button")
                .Click();
        };

        // Assert
        act.Should().NotThrow<Exception>();
    }
}
