using AngleSharp;
using FluentAssertions;
using FrontWasm.Pages;
using System;
using System.Collections.Generic;

namespace FrontWasm.UnitTest.Pages;

public class DisplayListTest : TestContext
{
    [Fact]
    public void List_should_get_parameters()
    {
        // Arrange
        var items = new List<string> { "Hello", "World" };
        var number = new Random().Next(1, 10);

        var actual = RenderComponent<DisplayList>(parameters => parameters
            .Add(p => p.Number, number)
            .Add(p => p.Items, items)
            );

        // Act
        var numberMarkup = actual.Find("p")
            .ToMarkup();

        var itemsMarkup = actual.Find("ul").ToHtml();

        // Assert
        numberMarkup.Should().NotBeNullOrEmpty();
        numberMarkup.Trim().Should().BeEquivalentTo($"<p>Your number is: {number}</p>");

        itemsMarkup.Should().NotBeNullOrEmpty();
        itemsMarkup.Trim().Should().BeEquivalentTo("<ul><li>Hello</li><li>World</li></ul>");
    }
}
