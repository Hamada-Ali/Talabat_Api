﻿using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Specification;
using Services.Helper;
using Services.services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ProductResultDto> GetProductByIdAsync(int? id)
        {
            var specs = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationsAsync(specs);

  
            
            var mappedProduct = _mapper.Map<ProductResultDto>(product);

            return mappedProduct;
        }

        public async Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification)
        {
            var specs = new ProductsWithTypesAndBrandsSpecification(specification);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(specs);

            var countSpecs = new ProductWithFiltersForCountSpecifications(specification);

            return new Pagination<ProductResultDto>(specification.PageIndex, specification.PageSize, totalItems,  mappedProducts);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync()

            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public async Task<IReadOnlyList<ProductType>> GetProductsTypesAsync()
         => await _unitOfWork.Repository<ProductType>().GetAllAsync();
    }
}
