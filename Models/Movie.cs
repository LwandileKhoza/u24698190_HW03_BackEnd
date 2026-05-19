using System.ComponentModel.DataAnnotations;

namespace FilmLog.API.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OmdbId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Actors { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string ImdbRating { get; set; } = string.Empty;

        public List<WatchlistItem>? WatchlistItems { get; set; }
        public List<WatchedItem>? WatchedItems { get; set; }
    }
}