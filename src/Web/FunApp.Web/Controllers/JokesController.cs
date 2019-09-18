using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunApp.Services.DataServices;
using FunApp.Services.MachineLearning;
using FunApp.Web.Models.Category;
using FunApp.Web.Models.Jokes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FunApp.Web.Controllers
{
    
    public class JokesController : BaseController
    {
        private readonly IJokeService jokeService;
        private readonly ICategoryService categoryService;
        private readonly IJokesCategorizer jokeCategorizerService;

        public JokesController(IJokeService js, ICategoryService cs, IJokesCategorizer jc)
        {
            this.jokeService = js;
            this.categoryService = cs;
            this.jokeCategorizerService = jc;
        }

        [Authorize]
        public IActionResult Create()
        {
            this.ViewData["Categories"] = this.categoryService.GetAll()
                .Select(s => new SelectListItem {
                    Value = s.Id.ToString(),
                    Text = s.NameAndCount });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJokeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var id = await this.jokeService.Create(input.CategoryId, input.Content);

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var joke = this.jokeService.GetJokeById(id);

            return this.View(joke);
        }

        public IActionResult View(int id)
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public SpecializedCategory SuggestCategory(string joke)
        {
            var categorizedCategory = this.jokeCategorizerService.Categorize("MlModels/JokesCategoryModel.zip", joke);
            var category = this.categoryService.GetCategoryByName(categorizedCategory);

            var result = new SpecializedCategory {
                Id = category?.Id??0,
                Name = categorizedCategory,
            };

            return result;
        }
    }
}