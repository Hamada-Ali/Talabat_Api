using Core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.OrderService.Dto
{
    public class OrderItemDto
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public string ProductName { get; set; }
        public int ProductItemId { get; set; }
    }
}
