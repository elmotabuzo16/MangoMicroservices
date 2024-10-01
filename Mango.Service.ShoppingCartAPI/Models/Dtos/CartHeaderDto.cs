using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Service.ShoppingCartAPI.Models.Dtos
{
    public class CartHeaderDto
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        // This is only for view, not in database
        public double Discount { get; set; }
        public double CartTotal { get; set; }

    }
}
