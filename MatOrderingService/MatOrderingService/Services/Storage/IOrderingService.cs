using MatOrderingService.Domain;
using MatOrderingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Services.Storage
{
    public interface IOrderingService
    {
        Task<IEnumerable<OrderInfo>> GetAll();
        Task<OrderInfo> Get(int id);
        Task<OrderInfo> Create(NewOrder order);
        Task<OrderInfo> Update(int id, EditOrder value);
        Task<bool> Delete(int id);
        Task<IEnumerable<OrderStatisticItem>> GetStatistics();
        Task<IEnumerable<OrderStatisticItem>> GetStatisticsDapper();
    }
}
