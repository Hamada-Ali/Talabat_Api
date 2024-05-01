using API.HandleReponses;
using API.Helpers;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.services;
using Services.services.ProductService;
using Services.services.ProductService.Dto;

namespace API.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) {

            _productService = productService;
        }

        [HttpGet("AllProducts")]
        public async Task<ActionResult<Pagination<ProductResultDto>>> GetProducts([FromQuery]ProductSpecification specs)
        {
            var products = await _productService.GetProductsAsync(specs);

            return Ok(products);
        }

        [HttpGet("AllProductsBrands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrand = await _productService.GetProductsBrandsAsync();

            return Ok(productBrand);
        }

        [HttpGet("AllProductsTypes")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypes()
        {
            var productType = await _productService.GetProductsTypesAsync();

            return Ok(productType);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [Cache(10)]
        // cache attribute
        public async Task<ActionResult<ProductResultDto>> GetProductById(int? id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if(product is null)
            {
                return NotFound(new APIResponse(404));
            }

            return Ok(product);
        }
    }
}
