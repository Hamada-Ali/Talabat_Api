using Core.Entities.OrderEntities;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.OrderService.Dto
{
    public class OrderResultDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset orderDate { get; set; } 
        public AddressDto shippingAddress { get; set; }
        public string deliveryMethod { get; set; }
        public OrderStatus orderStatus { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
    }
}
