using VideoGameApiVsa.Data;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Extensions;

/// <summary>
/// データベース関連の拡張メソッド
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// データベースの初期化とシードデータの投入
    /// </summary>
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VideoGameDbContext>();

        // InMemoryデータベースの作成
        await dbContext.Database.EnsureCreatedAsync();

        // すでにデータが存在する場合はスキップ
        if (dbContext.VideoGames.Any())
        {
            return;
        }

        // シードデータの投入
        dbContext.VideoGames.AddRange(
            new VideoGame
            {
                Id = 1,
                Title = "The Legend of Zelda: Breath of the Wild",
                Genre = "Action",
                ReleaseYear = 2017
            },
            new VideoGame
            {
                Id = 2,
                Title = "The Witcher 3: Wild Hunt",
                Genre = "RPG",
                ReleaseYear = 2015
            },
            new VideoGame
            {
                Id = 3,
                Title = "DOOM Eternal",
                Genre = "Shooter",
                ReleaseYear = 2020
            },
            new VideoGame
            {
                Id = 4,
                Title = "Red Dead Redemption 2",
                Genre = "Adventure",
                ReleaseYear = 2018
            },
            new VideoGame
            {
                Id = 5,
                Title = "Civilization VI",
                Genre = "Strategy",
                ReleaseYear = 2016
            }
        );

        await dbContext.SaveChangesAsync();
    }
}
