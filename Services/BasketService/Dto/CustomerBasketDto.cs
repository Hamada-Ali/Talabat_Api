﻿using Infrastructure.BasketRepository.BasketEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BasketService.Dto
{
    public class CustomerBasketDto
    {

        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public int? DeliveryMethods { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? paymnetIntentId { get;  set; }
        public string? ClientSecret { get; set; }
    }
}
