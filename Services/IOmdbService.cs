namespace FilmLog.API.Services
{
    public interface IOmdbService
    {
        Task<string> SearchByTitle(string title);
    }
}
