using Services.BasketService.Dto;
using Services.services.OrderService.Dto;


namespace Services.services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntentId, string basketId);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
