using Microsoft.EntityFrameworkCore;
using FilmLog.API.Data;
using FilmLog.API.DTOs;
using FilmLog.API.Models;

namespace FilmLog.API.Repositories
{
    public class WatchedRepository : IWatchedRepository
    {
        private readonly ApplicationDbContext _context;

        public WatchedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieResponseDto>> GetUserWatched(int userId)
        {
            try
            {
                return await _context.WatchedItems
                    .Where(w => w.UserId == userId)
                    .Include(w => w.Movie)
                    .Select(w => new MovieResponseDto
                    {
                        ItemId = w.Id,
                        OmdbId = w.Movie!.OmdbId,
                        Title = w.Movie.Title,
                        Year = w.Movie.Year,
                        Poster = w.Movie.Poster,
                        Actors = w.Movie.Actors,
                        Genre = w.Movie.Genre,
                        ImdbRating = w.Movie.ImdbRating,
                        TimesWatched = w.TimesWatched
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading watched list: {ex.Message}");
                return new List<MovieResponseDto>();
            }
        }

        public async Task<bool> MarkAsWatched(int userId, AddMovieDto movieDto)
        {
            try
            {
                var movie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.OmdbId == movieDto.OmdbId);

                if (movie == null)
                {
                    movie = new Movie
                    {
                        OmdbId = movieDto.OmdbId,
                        Title = movieDto.Title,
                        Year = movieDto.Year,
                        Poster = movieDto.Poster,
                        Actors = movieDto.Actors,
                        Genre = movieDto.Genre,
                        ImdbRating = movieDto.ImdbRating
                    };
                    _context.Movies.Add(movie);
                    await _context.SaveChangesAsync();
                }

                var watchlistItem = await _context.WatchlistItems
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movie.Id);

                if (watchlistItem != null)
                    _context.WatchlistItems.Remove(watchlistItem);

                var existingWatched = await _context.WatchedItems
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movie.Id);

                if (existingWatched != null)
                {
                    existingWatched.TimesWatched += 1;
                    existingWatched.LastWatchedAt = DateTime.UtcNow;
                }
                else
                {
                    _context.WatchedItems.Add(new WatchedItem
                    {
                        UserId = userId,
                        MovieId = movie.Id,
                        TimesWatched = 1,
                        LastWatchedAt = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking as watched: {ex.Message}");
                return false;
            }
        }

        public async Task<MovieResponseDto?> UpdateTimesWatched(int watchedItemId, int userId)
        {
            try
            {
                var item = await _context.WatchedItems
                    .Include(w => w.Movie)
                    .FirstOrDefaultAsync(w => w.Id == watchedItemId && w.UserId == userId);

                if (item == null) return null;

                item.TimesWatched += 1;
                item.LastWatchedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new MovieResponseDto
                {
                    ItemId = item.Id,
                    OmdbId = item.Movie!.OmdbId,
                    Title = item.Movie.Title,
                    Year = item.Movie.Year,
                    Poster = item.Movie.Poster,
                    Actors = item.Movie.Actors,
                    Genre = item.Movie.Genre,
                    ImdbRating = item.Movie.ImdbRating,
                    TimesWatched = item.TimesWatched
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating times watched: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RemoveFromWatched(int watchedItemId, int userId)
        {
            try
            {
                var item = await _context.WatchedItems
                    .FirstOrDefaultAsync(w => w.Id == watchedItemId && w.UserId == userId);

                if (item == null) return false;

                _context.WatchedItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing from watched: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ResetTimesWatched(int watchedItemId, int userId)
        {
            try
            {
                var item = await _context.WatchedItems
                    .Include(w => w.Movie)
                    .FirstOrDefaultAsync(w => w.Id == watchedItemId && w.UserId == userId);

                if (item == null) return false;

                var alreadyInWatchlist = await _context.WatchlistItems
                    .AnyAsync(w => w.UserId == userId && w.MovieId == item.MovieId);

                if (!alreadyInWatchlist)
                {
                    _context.WatchlistItems.Add(new WatchlistItem
                    {
                        UserId = userId,
                        MovieId = item.MovieId,
                        AddedAt = DateTime.UtcNow
                    });
                }

                _context.WatchedItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting times watched: {ex.Message}");
                return false;
            }
        }
    }
}
