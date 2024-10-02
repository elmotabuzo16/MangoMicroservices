using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    //[Authorize]
    public class ProductAPIController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private ResponseDto _responseDto;

        public ProductAPIController(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _responseDto = new ResponseDto();
        }

        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _appDbContext.Products.ToListAsync();
                var productDto = _mapper.Map<IEnumerable<ProductDto>>(products);
                _responseDto.Result = productDto;

                return Ok(_responseDto);
            }
            catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [Route("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == Id);
                var productDto = _mapper.Map<ProductDto>(product);
                _responseDto.Result = productDto;

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
        public async Task<IActionResult> Post([FromBody]ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _appDbContext.Products.AddAsync(product);
                await _appDbContext.SaveChangesAsync();

                _responseDto.Result = _mapper.Map<ProductDto>(product);

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
        public async Task<IActionResult> Put([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _appDbContext.Products.Update(product);
                await _appDbContext.SaveChangesAsync();

                _responseDto.Result = _mapper.Map<ProductDto>(product);

                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return BadRequest(_responseDto);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();

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
