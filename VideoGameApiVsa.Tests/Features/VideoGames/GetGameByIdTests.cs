using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;
using VideoGameApiVsa.Features.VideoGames;

namespace VideoGameApiVsa.Tests.Features.VideoGames;

/// <summary>
/// GetGameById機能のテストクラス
/// </summary>
public class GetGameByIdTests
{
    /// <summary>
    /// ゲームが存在する場合、指定されたIDのゲームを返すことを確認するテスト
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnGame_WhenGameExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VideoGameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var dbContext = new VideoGameDbContext(options);
        var game = new VideoGame { Id = 1, Title = "Test Game", Genre = "Action", ReleaseYear = 2020 };
        dbContext.VideoGames.Add(game);
        await dbContext.SaveChangesAsync();

        var handler = new GetGameById.Handler(dbContext);
        var query = new GetGameById.GetGameByIdQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Test Game");
        result.Genre.Should().Be("Action");
        result.ReleaseYear.Should().Be(2020);
    }

    /// <summary>
    /// ゲームが存在しない場合、nullを返すことを確認するテスト
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenGameDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VideoGameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var dbContext = new VideoGameDbContext(options);
        var handler = new GetGameById.Handler(dbContext);
        var query = new GetGameById.GetGameByIdQuery(999);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}

