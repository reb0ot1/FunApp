using FunApp.Data.Models;
using FunApp.Services.Models.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunApp.Services.DataServices
{
    public interface ICategoryService
    {
        IEnumerable<CategoryIdAndNameViewModel> GetAll();

        bool IsCategoryIdValid(int categoryId);

        Category GetCategoryByName(string categoryName);
    }
}
