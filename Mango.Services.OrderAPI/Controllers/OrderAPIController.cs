using AutoMapper;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Dtos;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.OrderDto;
using Mango.Services.OrderAPI.Utility;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private ResponseDto _responseDto;

        public OrderAPIController(AppDbContext dbContext, IMapper mapper, IProductService productService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
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
                await _dbContext.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDto.Result = orderHeaderDto;

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
