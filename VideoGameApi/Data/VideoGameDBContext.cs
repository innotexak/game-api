using Microsoft.EntityFrameworkCore;
using VideoGameApi.Entities;

namespace VideoGameApi.Data
{
    public class VideoGameDBContext(DbContextOptions<VideoGameDBContext> options) : DbContext(options)
    {
        public DbSet<VideoGame> VideoGames => Set<VideoGame>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VideoGame>().HasData(
                 new VideoGame
                 {
                     Id = 1,
                     Title = "Spider-Man 2",
                     Platform = "PS5",
                     Developer = "Insomniac Games",
                     Publisher = "Sony Interative Entertainment"
                 }
                );
            
        }
    }
}
 