using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrinkStore.Persistence;
using DrinkStore.Persistence.Services;
using DrinkStore.Persistence.DTO;

namespace DrinkStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DrinkStoreService _service;
        public CategoriesController(DrinkStoreService service)
        {
            _service = service;
        }

        // GET: api/Categories/1
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<ProductDto>> GetCategoryById(int id)
        {
            try
            {
                return _service.GetCategoryById(id).Select(c => (ProductDto)c).ToList();
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
