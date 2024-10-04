using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Dtos;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dtos;
using Mango.Services.OrderAPI.Models.OrderDto;
using Mango.Services.OrderAPI.Models.RewardsDto;
using Mango.Services.OrderAPI.Utility;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ResponseDto _responseDto;

        public OrderAPIController(AppDbContext dbContext, IMapper mapper, IProductService productService, IMessageBus messageBus, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
            _responseDto = new ResponseDto();
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody]CartDto cartDto)
        {
            try
            {
                var orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = StaticDetails.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);


                var orderEntry = await _dbContext.OrderHeaders.AddAsync(_mapper.Map<OrderHeader>(orderHeaderDto));
                var orderCreated = orderEntry.Entity;
                var id = await _dbContext.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDto.Result = orderHeaderDto;

                RewardsDto rewardsDto = new()
                {
                    OrderId = orderCreated.OrderHeaderId,
                    RewardsActivity = Convert.ToInt32(orderCreated.OrderTotal),
                    UserId = orderCreated.UserId
                };

                string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                await _messageBus.PublishMessage(rewardsDto, topicName);

                return Ok(_responseDto);

            }
            catch (Exception ex) 
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<IActionResult> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = "https://example.com/success",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                            Quantity = 2,
                        },
                    },

                    Mode = "payment",
                };

                var service = new Stripe.Checkout.SessionService();
                service.Create(options);

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }
    }
}
