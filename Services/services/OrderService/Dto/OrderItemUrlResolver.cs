using AutoMapper;
using Core.Entities.OrderEntities;
using Microsoft.Extensions.Configuration;

namespace Services.services.OrderService.Dto
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {

            _configration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.ItemOrder.PictureUrl))
            {
                return _configration["BaseUrl"] + source.ItemOrder.PictureUrl;
            }

            return null;
        }
    }
}
