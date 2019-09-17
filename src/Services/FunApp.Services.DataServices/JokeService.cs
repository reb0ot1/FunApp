using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunApp.Data.Common;
using FunApp.Data.Models;
using FunApp.Services.Mapping;
using FunApp.Services.Models.Home;
using FunApp.Services.Models.Jokes;

namespace FunApp.Services.DataServices
{
    public class JokeService : IJokeService
    {
        private readonly IRepository<Joke> jokesRepository;
        private readonly IRepository<Category> categoriesRepository;

        public JokeService(IRepository<Joke> jokeRepo, IRepository<Category> cr)
        {
            this.jokesRepository = jokeRepo;
            this.categoriesRepository = cr;
        }

        public int GetCount()
        {
            return this.jokesRepository.All().Count();
        }

        public IEnumerable<JokeIndexViewModel> GetRandomJokes(int count)
        {
            return this.jokesRepository
                .All()
                .OrderBy(o => Guid.NewGuid())
                .Take(count)
                .To<JokeIndexViewModel>()
                .ToList();
        }
        
        public async Task<int> Create(int categoryId, string content)
        {
            var joke = new Joke { CategoryId = categoryId, Content = content };

            await this.jokesRepository.AddAsync(joke);
            await this.jokesRepository.SaveChangesAsync();

            return joke.Id;
        }

        public JokesDetailedViewModel GetJokeById(int id)
        {
            var joke = this.jokesRepository.All().Where(e => e.Id == id).To<JokesDetailedViewModel>().FirstOrDefault();

            return joke;
        }
    }
}
