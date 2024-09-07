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
    [Authorize(Roles = "administrator")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DrinkStoreService _service;
        public OrdersController(DrinkStoreService service)
        {
            _service = service;
        }

        // GET: api/Orders/
        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            List<Order> orders;
            try
            {
                orders = _service.GetOrders();
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }

            return orders.Select(c => (OrderDto)c).ToList();
        }


        // GET: api/Orders/1
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<OrderLineDto>> GetOrderLinesByOrderId(int id)
        {
            List<OrderLine> orderLines;
            try
            {
                orderLines = _service.GetOrderLinesByOrderId(id);
                if(orderLines is null)
                        return NotFound();
                return orderLines.Select(c => (OrderLineDto)c).ToList();
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }  
        }

        // POST: api/Orders/
        [HttpPost]
        public ActionResult<IEnumerable<OrderDto>> GetFilteredOrders(FilterDto filter)
        {
            List<Order> orders;
            try
            {
                orders = _service.GetFilteredOrders(filter);
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }

            return orders.Select(c => (OrderDto)c).ToList();
        }

        [HttpPut("{id}")]
        public IActionResult PutOrder(Int32 id, OrderDto order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }
            if (_service.UpdateOrder(id, (Order)order))
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
