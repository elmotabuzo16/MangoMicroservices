using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    //[Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public CouponAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _dbContext.Coupons.ToListAsync();
                var couponsDto = _mapper.Map<IEnumerable<CouponDto>>(result);
                _responseDto.Result = couponsDto;

                return Ok(_responseDto);

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await _dbContext.Coupons.FirstAsync(x => x.CouponId == Id);
                var couponDto = _mapper.Map<CouponDto>(result);
                _responseDto.Result = couponDto;

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [HttpGet("GetByCode/{couponCode}")]
        public async Task<IActionResult> GetByCode(string couponCode)
        {
            try
            {
                var result = await _dbContext.Coupons.FirstAsync(x => x.CouponCode.ToLower() == couponCode.ToLower());
                var couponDto = _mapper.Map<CouponDto>(result);
                _responseDto.Result = couponDto;

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _dbContext.Coupons.Add(coupon);
                await _dbContext.SaveChangesAsync();

                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _dbContext.Coupons.Update(coupon);
                await _dbContext.SaveChangesAsync();

                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var coupon = _dbContext.Coupons.First(x => x.CouponId == Id);
                _dbContext.Coupons.Remove(coupon);
                await _dbContext.SaveChangesAsync();

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
