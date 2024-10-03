using Mango.Services.OrderAPI.Models.OrderDto;

namespace Mango.Services.OrderAPI.Models.Dtos
{
    public class StripeRequestDto
    {
        public string StripeSessionUrl { get; set; }
        public string StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderDto orderHeader { get; set; }

    }
}
