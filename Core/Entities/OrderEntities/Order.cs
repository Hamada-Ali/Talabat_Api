namespace Core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed
    }
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(string buyerEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
            //GetTotal = getTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset orderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; }
        // public decimal GetTotal { get; set; }

        public decimal getTotal() => SubTotal + DeliveryMethod.Price;
    }
}
