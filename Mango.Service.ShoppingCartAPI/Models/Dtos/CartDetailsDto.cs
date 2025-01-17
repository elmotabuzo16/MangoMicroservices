﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.Dtos
{
    public class CartDetailsDto
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }

        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }
        public int Count { get; set; }

    }
}
