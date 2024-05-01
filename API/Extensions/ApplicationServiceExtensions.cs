using API.HandleReponses;
using Infrastructure.BasketRepository;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Services.BasketService;
using Services.BasketService.Dto;
using Services.services.CacheService;
using Services.services.OrderService;
using Services.services.OrderService.Dto;
using Services.services.PaymentService;
using Services.services.ProductService;
using Services.services.ProductService.Dto;
using Services.services.TokenService;
using Services.services.UserService;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // Add Scope Objects ( Inject Type )
           services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();    
            services.AddScoped<IPaymentService, PaymentService>();    
            services.AddScoped<IOrderService, OrderService>();    

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(model => model.Value.Errors.Count > 0)
                    .SelectMany(model => model.Value.Errors)
                    .Select(error => error.ErrorMessage).ToList();

                    var errorResponse = new APIValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //services.AddAutoMapper(x => x.AddProfile(new ProductProfile()));

            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            return services;
        }
    }
}
