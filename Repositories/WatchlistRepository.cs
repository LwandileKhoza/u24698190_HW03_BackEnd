using Microsoft.EntityFrameworkCore;
using FilmLog.API.Data;
using FilmLog.API.DTOs;
using FilmLog.API.Models;

namespace FilmLog.API.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WatchlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieResponseDto>> GetUserWatchlist(int userId)
        {
            try
            {
                return await _context.WatchlistItems
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
                        TimesWatched = 0
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading watchlist: {ex.Message}");
                return new List<MovieResponseDto>();
            }
        }

        public async Task<bool> AddToWatchlist(int userId, AddMovieDto movieDto)
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

                var alreadyAdded = await _context.WatchlistItems
                    .AnyAsync(w => w.UserId == userId && w.MovieId == movie.Id);

                if (alreadyAdded) return false;

                _context.WatchlistItems.Add(new WatchlistItem
                {
                    UserId = userId,
                    MovieId = movie.Id,
                    AddedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to watchlist: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveFromWatchlist(int userId, string omdbId)
        {
            try
            {
                var item = await _context.WatchlistItems
                    .Include(w => w.Movie)
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.Movie!.OmdbId == omdbId);

                if (item == null) return false;

                _context.WatchlistItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing from watchlist: {ex.Message}");
                return false;
            }
        }
    }
}