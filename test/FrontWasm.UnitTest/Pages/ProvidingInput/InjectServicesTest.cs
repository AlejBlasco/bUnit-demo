using FluentAssertions;
using FrontWasm.Components;

namespace FrontWasm.UnitTest.Pages.ProvidingInput
{
    public class InjectServicesTest : TestContext
    {
        [Fact]
        public void Component_should_inject_services()
        {
            // Arrange
            Services.AddSingleton<IWeatherForecastService>(new WeatherForecastService());
            var actual = RenderComponent<InjectServices>();

            // Act

            // Assert
            actual.Instance.forecasts.Should().NotBeNullOrEmpty();
        }
    }
}
