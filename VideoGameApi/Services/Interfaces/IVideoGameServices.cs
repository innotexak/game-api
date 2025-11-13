using VideoGameApi.Data;
using VideoGameApi.Entities;

namespace VideoGameApi.Services.Interfaces
{
    public interface IVideoGameService
    {
        Task<List<VideoGame>> GetAllAsync();
        Task<VideoGame?> GetByIdAsync(int id);
        Task<VideoGame> AddAsync(VideoGame newVideoGame);
        Task<bool> UpdateAsync(int id, VideoGame updatedVideoGame);
        Task<bool> DeleteAsync(int id);
    }
}
