using FluentAssertions;
using FrontWasm.Pages.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontWasm.UnitTest.Pages.Interaction;

public class TriggeringRendersTest : TestContext
{
    [Fact]
    public void Render_should_increment_render_count()
    {
        // Arrange
        var actual = RenderComponent<TriggeringRenders>();

        // Act
        int initCount = actual.RenderCount;
        actual.Render();

        // Assert
        actual.Should().NotBeNull();

        initCount.Should().Be(1);
        actual.RenderCount.Should().Be(2);
    }

    [Fact]
    public void Render_should_set_new_params_value()
    {
        // Arrange
        var actual = RenderComponent<TriggeringRenders>(parameters => parameters
          .Add(p => p.Value, "Foo")
        );

        // Act
        var initMarkup = actual.Markup;
        actual.SetParametersAndRender(parameters => parameters
          .Add(p => p.Value, "Bar")
        );

        // Assert
        actual.Should().NotBeNull();

        initMarkup.Should().BeEquivalentTo("<span>Foo</span>");
        actual.Markup.Should().BeEquivalentTo("<span>Bar</span>");
    }

    [Fact]
    public void InvokeAsync_should_work()
    {
        // Arrange
        var actual = RenderComponent<Calc>();

        // Act
        actual.InvokeAsync(() => actual.Instance.Calculate(1, 2));

        // Assert
        actual.Should().NotBeNull();
        actual.Markup.Should().BeEquivalentTo("<output>3</output>");
    }

    [Fact]
    public async Task InvokeAsync_should_work_and_return_value()
    {
        // Arrange
        var actual = RenderComponent<Calc>();

        // Act
        var result = await actual.InvokeAsync(() => actual.Instance.CalculateAndReturn(1, 2));


        // Assert
        actual.Should().NotBeNull();
        actual.Markup.Should().BeEquivalentTo("<output>3</output>");

        result.Should().Be(3);
    }

    [Fact]
    public async Task InvokeAsync_should_gets_loading()
    {
        // Arrange
        var actual = RenderComponent<CalcWithLoading>();

        // Act
        await actual.InvokeAsync<Task>(() => actual.Instance.Calculate(1, 2));

        // Assert
        actual.Markup.Should().BeEquivalentTo("<output>Loading</output>");
    }

    [Fact]
    public async Task InvokeAsync_should_gets_result_on_complete_task()
    {
        // Arrange
        var actual = RenderComponent<CalcWithLoading>();

        // Act
        var task = await actual.InvokeAsync<Task>(() => actual.Instance.Calculate(1, 2));
        await task;

        // Assert
        actual.Markup.Should().BeEquivalentTo("<output>3</output>");
    }
}
