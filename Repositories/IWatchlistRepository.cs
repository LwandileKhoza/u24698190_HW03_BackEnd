using FilmLog.API.DTOs;

namespace FilmLog.API.Repositories
{
    public interface IWatchlistRepository
    {
        Task<List<MovieResponseDto>> GetUserWatchlist(int userId);
        Task<bool> AddToWatchlist(int userId, AddMovieDto movieDto);
        Task<bool> RemoveFromWatchlist(int userId, string omdbId);
    }
}
