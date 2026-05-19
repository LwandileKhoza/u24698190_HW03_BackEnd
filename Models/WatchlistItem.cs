using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmLog.API.Models
{
    public class WatchlistItem
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;    
    }
}