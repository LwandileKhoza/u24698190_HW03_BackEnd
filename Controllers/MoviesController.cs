using Microsoft.AspNetCore.Mvc;
using FilmLog.API.Services;
using System.Text.Json;

namespace FilmLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IOmdbService _omdbService;

        public MoviesController(IOmdbService omdbService)
        {
            _omdbService = omdbService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string t)
        {
            if (string.IsNullOrWhiteSpace(t))
                return BadRequest(new { message = "Search query is required." });

            var result = await _omdbService.SearchByTitle(t);

            if (string.IsNullOrEmpty(result))
                return NotFound(new { message = "No results found." });

            var jsonObject = JsonSerializer.Deserialize<object>(result);
            return Ok(jsonObject);
        }
    }
}