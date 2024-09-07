using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Web.Models;
using DrinkStore.Persistence.Services;
using DrinkStore.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DrinkStoreService _service;

        public CategoryController(DrinkStoreService service)
        {
            _service = service;
        }


        //Kategória megjelenítése
        public IActionResult Index(int Id,int page=1,bool orderByManufacturer=false, bool orderByPrice=false, bool reverse=false)
        {

            if (1>page)
            {
                return RedirectToAction("Index", "Home");
            }

            //Kategóriához tartozó termékek rendezve
            Tuple<string, IEnumerable<Product>> category = _service.GetCategoryById(Id, page,orderByManufacturer, orderByPrice,reverse);

            if (category==null || category.Item2==null|| category.Item2.Count()==0)
            {
                return RedirectToAction("Index", "Home");
            }

            CategoryViewModel model = new CategoryViewModel
            {
                OrderByManufacturer = orderByManufacturer,
                OrderByPrice = orderByPrice,
                Reverse = reverse,

                CurrentPage = page,
                PrevPageAvailable = page > 1,
                NextPageAvailable = category.Item2.Count() == 21,
                Products = category.Item2.Take(20),
                CategoryName = category.Item1,
                VAT = _service.GetVAT()
            };

            return View(model);
        }
    }
}
