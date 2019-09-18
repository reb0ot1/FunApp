using AutoMapper;
using FunApp.Data.Models;
using FunApp.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunApp.Services.Models.Home
{
    public class JokeIndexViewModel : IMapFrom<Joke>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string HtmlContent => this.Content.Replace("\n", "<br/>\n");

        public string CategoryName { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<Joke, JokeIndexViewModel>()
            //    .ForMember(x => x.CategoryName, x => x.MapFrom(j => j.Category.Name));
        }
    }
}
