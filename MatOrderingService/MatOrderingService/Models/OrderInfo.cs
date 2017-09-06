using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Models
{
    public class OrderInfo
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }

        public ICollection<OrderItemInfo> OrderItems { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatorId { get; set; }
    }
}
