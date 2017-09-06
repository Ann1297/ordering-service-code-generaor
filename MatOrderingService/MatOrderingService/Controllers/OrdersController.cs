using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatOrderingService.Domain;
using MatOrderingService.Services.Storage;
using System.Net;
using MatOrderingService.Models;
using Microsoft.AspNetCore.Authorization;
using MatOrderingService.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MatOrderingService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private IOrderingService _ordersService;

        public OrdersController(IOrderingService ordersService)
        {
            _ordersService = ordersService;
        }

        /// <summary>
        /// Get list of all orders
        /// </summary>
        /// <response code="200">Orders recieved</response>
        [HttpGet]
        [Route("all")]
        [Route("/api/orders")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _ordersService.GetAll());
        }

        /// <summary>
        /// Get an order by unique id
        /// </summary>
        /// <param name="id">Id of order to get</param>
        /// <response code="200">Order recieved</response>
        /// <response code="404">Order not found</response>
        [HttpGet]
        [Route("/api/orders/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _ordersService.Get(id));
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="order">Properties of created order</param>
        /// <response code="200">Order created</response>
        /// <response code="400">Order is not valid</response>
        [HttpPost]
        [Route("/api/orders")]
        public async Task<IActionResult> Post([FromBody]NewOrder order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _ordersService.Create(order));
        }

        /// <summary>
        /// Updates an Order's Detail by unique id 
        /// </summary>
        /// <param name="id">Id of order to update</param>
        /// <param name="value">New properties of the order</param>
        /// <response code="200">Order updated</response>
        /// <response code="400">Order is not valid</response>
        [HttpPut]
        [Route("/api/orders/{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]EditOrder value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ordersService.Update(id, value);

            return Ok(result);
        }

        /// <summary>
        /// Delete an order by unique id 
        /// </summary>
        /// <param name="id">Id of order to delete</param>
        /// <response code="200">Order deleted</response>
        /// <response code="400">Order is not deleted</response>
        [HttpDelete]
        [Route("/api/orders/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _ordersService.Delete(id);

            return Ok();
        }

        /// <summary>
        /// Get statistics about creators
        /// </summary>
        /// <response code="200">Statistics recieved</response>
        [HttpGet]
        [Route("/api/orders/statistics")]
        [ProducesResponseType(typeof(OrderStatisticItem[]), 200)]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(await _ordersService.GetStatistics());
        }

        /// <summary>
        /// Get statistics about creators using dapper
        /// </summary>
        /// <response code="200">Statistics recieved</response>
        [HttpGet]
        [Route("/api/orders/statisticsdapper")]
        [ProducesResponseType(typeof(OrderStatisticItem[]), 200)]
        public async Task<IActionResult> GetStatisticsDapper()
        {
            return Ok(await _ordersService.GetStatisticsDapper());
        }
    }
}