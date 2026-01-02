using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Features.VideoGames;

public static class GetAllGames
{
    public record Query : IRequest<IEnumerable<Response>>;

    public record Response(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query query, CancellationToken ct)
        {
            var videoGamses = await dbContext.VideoGames.ToListAsync(ct);
            return videoGamses.Select(vg => new Response(vg.Id, vg.Title, vg.Genre, vg.ReleaseYear));
        }
    }

    // 案3
    public static async Task<IResult> Endpoint(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new Query(), ct);
        return Results.Ok(result);
    }

    // 案4
    internal static void GetAllGamesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender, CancellationToken ct) =>
             await sender.Send(new Query(), ct));
    }

    // 案2
    //public class EndPoint : ICarterModule
    //{
    //    public void AddRoutes(IEndpointRouteBuilder app)
    //    {
    //        app.MapGet("api/games/", async (ISender sender, CancellationToken ct) =>
    //             await sender.Send(new Query(), ct));
    //    }
    //}
}

// 案1
//[ApiController]
//[Route("api/games")]
//public class GetAllGamsesController(ISender sender) : ControllerBase
//{
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<GetAllGames.Response>>> GetAllGames(CancellationToken ct)
//    {
//        var response = await sender.Send(new GetAllGames.Query(), ct);
//        return Ok(response);
//    }
//}
