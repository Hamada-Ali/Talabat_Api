using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specification
{
    public class ProductWithFiltersForCountSpecifications : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecifications(ProductSpecification specification)
            : base (x =>
               (string.IsNullOrEmpty(specification.Search) || x.Name.Trim().ToLower().Contains(specification.Search)) &&
             (!specification.BrandId.HasValue || x.ProductBrandId == specification.BrandId)
                && (!specification.TypeId.HasValue || x.ProductTypeId == specification.TypeId)
            )
        {

        }
    }
}
