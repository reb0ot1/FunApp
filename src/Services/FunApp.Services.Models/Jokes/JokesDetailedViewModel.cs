using FunApp.Data.Models;
using FunApp.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunApp.Services.Models.Jokes
{
    public class JokesDetailedViewModel : IMapFrom<Joke>
    {
        public string Content { get; set; }

        public string CategoryName { get; set; }
    }
}
