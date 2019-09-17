﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FunApp.Web.Models;
using FunApp.Data.Common;
using FunApp.Data.Models;
using FunApp.Services.DataServices;
using FunApp.Services.Models.Home;
using Microsoft.AspNetCore.Identity;

namespace FunApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IJokeService jokeService;

        public HomeController(IJokeService js)
        {
            this.jokeService = js;
        }

        public IActionResult Index()
        {
            var jokes = this.jokeService.GetRandomJokes(20);

            IndexViewModel viewModel = new IndexViewModel()
            {
                Jokes = jokes
            };

            return View(viewModel);
        }

        public IActionResult About()
        {
            var jokesCount = this.jokeService.GetCount();
            ViewData["Message"] = $"My application has {jokesCount} jokes";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
