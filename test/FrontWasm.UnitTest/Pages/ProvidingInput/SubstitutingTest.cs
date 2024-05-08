using FluentAssertions;
using FrontWasm.Pages.ProvidingInput;
using Moq;
using System;

namespace FrontWasm.UnitTest.Pages.ProvidingInput;

public class SubstitutingTest : TestContext
{
    [Fact]
    public void Foo_does_not_have_a_bar_but_stub()
    {
        // Arrange
        ComponentFactories.AddStub<Bar>();
        var actual = RenderComponent<Substituting>();

        // Act

        // Assert
        actual.Should().NotBeNull();

        actual.HasComponent<Bar>().Should().BeFalse();
        actual.HasComponent<Stub<Bar>>().Should().BeTrue();
    }

    [Fact]
    public void Foo_does_not_have_a_bar_but_substituded_markup()
    {
        // Arrange
        ComponentFactories.AddStub<Bar>("<div>NOT FROM BAR</div>");
        var actual = RenderComponent<Substituting>();

        // Act

        // Assert
        actual.Should().NotBeNull();

        actual.HasComponent<Bar>().Should().BeFalse();
        actual.HasComponent<Stub<Bar>>().Should().BeTrue();
    }

    [Fact]
    public void Foo_does_not_have_a_bar_but_Mock()
    {
        // Arrange
        var mockBar = new Mock<Bar>();
        ComponentFactories.Add<Bar>(mockBar.Object);

        var actual = RenderComponent<Substituting>();

        // Act
        IRenderedComponent<Bar> renderedBar = actual.FindComponent<Bar>();

        // Assert
        actual.Should().NotBeNull();

        renderedBar.Should().NotBeNull();
        renderedBar.Instance.Should().BeEquivalentTo(mockBar.Object);
    }
}
