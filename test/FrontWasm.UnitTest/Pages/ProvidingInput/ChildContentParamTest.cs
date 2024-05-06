using AngleSharp;
using AngleSharp.Dom;
using FluentAssertions;
using FrontWasm.Pages.ProvidingInput;

namespace FrontWasm.UnitTest.Pages.ProvidingInput;

public class ChildContentParamTest : TestContext
{
    [Fact]
    public void Component_should_render_childcontent_if_its_passed()
    {
        // Arrange
        var child = "<h1>Hello World</h1>";

        var actual = RenderComponent<ChildContentParam>(parameters => parameters
          .AddChildContent("<h1>Hello World</h1>")
        );

        // Act
        var childMarkup = actual.Find("h1")
            .ToHtml();

        // Assert
        childMarkup.Should().NotBeNullOrWhiteSpace();
        childMarkup.Should().BeEquivalentTo(child);
    }

    [Fact]
    public void Component_should_not_render_childcontent_if_its_null()
    {
        // Arrange
        var actual = RenderComponent<ChildContentParam>();

        // Act
        IElement? child = null;
        var act = () =>
        {
            child = actual.Find("h1");
        };

        // Assert
        act.Should().Throw<ElementNotFoundException>();
        child.Should().BeNull();
    }
}
