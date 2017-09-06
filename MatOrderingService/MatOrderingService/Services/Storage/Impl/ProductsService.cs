using MatOrderingService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Services.Storage.Impl
{
    public class ProductsService : IProductsService
    {
        private OrderDbContext _context;

        public ProductsService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            return _context.Products.Select(p => p.Name);
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }
    }
}
