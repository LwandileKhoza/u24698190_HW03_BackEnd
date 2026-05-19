using FilmLog.API.DTOs;

namespace FilmLog.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> Register(AuthRequestDto request);
        Task<AuthResponseDto?> Login(AuthRequestDto request);
    }
}

