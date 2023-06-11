using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using _6_5ApiHomework.Data;
using System.Runtime.CompilerServices;
using _6_5ApiHomework.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace _6_5ApiHomework.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly string _connectionString;

        public JokesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("getjoke")]
        [HttpGet]
        public Joke GetJoke()
        {
            //var client = new HttpClient();
            //var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            //Joke joke = JsonSerializer.Deserialize<List<Joke>>(json, new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //}).FirstOrDefault();


            Joke joke = new Joke
            {
                Question = "['hip', 'hip']",
                Answer = "(hip hip array)",
                OriginalId = 27
            };
            var repo = new JokesRepository(_connectionString);
            repo.AddJoke(joke);
            if (!repo.JokeExists(joke.OriginalId))
            {
                repo.AddJoke(joke);
                return joke;
            }
            int id = repo.GetJokeId(joke.OriginalId);
            return repo.GetJokeById(id);

        }



        [Route("viewall")]
        [HttpGet]
        public List<Joke> GetJokes()
        {
            var jokeRepo = new JokesRepository(_connectionString);
            return jokeRepo.GetAll();
        }


        [Route("getlikes")]
        [HttpGet]
        public int GetLikes(int id)
        {
            var repo = new JokesRepository(_connectionString);
            var withLikes = repo.GetWithLikes(id);
            return withLikes.UserLikedJokes.Count(wl => wl.Liked == true);
        }

        [Route("getdislikes")]
        [HttpGet]
        public int GetDislikes(int id)
        {
            var repo = new JokesRepository(_connectionString);
            var withLikes = repo.GetWithLikes(id);
            return withLikes.UserLikedJokes.Count(wl => wl.Liked == false);
        }

        [HttpGet]
        [Route("getinteractionstatus/{jokeid}")]
        public object GetInteractionStatus(int jokeId)
        {
            InteractionStatus status = GetStatus(jokeId);
            return new { status };
        }

        private InteractionStatus GetStatus(int jokeId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return InteractionStatus.Unauthenticated;
            }
            var repo = new UserRepository(_connectionString);
            var user = repo.GetByEmail(User.Identity.Name);
            var jokesRepo = new JokesRepository(_connectionString);
            UserLikedJokes likeStatus = jokesRepo.GetLike(user.Id, jokeId);
            if (likeStatus == null)
            {
                return InteractionStatus.NeverInteracted;
            }

            return likeStatus.Liked
                ? InteractionStatus.Liked
                : InteractionStatus.Disliked;
        }

        [HttpPost]
        [Authorize]
        [Route("interactwithjoke")]
        public void InteractWithJoke(InteractionViewModel ivm)
        {
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetByEmail(User.Identity.Name);
            var repo = new JokesRepository(_connectionString);
            repo.InteractWithJoke(user.Id, ivm.jokeId, ivm.Like);
        }

    }
}
