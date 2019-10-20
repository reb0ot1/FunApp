using FunApp.Data;
using FunApp.Data.Common;
using FunApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace FunApp.Services.DataServices.Tests
{
    public class JokesServiceTests
    {
        [Fact]
        public void GetCountsShouldReturnCorrectNumner()
        {
            var jokesRepository = new Mock<IRepository<Joke>>();
            jokesRepository.Setup(r => r.All()).Returns(new List<Joke> {
                new Joke(),
                new Joke(),
                new Joke(),
                new Joke()
            }.AsQueryable());

            var service = new JokeService(jokesRepository.Object, null);
            Assert.Equal(4, service.GetCount());
        }

        [Fact]
        public async Task GetCountShoulReturnCorrectNumberUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<FunAppContext>()
                .UseInMemoryDatabase("Find_User_Database").Options;

            var dbContext = new FunAppContext(options);


            dbContext.Jokes.Add(new Joke());
            dbContext.Jokes.Add(new Joke());
            dbContext.Jokes.Add(new Joke());

            await dbContext.SaveChangesAsync();

            var repository = new DbRepository<Joke>(dbContext);

            var jokeService = new JokeService(repository, null);
            var count = jokeService.GetCount();

            Assert.Equal(3, count);

        }
    }

    //public class JokesRepository : IRepository<Joke>
    //{
    //    public Task AddAsync(Joke entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IQueryable<Joke> All()
    //    {
    //        return new List<Joke>() {
    //            new Joke(),
    //            new Joke(),
    //            new Joke()
    //        }.AsQueryable();
    //    }

    //    public void Delete(Joke entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<int> SaveChangesAsync()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
