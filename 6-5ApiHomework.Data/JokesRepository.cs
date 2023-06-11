using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6_5ApiHomework.Data
{
    public class JokesRepository
    {
        public string _connectionString;

        public JokesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddJoke(Joke joke)
        {
            var context = new JokesDbContext(_connectionString);
            Joke j = context.Jokes.FirstOrDefault(j => j.OriginalId == joke.OriginalId);
            if (j == null)
            {
                context.Jokes.Add(joke);
                context.SaveChanges();
            }
        }

        public bool JokeExists(int originId)
        {
            var context = new JokesDbContext(_connectionString);
            return context.Jokes.Any(j => j.OriginalId == originId);
        }

        public Joke GetWithLikes(int jokeId)
        {
            using var context = new JokesDbContext(_connectionString);
            return context.Jokes.Include(u => u.UserLikedJokes)
                .FirstOrDefault(j => j.Id == jokeId);
        }


        public List<Joke> GetAll()
        {
            using var context = new JokesDbContext(_connectionString);
            return context.Jokes.Include(j => j.UserLikedJokes).ToList();
        }

        public int GetJokeId(int originalId)
        {
            var context = new JokesDbContext(_connectionString);
            return context.Jokes.FirstOrDefault(j => j.OriginalId == originalId).Id;
        }

        public Joke GetJokeById (int id)
        {
            var context = new JokesDbContext(_connectionString);
            return context.Jokes.FirstOrDefault(j => j.Id == id);
        }

        public void InteractWithJoke(int userId, int jokeId, bool like)
        {
            using var context = new JokesDbContext(_connectionString);
            var joke = context.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
            if (joke == null)
            {
                context.UserLikedJokes.Add(new UserLikedJokes
                {
                    UserId = userId,
                    JokeId = jokeId,
                    Liked = like,
                });
            }
            else
            {
                joke.Liked = like;
            }

            context.SaveChanges();
        }

        public UserLikedJokes GetLike(int userId, int jokeId)
        {
            using var context = new JokesDbContext(_connectionString);
            return context.UserLikedJokes.FirstOrDefault(u => u.UserId == userId && u.JokeId == jokeId);
        }


    }
}
