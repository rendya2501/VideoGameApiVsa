using MediatR;
using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Features.VideoGames;

public static class CreateGame
{
    /// <summary>
    /// リクエスト用DTO（OpenAPI/Scalarで表示される）
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Genre"></param>
    /// <param name="ReleaseYear"></param>
    public record Request(string Title, string Genre, int ReleaseYear);

    /// <summary>
    /// MediatRコマンド（内部使用のみ）
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Genre"></param>
    /// <param name="ReleaseYear"></param>
    public record Command(string Title, string Genre, int ReleaseYear) : IRequest<Response>;

    public record Response(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var videoGame = new VideoGame
            {
                Title = command.Title,
                Genre = command.Genre,
                ReleaseYear = command.ReleaseYear
            };

            dbContext.VideoGames.Add(videoGame);

            await dbContext.SaveChangesAsync(ct);

            return new Response(videoGame.Id, videoGame.Title, videoGame.Genre, videoGame.ReleaseYear);
        }
    }

    // 案3
    public static async Task<IResult> Endpoint(ISender sender, Request request, CancellationToken ct)
    {
        var command = new Command(request.Title, request.Genre, request.ReleaseYear);
        var result = await sender.Send(command, ct);
        //return Results.Created($"/api/games/{result.Id}", result);
        return Results.CreatedAtRoute(
            routeName: "GetGameById",
            routeValues: new { id = result.Id },
            value: result);
    }

    // 案4
    internal static void CreateGameEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (ISender sender, Request request, CancellationToken ct) =>
        {
            var command = new Command(request.Title, request.Genre, request.ReleaseYear);
            var result = await sender.Send(command, ct);
            // return Results.Created($"/api/games/{game.Id}", game);
            return Results.CreatedAtRoute(
                routeName: "GetGameById",
                routeValues: new { id = result.Id },
                value: result);
        });
    }

    // 案2
    //public class EndPoint : ICarterModule
    //{
    //    public void AddRoutes(IEndpointRouteBuilder app)
    //    {
    //        app.MapPost("api/games/", async (ISender sender, Command command, CancellationToken ct) =>
    //        {
    //            var game = await sender.Send(command, ct);
    //            return Results.Created($"/api/games/{game.Id}", game);
    //        });
    //    }
    //}
}


// 案1
//[ApiController]
//[Route("api/games")]
//public class CreateGameController(ISender sender) : ControllerBase
//{
//    [HttpPost]
//    public async Task<ActionResult<CreateGame.Response>> CreateGame(CreateGame.Command command, CancellationToken ct)
//    {
//        var response = await sender.Send(command, ct);
//        // return CreatedAtAction(nameof(CreateGame), new { id = response.Id }, response);
//        return Ok(response);
//    }
//}
