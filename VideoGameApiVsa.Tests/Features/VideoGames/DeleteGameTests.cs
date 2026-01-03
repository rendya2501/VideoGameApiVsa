using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;
using VideoGameApiVsa.Features.VideoGames;

namespace VideoGameApiVsa.Tests.Features.VideoGames;

/// <summary>
/// DeleteGame機能のテストクラス
/// </summary>
public class DeleteGameTests
{
    /// <summary>
    /// ゲームが存在する場合、ゲームを削除することを確認するテスト
    /// </summary>
    [Fact]
    public async Task Handle_ShouldDeleteGame_WhenGameExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VideoGameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var dbContext = new VideoGameDbContext(options);
        var game = new VideoGame { Id = 1, Title = "Game to Delete", Genre = "Action", ReleaseYear = 2020 };
        dbContext.VideoGames.Add(game);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteGame.Handler(dbContext);
        var command = new DeleteGame.DeleteGameCommand(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        var gameInDb = await dbContext.VideoGames.FindAsync([1]);
        gameInDb.Should().BeNull();
    }

    /// <summary>
    /// ゲームが存在しない場合、falseを返すことを確認するテスト
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenGameDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VideoGameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var dbContext = new VideoGameDbContext(options);
        var handler = new DeleteGame.Handler(dbContext);
        var command = new DeleteGame.DeleteGameCommand(999);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}

