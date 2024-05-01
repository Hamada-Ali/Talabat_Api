using AutoMapper;
using AutoMapper.Execution;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.ProductService.Dto
{
    public class ProductUrlResolver : IValueResolver<Product, ProductResultDto, string>
    {
        private readonly IConfiguration _configration;

        public ProductUrlResolver(IConfiguration configuration)
        {

            _configration = configuration;
        }

        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _configration["BaseUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
