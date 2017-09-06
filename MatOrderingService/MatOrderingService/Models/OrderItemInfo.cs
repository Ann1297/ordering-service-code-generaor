using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Models
{
    public class OrderItemInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        //public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
