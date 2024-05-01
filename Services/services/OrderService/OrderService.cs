using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Services.BasketService;
using Services.services.OrderService.Dto;
using Services.services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto)
        {
            // Get basket
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);

            if (basket is null)
                return null;

            var orderItems = new List<OrderItemDto>();

            foreach(var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var itemOrdered = new ProductItemOrder(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderitem = new OrderItem(productItem.Price, item.Quantity, itemOrdered);

                var mappedOrderedItem = _mapper.Map<OrderItemDto>(orderitem);
                orderItems.Add(mappedOrderedItem);
            }

            // Get Delivery Method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            // calculate subtotal
            var subtotal = orderItems.Sum(item =>  item.Price * item.Quantity);


            // todo

            var specs = new OrderWithPaymentIntentSpecification(basket.paymnetIntentId);

            var ExistingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if(ExistingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(ExistingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.paymnetIntentId);
            }

            // create order
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order(orderDto.BuyerEmail, mappedShippingAddress, deliveryMethod, mappedOrderItems, subtotal, basket.paymnetIntentId);

            await _unitOfWork.Repository<Order>().Add(order);

            await _unitOfWork.Complete();

            // delete basket
            await _basketService.DeleteBasketAsync(orderDto.BasketId);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications(buyerEmail);

            var orders =  await _unitOfWork.Repository<Order>().GetAllWithSpecificationsAsync(specs);

            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications( id, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            var mappedOrders =  _mapper.Map<OrderResultDto>(order);

            return mappedOrders;
        }
    }
}
