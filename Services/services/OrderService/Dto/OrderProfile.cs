using AutoMapper;
using Core.Entities.OrderEntities;
using Core.IdentityEntities;

namespace Services.services.OrderService.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, ShippingAddress>();
            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.deliveryMethod, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.shippingAddress, option => option.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId, option => option.MapFrom(src => src.ItemOrder.ProductItemId))
                .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ItemOrder.ProductName))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom(src => src.ItemOrder.PictureUrl))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<OrderItemUrlResolver>());

        }
    }
}
