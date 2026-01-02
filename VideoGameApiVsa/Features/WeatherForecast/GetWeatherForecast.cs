using MediatR;

namespace VideoGameApiVsa.Features.WeatherForecast;

public static class GetWeatherForecast
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public record Query() : IRequest<IEnumerable<Response>>;

    public record Response(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);

    public class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        public Task<IEnumerable<Response>> Handle(Query request, CancellationToken ct)
        {
            var TemperatureC = Random.Shared.Next(-20, 55);
            var result = Enumerable.Range(1, 5).Select(index => new Response
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC,
                32 + (int)(TemperatureC / 0.5556),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ));
            return Task.FromResult(result);
        }
    }

    // 案4
    internal static void GetWeatherForecastEndPoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender, CancellationToken ct) =>
            await sender.Send(new Query(), ct));
    }
}

//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries =
//        [
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        ];

//        [HttpGet(Name = "GetWeatherForecast")]
//        public IEnumerable<WeatherForecast> Get()
//        {
//            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                TemperatureC = Random.Shared.Next(-20, 55),
//                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//    }
