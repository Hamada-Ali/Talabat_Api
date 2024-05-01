using Core.Entities.OrderEntities;

namespace Infrastructure.Specification
{
    public class OrderWithItemsSpecifications : BaseSpecification<Order>
    {
        public OrderWithItemsSpecifications(string email) : base(order => order.BuyerEmail == email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.orderDate);
        }

        public OrderWithItemsSpecifications(int id, string email) : base(order => order.BuyerEmail == email && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
