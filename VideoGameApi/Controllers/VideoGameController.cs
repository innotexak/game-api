  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        static private List<VideoGame> videoGames = new List<VideoGame>
        {
            new VideoGame
            {
                Id = 1,
                Title = "Spider-Man 2",
                Platform= "PS5",
                Developer="Insomniac Games",
                Publisher="Sony Interative Entertainment"
            }
        };

        [HttpGet]
        public ActionResult<List<VideoGame>> GetVideoGames()
        {
            return Ok(videoGames);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<VideoGame> GetVideoGameById(int id)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public ActionResult<VideoGame> AddVideoGame(VideoGame newVideoGame)
            {
            if (newVideoGame is null)
                return BadRequest();

            newVideoGame.Id = videoGames.Max(g => g.Id) + 1;
            videoGames.Add(newVideoGame);
            return CreatedAtAction(nameof(GetVideoGameById), new { id= newVideoGame.Id }, newVideoGame);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateVideoGame(int id, VideoGame updatedVideoGame)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null)
                return NotFound();

            game.Title = updatedVideoGame.Title;
            game.Publisher = updatedVideoGame.Publisher;
            game.Developer = updatedVideoGame.Developer;
            game.Platform = updatedVideoGame.Platform;

            return NoContent();


        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideoGame(int id)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null)
                return NotFound();

            videoGames.Remove(game);
            return NoContent();

        }



    }
}  