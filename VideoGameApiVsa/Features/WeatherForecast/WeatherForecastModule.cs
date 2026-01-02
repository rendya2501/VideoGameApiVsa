using Carter;

namespace VideoGameApiVsa.Features.WeatherForecast;

public sealed class WeatherForecastModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/WeatherForecast")
            .WithTags("Weather");

        // 案3
        //group.MapGet("/", GetAllGames.Endpoint);

        // 案4
        group.GetWeatherForecastEndPoint();
    }
}
