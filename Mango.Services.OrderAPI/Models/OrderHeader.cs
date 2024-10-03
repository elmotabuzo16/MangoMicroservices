using System.ComponentModel.DataAnnotations;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        // This is only for view, not in database
        public double Discount { get; set; }
        public double OrderTotal { get; set; }

        // For message bus
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }

        public DateTime OrderTime { get; set; }
        public string? OrderStatus { get; set; }

        // Gateway for payments
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }

    }
}
