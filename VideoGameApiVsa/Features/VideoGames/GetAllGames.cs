using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Features.VideoGames;

public static class GetAllGames
{
    public record GetAllGamesQuery : IRequest<IEnumerable<GetAllGamesResponse>>;

    public record GetAllGamesResponse(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<GetAllGamesQuery, IEnumerable<GetAllGamesResponse>>
    {
        public async Task<IEnumerable<GetAllGamesResponse>> Handle(GetAllGamesQuery query, CancellationToken ct)
        {
            var videoGames = await dbContext.VideoGames.ToListAsync(ct);
            return videoGames.Select(vg => new GetAllGamesResponse(vg.Id, vg.Title, vg.Genre, vg.ReleaseYear));
        }
    }

    public static async Task<IResult> Endpoint(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetAllGamesQuery(), ct);
        return Results.Ok(result);
    }
}
