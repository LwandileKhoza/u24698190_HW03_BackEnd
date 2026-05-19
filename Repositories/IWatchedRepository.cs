using FilmLog.API.DTOs;

namespace FilmLog.API.Repositories
{
    public interface IWatchedRepository
    {
        Task<List<MovieResponseDto>> GetUserWatched(int userId);
        Task<bool> MarkAsWatched(int userId, AddMovieDto movieDto);
        Task<MovieResponseDto?> UpdateTimesWatched(int watchedItemId, int userId);
        Task<bool> RemoveFromWatched(int watchedItemId, int userId);
        Task<bool> ResetTimesWatched(int watchedItemId, int userId);
    }
}
