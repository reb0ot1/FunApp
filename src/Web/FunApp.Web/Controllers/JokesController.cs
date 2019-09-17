using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunApp.Services.DataServices;
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

        public JokesController(IJokeService js, ICategoryService cs)
        {
            this.jokeService = js;
            this.categoryService = cs;
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
    }
}