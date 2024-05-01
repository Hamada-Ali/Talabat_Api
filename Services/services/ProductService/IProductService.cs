using Core.Entities;
using Infrastructure.Specification;
using Services.Helper;
using Services.services.ProductService.Dto;

namespace Services.services.ProductService
{
    public interface IProductService
    {
        Task<ProductResultDto> GetProductByIdAsync(int? id);
        Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductsTypesAsync();
    }
}
