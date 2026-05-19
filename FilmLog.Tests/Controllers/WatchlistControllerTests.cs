using Moq;
using Microsoft.AspNetCore.Mvc;
using FilmLog.API.Controllers;
using FilmLog.API.DTOs;
using FilmLog.API.Repositories;

namespace FilmLog.Tests.Controllers
{
    public class WatchlistControllerTests
    {
        [Fact]
        public async Task GetWatchlist_ReturnsOk_WhenWatchlistHasMovies()
        {
            var mockRepo = new Mock<IWatchlistRepository>();

            mockRepo.Setup(repo => repo.GetUserWatchlist(It.IsAny<int>()))
                .ReturnsAsync(new List<MovieResponseDto>
                {
                    new MovieResponseDto
                    {
                        ItemId = 1,
                        OmdbId = "tt1375666",
                        Title = "Inception",
                        Year = "2010",
                        Genre = "Action, Sci-Fi",
                        TimesWatched = 0
                    }
                });

            var controller = new WatchlistController(mockRepo.Object);

            var claims = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.NameIdentifier, "1")
                }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = claims
                }
            };

            var result = await controller.GetWatchlist();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetWatchlist_ReturnsNotFound_WhenWatchlistIsEmpty()
        {
            var mockRepo = new Mock<IWatchlistRepository>();

            mockRepo.Setup(repo => repo.GetUserWatchlist(It.IsAny<int>()))
                .ReturnsAsync(new List<MovieResponseDto>());

            var controller = new WatchlistController(mockRepo.Object);

            var claims = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.NameIdentifier, "1")
                }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = claims
                }
            };

            var result = await controller.GetWatchlist();

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
