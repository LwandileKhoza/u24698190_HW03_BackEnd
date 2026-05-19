using Microsoft.AspNetCore.Mvc;
using FilmLog.API.DTOs;
using FilmLog.API.Services;

namespace FilmLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] AuthRequestDto request)
        {
            var result = await _authService.Register(request);

            if (result == null)
                return BadRequest(new { message = "Email already registered or invalid input." });

            return CreatedAtAction(nameof(Register), result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthRequestDto request)
        {
            var result = await _authService.Login(request);

            if (result == null)
                return Unauthorized(new { message = "Incorrect email or password." });

            return Ok(result);
        }
    }
}
