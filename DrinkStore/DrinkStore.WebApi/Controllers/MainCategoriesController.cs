using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;
using DrinkStore.Persistence.DTO;
using DrinkStore.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainCategoriesController : ControllerBase
    {
        private readonly DrinkStoreService _service;
        public MainCategoriesController(DrinkStoreService service)
        {
            _service = service;
        }

        // GET: api/MainCategories
        [HttpGet]
        public ActionResult<IEnumerable<MainCategoryDto>> GetMainCategories()
        {
            List<MainCategory> mainCategories;
            try
            {
                mainCategories = _service.GetMainCategories();
                return mainCategories.Select(mc => (MainCategoryDto)mc).ToList();
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }
        }

        // GET: api/MainCategories/1
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CategoryDto>> GetCategoriesByMainCategory(int id)
        {
            List<Category> categories;
            try
            {
                categories = _service.GetCategoriesByMainCategory(id);
                if (categories is null)
                    return NotFound();
                else
                    return categories.Select(c => (CategoryDto)c).ToList();
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }
        }

    }


    

}
