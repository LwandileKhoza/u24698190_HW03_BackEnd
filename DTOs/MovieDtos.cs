namespace FilmLog.API.DTOs
{
    public class MovieResponseDto
    {
        public int ItemId { get; set; }
        public string OmdbId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Actors { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string ImdbRating { get; set; } = string.Empty;
        public int TimesWatched { get; set; }
    }

    public class AddMovieDto
    {
        public string OmdbId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Actors { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string ImdbRating { get; set; } = string.Empty;
    }
}