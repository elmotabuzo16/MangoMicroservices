﻿namespace Mango.Services.EmailAPI.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        // This is only for view, not in database
        public double Discount { get; set; }
        public double CartTotal { get; set; }

        // For message bus
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

    }
}