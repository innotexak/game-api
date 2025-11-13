using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Entities;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;

        public VideoGameController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            var games = await _videoGameService.GetAllAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var game = await _videoGameService.GetByIdAsync(id);
            if (game == null)
                return NotFound();

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGame>> AddVideoGame(VideoGame newVideoGame)
        {
            if (newVideoGame == null)
                return BadRequest();

            var createdGame = await _videoGameService.AddAsync(newVideoGame);
            return CreatedAtAction(nameof(GetVideoGameById), new { id = createdGame.Id }, createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id, VideoGame updatedVideoGame)
        {
            var success = await _videoGameService.UpdateAsync(id, updatedVideoGame);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame(int id)
        {
            var success = await _videoGameService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
