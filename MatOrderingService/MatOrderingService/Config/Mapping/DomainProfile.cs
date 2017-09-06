using AutoMapper;
using MatOrderingService.Domain;
using MatOrderingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Config.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Order, OrderInfo>()
                .ForMember(d => d.OrderStatus,
                opt => opt.MapFrom(s => s.OrderStatus.ToString()));

            CreateMap<NewOrder, Order>();

            CreateMap<EditOrder, Order>();

            CreateMap<NewOrderItem, OrderItem>();


            CreateMap<OrderItem, OrderItemInfo>()
                .ForMember(d => d.Name,
                opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Code,
                opt => opt.MapFrom(s => s.Product.Code));
        }
    }
}
