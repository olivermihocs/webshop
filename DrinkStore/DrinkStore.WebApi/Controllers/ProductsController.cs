using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;
using DrinkStore.Persistence.DTO;
using DrinkStore.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DrinkStoreService _service;
        public ProductsController(DrinkStoreService service)
        {
            _service = service;
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            try
            {
                return (ProductDto)_service.GetProductById(id);
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult<ProductDto> PostProduct(ProductDto productDto)
        {
            var product = _service.CreateProduct((Product)productDto);
            if(product is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, (ProductDto)product);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult PutProduct(Int32 id, ProductDto product)
        {
            if (_service.UpdateProduct((Product)product))
            {
                return Ok();
            }
            else
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }


}
