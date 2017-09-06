using System.Collections.Generic;
using System.Threading.Tasks;
using MatOrderingService.Domain;

namespace MatOrderingService.Services.Storage
{
    public interface IProductsService
    {
        Task<IEnumerable<string>> GetAll();
        Task<Product> GetById(int id);
    }
}