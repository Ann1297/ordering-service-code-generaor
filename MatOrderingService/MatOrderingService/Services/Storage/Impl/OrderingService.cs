using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatOrderingService.Domain;
using AutoMapper;
using MatOrderingService.Models;
using MatOrderingService.Exceptions;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace MatOrderingService.Services.Storage.Impl
{
    public class OrderingService : IOrderingService
    {
        private IMapper _mapper;
        private IOrderCodeGenerator _codeGenerator;
        private OrderDbContext _context;
        //private List<Order> _orderList;

        public OrderingService(IMapper mapper, IOrderCodeGenerator codeGenerator, OrderDbContext context)
        {
            _mapper = mapper;
            _codeGenerator = codeGenerator;
            _context = context;
        }

        public async Task<IEnumerable<OrderInfo>> GetAll()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(o => !o.IsDeleted)
                .ToArrayAsync();

            for (int i = 0; i < orders.Length; i++)
            {
                orders[i] = await GetOrderItemsForOrder(orders[i]);
            }
            
            return orders
                .Select(o => _mapper.Map<OrderInfo>(o))
                .ToArray();
        }

        public async Task<OrderInfo> Get(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            order = await GetOrderItemsForOrder(order);

            return _mapper.Map<OrderInfo>(order);
        }

        public async Task<OrderInfo> Create(NewOrder value)
        {
            if (value == null)
            {
                throw new EntityNotFoundException();
            }

            var order = _mapper.Map<Order>(value);
            order.OrderCode = new Guid().ToString();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            order.OrderCode = await _codeGenerator.Get(order.Id);
            await _context.SaveChangesAsync();

            order = await GetOrderItemsForOrder(order);

            return _mapper.Map<OrderInfo>(order);
        }

        public async Task<OrderInfo> Update(int id, EditOrder value)
        {
            var order = await _context.Orders
                .Include(i => i.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || value == null)
            {
                throw new EntityNotFoundException();
            }

            order.OrderItems.Clear();
            order.OrderItems = _mapper.Map<ICollection<OrderItem>>(value.OrderItems);
            await _context.SaveChangesAsync();

            order = await GetOrderItemsForOrder(order);
            
            return _mapper.Map<OrderInfo>(order);
        }

        private async Task<Order> GetOrderItemsForOrder(Order order)
        {
            order = await _context.Orders
                            .Include(i => i.OrderItems)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefaultAsync(i => i.Id == order.Id);
            return order;
        }

        public async Task<bool> Delete(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }
            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderStatisticItem>> GetStatistics()
        {
            var orderStatisticItems = await _context
                .Orders
                .AsNoTracking()
                .Where(o => !o.IsDeleted)
                .GroupBy(o => o.CreatorId)
                .Select(s => new OrderStatisticItem { CreatorId = s.Key, NumberOfOrders = s.Count() })
                .ToArrayAsync();

            return orderStatisticItems;
        }

        public async Task<IEnumerable<OrderStatisticItem>> GetStatisticsDapper()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var orderStatisticItems = await connection.QueryAsync<OrderStatisticItem>(@"
                    SELECT CreatorId, COUNT(*) As NumberOfOrders
                    FROM Orders
                    GROUP BY CreatorId
                ");

                return orderStatisticItems;
            }
        }
    }
}