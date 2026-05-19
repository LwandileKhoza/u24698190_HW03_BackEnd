using System.ComponentModel.DataAnnotations;

namespace FilmLog.API.Models
{
    public class WatchedItem
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int TimesWatched { get; set; } = 1;              
        public DateTime LastWatchedAt { get; set; } = DateTime.UtcNow;
    }
}