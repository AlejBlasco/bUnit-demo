using Bunit.Extensions.WaitForHelpers;
using FluentAssertions;
using FrontWasm.Pages.Interaction;
using System;
using System.Threading.Tasks;

namespace FrontWasm.UnitTest.Pages.Interaction;

public class AsynchronousStateChangeTest : TestContext
{
    [Fact]
    public void Component_wait_for_state_should_work()
    {
        // Arrange
        var textService = new TaskCompletionSource<string>();
        var actual = RenderComponent<AsyncData>(parameters => parameters
          .Add(p => p.TextService, textService.Task)
        );

        // Act
        textService.SetResult("Hello World");
        actual.WaitForState(() => actual.Find("p").TextContent == "Hello World");

        // Assert
        actual.Markup.Should().BeEquivalentTo("<p>Hello World</p>");
    }

    [Fact]
    public void Component_wait_for_state_should_throw_exception_for_timeout()
    {
        // Arrange
        var textService = new TaskCompletionSource<string>();
        var actual = RenderComponent<AsyncData>(parameters => parameters
          .Add(p => p.TextService, textService.Task)
        );

        // Act
        textService.SetResult("Hello World");
        var act = () => {
            actual.WaitForState(() => actual.Find("p").TextContent == "Long time", TimeSpan.FromSeconds(1));
        };
        
        // Assert
        act.Should().Throw<WaitForFailedException>();
    }
}
