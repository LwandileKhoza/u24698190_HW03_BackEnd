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
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistRepository _repository;

        public WatchlistController(IWatchlistRepository repository)
        {
            _repository = repository;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieResponseDto>>> GetWatchlist()
        {
            var userId = GetUserId();
            var watchlist = await _repository.GetUserWatchlist(userId);

            if (watchlist == null || watchlist.Count == 0)
                return NotFound(new { message = "Watchlist is empty." });

            return Ok(watchlist);
        }

        [HttpPost]
        public async Task<ActionResult> AddToWatchlist([FromBody] AddMovieDto movieDto)
        {
            var userId = GetUserId();
            var success = await _repository.AddToWatchlist(userId, movieDto);

            if (!success)
                return BadRequest(new { message = "Movie already in watchlist or could not be added." });

            return CreatedAtAction(nameof(GetWatchlist), new { message = "Added to watchlist." });
        }

        [HttpDelete("{omdbId}")]
        public async Task<ActionResult> RemoveFromWatchlist(string omdbId)
        {
            var userId = GetUserId();
            var success = await _repository.RemoveFromWatchlist(userId, omdbId);

            if (!success)
                return NotFound(new { message = "Movie not found in watchlist." });

            return Ok(new { message = "Removed from watchlist." });
        }
    }
}