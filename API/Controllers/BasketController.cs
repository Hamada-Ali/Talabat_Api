using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.BasketService;
using Services.BasketService.Dto;

namespace API.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
        {
            return Ok(await _basketService.GetBasketAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basket)
        {
            return Ok(await _basketService.UpdateBasketAsync(basket));
        }

        [HttpDelete]
        public async Task DeleteBasketById(string Id)
        
             => Ok(await _basketService.DeleteBasketAsync(Id));
        
    }
}
