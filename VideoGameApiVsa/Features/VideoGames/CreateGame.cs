using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Features.VideoGames;

public static class CreateGame
{
    public record Command(string Title, string Genre, int ReleaseYear) : IRequest<Response>;

    public record Response(int Id, string Title, string Genre, int ReleaseYear);

    public class Handler(VideoGameDbContext dbContext) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var videoGame = new VideoGame
            {
                Title = request.Title,
                Genre = request.Genre,
                ReleaseYear = request.ReleaseYear
            };
            dbContext.VideoGames.Add(videoGame);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new Response(videoGame.Id, videoGame.Title, videoGame.Genre, videoGame.ReleaseYear);
        }
    }
}

[ApiController]
[Route("api/games")]
public class CreateGameController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateGame.Response>> CreateGame(CreateGame.Command command, CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        // return CreatedAtAction(nameof(CreateGame), new { id = response.Id }, response);
        return Ok(response);
    }
}
