using Microsoft.EntityFrameworkCore;
using VideoGameApi.Data;
using VideoGameApi.Entities;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Services
{
    public class VideoGameService : IVideoGameService
    {
        private readonly VideoGameDBContext _context;

        public VideoGameService(VideoGameDBContext context)
        {
            _context = context;
        }

        public async Task<List<VideoGame>> GetAllAsync()
        {
            return await _context.VideoGames.ToListAsync();
        }

        public async Task<VideoGame?> GetByIdAsync(int id)
        {
            return await _context.VideoGames.FindAsync(id);
        }

        public async Task<VideoGame> AddAsync(VideoGame newVideoGame)
        {
            _context.VideoGames.Add(newVideoGame);
            await _context.SaveChangesAsync();
            return newVideoGame;
        }

        public async Task<bool> UpdateAsync(int id, VideoGame updatedVideoGame)
        {
            var existingGame = await _context.VideoGames.FindAsync(id);
            if (existingGame == null) return false;

            existingGame.Title = updatedVideoGame.Title;
            existingGame.Publisher = updatedVideoGame.Publisher;
            existingGame.Developer = updatedVideoGame.Developer;
            existingGame.Platform = updatedVideoGame.Platform;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game == null) return false;

            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
