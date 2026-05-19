using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FilmLog.API.DTOs;
using FilmLog.API.Repositories;

namespace FilmLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WatchedController : ControllerBase
    {
        private readonly IWatchedRepository _repository;

        public WatchedController(IWatchedRepository repository)
        {
            _repository = repository;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieResponseDto>>> GetWatched()
        {
            var userId = GetUserId();
            var watched = await _repository.GetUserWatched(userId);

            if (watched == null || watched.Count == 0)
                return NotFound(new { message = "Watched list is empty." });

            return Ok(watched);
        }

        [HttpPost]
        public async Task<ActionResult> MarkAsWatched([FromBody] AddMovieDto movieDto)
        {
            var userId = GetUserId();
            var success = await _repository.MarkAsWatched(userId, movieDto);

            if (!success)
                return BadRequest(new { message = "Could not mark movie as watched." });

            return CreatedAtAction(nameof(GetWatched), new { message = "Marked as watched." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieResponseDto>> UpdateTimesWatched(int id)
        {
            var userId = GetUserId();
            var result = await _repository.UpdateTimesWatched(id, userId);

            if (result == null)
                return NotFound(new { message = "Watched item not found." });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveFromWatched(int id)
        {
            var userId = GetUserId();
            var success = await _repository.RemoveFromWatched(id, userId);

            if (!success)
                return NotFound(new { message = "Watched item not found." });

            return Ok(new { message = "Removed from watched list." });
        }

        [HttpPost("reset/{id}")]
        public async Task<ActionResult> ResetTimesWatched(int id)
        {
            var userId = GetUserId();
            var success = await _repository.ResetTimesWatched(id, userId);

            if (!success)
                return NotFound(new { message = "Watched item not found." });

            return Ok(new { message = "Counter reset. Movie moved back to Watchlist." });
        }
    }
}
