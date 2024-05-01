using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderEntities
{

    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(decimal price, int quantity, ProductItemOrder itemOrder)
        {
            Price = price;
            Quantity = quantity;
            ItemOrder = itemOrder;
        }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrder ItemOrder { get; set; }
    }
}
