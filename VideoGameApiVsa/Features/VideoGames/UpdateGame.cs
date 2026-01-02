using MediatR;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Features.VideoGames;

public static class UpdateGame
{
    public record Command(int Id, string Title, string Genre, int ReleaseYear) : IRequest<Response?>;

    public record Response(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<Command, Response?>
    {
        public async Task<Response?> Handle(Command command, CancellationToken ct)
        {
            var videoGame = await dbContext.VideoGames.FindAsync([command.Id], ct);
            if (videoGame is null)
            {
                return null;
            }

            videoGame.Title = command.Title;
            videoGame.Genre = command.Genre;
            videoGame.ReleaseYear = command.ReleaseYear;

            await dbContext.SaveChangesAsync(ct);
            return new Response(videoGame.Id, videoGame.Title, videoGame.Genre, videoGame.ReleaseYear);
        }
    }

    // 案3
    public static async Task<IResult> Endpoint(ISender sender, int id, Command command, CancellationToken ct)
    {
        var updatedGame = await sender.Send(command with { Id = id }, ct);
        return updatedGame is not null
            ? Results.Ok(updatedGame)
            : Results.NotFound($"Video game with id {id} not found.");
    }

    // 案4
    internal static void UpdateGameEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:int}", async (ISender sender, Command command, int id, CancellationToken ct) =>
        {
            var updatedGame = await sender.Send(command with { Id = id }, ct);
            return updatedGame is not null
                ? Results.Ok(updatedGame)
                : Results.NotFound($"Video game with id {id} not found.");
        });
    }

    //// 案2
    //public class EndPoint : ICarterModule
    //{
    //    public void AddRoutes(IEndpointRouteBuilder app)
    //    {
    //        app.MapPut("api/games/{id:int}", async (ISender sender, Command command, int id, CancellationToken ct) =>
    //        {
    //            var updatedGame = await sender.Send(command with { Id = id }, ct);
    //            return updatedGame is not null ? Results.Ok(updatedGame)
    //                : Results.NotFound($"Video game with id {id} not found.");
    //        });
    //    }
    //}
}

// 案1
//[ApiController]
//[Route("api/games")]
//public class UpdateGameController(ISender sender) : ControllerBase
//{
//    [HttpPut("{id}")]
//    public async Task<ActionResult<UpdateGame.Response>> UpdateGame(int id, UpdateGame.Command command, CancellationToken cancellationToken)
//    {
//        if (id != command.Id)
//        {
//            return BadRequest("ID in URL does not match ID in request body.");
//        }

//        var response = await sender.Send(command, cancellationToken);
//        if (response == null)
//        {
//            return NotFound("Video game with given In not found");
//        }

//        return Ok(response);
//    }
//}
