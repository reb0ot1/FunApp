using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FunApp.Data.Common;
using FunApp.Data.Models;
using FunApp.Services.Models;
using FunApp.Services.Models.Categories;
using FunApp.Services.Mapping;

namespace FunApp.Services.DataServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> repository;

        public CategoryService(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<CategoryIdAndNameViewModel> GetAll()
        {
            var categories = this.repository.All()
                .OrderBy(o => o.Name)
                .To<CategoryIdAndNameViewModel>();
                //.Select(e => new CategoryIdAndNameViewModel { Id = e.Id, Name = e.Name });

            return categories;
        }

        public Category GetCategoryByName(string categoryName)
        {
            return this.repository.All().FirstOrDefault(e => e.Name == categoryName)??null;
        }

        public bool IsCategoryIdValid(int categoryId)
        {
            return this.repository.All().Any(a => a.Id == categoryId);
        }
    }
}
