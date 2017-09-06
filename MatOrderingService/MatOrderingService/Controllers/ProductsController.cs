using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MatOrderingService.Services.Storage;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MatOrderingService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("all")]
        [Route("/api/products")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _productService.GetAll());
        }

        [HttpGet]
        [Route("/api/products/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _productService.GetById(id));
        }
    }
}
