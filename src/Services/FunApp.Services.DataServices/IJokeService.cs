using FunApp.Data.Models;
using FunApp.Services.Models.Home;
using FunApp.Services.Models.Jokes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunApp.Services.DataServices
{
    public interface IJokeService
    {
        IEnumerable<JokeIndexViewModel> GetRandomJokes(int count);

        int GetCount();

        Task<int> Create(int categoryId, string content);

        JokesDetailedViewModel GetJokeById(int id);
    }
}
