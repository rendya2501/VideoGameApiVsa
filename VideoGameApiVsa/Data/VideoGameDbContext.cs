using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Data;

public class VideoGameDbContext(DbContextOptions<VideoGameDbContext> options) : DbContext(options)
{
    public DbSet<VideoGame> VideoGames => Set<VideoGame>();

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // エンティティの初期データを定義
    //    modelBuilder.Entity<VideoGame>().HasData(
    //        new VideoGame { Id = 1, Genre = "Action", Title = "The Legend of Zelda: Breath of the Wild", ReleaseYear = 2017 },
    //        new VideoGame { Id = 2, Genre = "RPG", Title = "The Witcher 3: Wild Hunt", ReleaseYear = 2015 },
    //        new VideoGame { Id = 3, Genre = "Shooter", Title = "DOOM Eternal", ReleaseYear = 2020 },
    //        new VideoGame { Id = 4, Genre = "Adventure", Title = "Red Dead Redemption 2", ReleaseYear = 2018 },
    //        new VideoGame { Id = 5, Genre = "Strategy", Title = "Civilization VI", ReleaseYear = 2016 }
    //    );
    //}
}
