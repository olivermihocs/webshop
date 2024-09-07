using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrinkStore.Web.Models;
using DrinkStore.Persistence;
using DrinkStore.Persistence.Services;

namespace DrinkStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DrinkStoreService _service;

        public HomeController(ILogger<HomeController> logger,DrinkStoreService service)
        {
            _logger = logger;
            _service = service;
        }

        //Főoldal
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            List<List<Category>> list = new List<List<Category>>();
            List <MainCategory> mainCategories = _service.GetMainCategories();
            foreach(MainCategory mainCategory in mainCategories)
            {
                list.Add(_service.GetCategoriesByMainCategory(mainCategory.Id));
            }
            model.Categories = list;

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
