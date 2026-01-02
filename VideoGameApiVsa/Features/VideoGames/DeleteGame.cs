using MediatR;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Features.VideoGames;

public static class DeleteGame
{
    public record Command(int Id) : IRequest<bool>;

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command command, CancellationToken ct)
        {
            var videoGame = await dbContext.VideoGames.FindAsync([command.Id], ct);
            if (videoGame is null)
            {
                return false;
            }

            dbContext.VideoGames.Remove(videoGame);
            await dbContext.SaveChangesAsync(ct);

            return true;
        }
    }

    // 案3
    public static async Task<IResult> Endpoint(ISender sender, int id, CancellationToken ct)
    {
        var deleted = await sender.Send(new Command(id), ct);
        return deleted
            ? Results.NoContent()
            : Results.NotFound($"Video game with id {id} not found.");
    }

    // 案4
    internal static void DeleteGameEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:int}", async (ISender sender, int id, CancellationToken ct) =>
            await sender.Send(new Command(id), ct) 
                ? Results.NoContent()
                : Results.NotFound($"Video game with id {id} not found."));
    }

    //// 案2
    //public class EndPoint : ICarterModule
    //{
    //    public void AddRoutes(IEndpointRouteBuilder app)
    //    {
    //        app.MapDelete("api/games/{id:int}", async (ISender sender, int id, CancellationToken ct) =>
    //            await sender.Send(new Command(id), ct) ? Results.NoContent()
    //                : Results.NotFound($"Video game with id {id} not found."));
    //    }
    //}
}

// 案1
//[ApiController]
//[Route("api/games")]
//public class DeleteGameController(ISender sender) : ControllerBase
//{
//    [HttpDelete("{id}")]
//    public async Task<ActionResult<bool>> DeleteGame(int id, CancellationToken cancellationToken)
//    {
//        var response = await sender.Send(new DeleteGame.Command(id), cancellationToken);
//        if (!response)
//        {
//            return NotFound("Video game with given ID not found");
//        }

//        return NoContent();
//    }
//}
