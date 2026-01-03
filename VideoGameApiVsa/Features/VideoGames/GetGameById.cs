using MediatR;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Features.VideoGames;

public static class GetGameById
{
    public record GetGameByIdQuery(int Id) : IRequest<GetGameByIdResponse?>;

    public record GetGameByIdResponse(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<GetGameByIdQuery, GetGameByIdResponse?>
    {
        public async Task<GetGameByIdResponse?> Handle(GetGameByIdQuery query, CancellationToken ct)
        {
            var videoGame = await dbContext.VideoGames.FindAsync([query.Id], ct);
            if (videoGame is null)
            {
                return null;
            }

            return new GetGameByIdResponse(videoGame.Id, videoGame.Title, videoGame.Genre, videoGame.ReleaseYear);
        }
    }

    public static async Task<IResult> Endpoint(ISender sender, int id, CancellationToken ct)
    {
        var result = await sender.Send(new GetGameByIdQuery(id), ct);

        if (result is null)
            return Results.NotFound($"Video game with id {id} not found.");

        return Results.Ok(result);
    }
}
