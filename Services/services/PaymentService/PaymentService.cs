using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Microsoft.Extensions.Configuration;
using Services.BasketService;
using Services.BasketService.Dto;
using Services.services.OrderService.Dto;
using Stripe;
using Product = Core.Entities.Product;

namespace Services.services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IBasketService basketService, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketService.GetBasketAsync(basketId);

            if (basket is null)
            {
                return null;
            }

            var shippingPirce = 0m;

            if (basket.DeliveryMethods.HasValue)
            {
                var deliverymethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethods.Value);

                shippingPirce = deliverymethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }

            }

            var services = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.paymnetIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100) + ((long)shippingPirce * 100)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await services.CreateAsync(options);
                basket.paymnetIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100) + ((long)shippingPirce * 100))
                };

                await services.UpdateAsync(basket.paymnetIntentId, options);
            }

            await _basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new OrderWithItemsSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
            {
                return null;
            }

            order.orderStatus = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();


            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntentId, string basketId)
        {
            var specs = new OrderWithItemsSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
            {
                return null;
            }

            order.orderStatus = OrderStatus.PaymentReceived;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            await _basketService.DeleteBasketAsync(basketId);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

    }
}
