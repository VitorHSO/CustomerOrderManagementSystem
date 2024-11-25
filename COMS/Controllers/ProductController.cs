using COMS.Application.DTOs.Product;
using COMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDetailedDTO>>> GetAll()
        {

            var result = await _productService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetById/{productId}")]
        public async Task<ActionResult> GetById(int productId)
        {
            var result = await _productService.GetById(productId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }



        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@UserDto}", productDto);
                return BadRequest(ModelState);
            }

            var result = await _productService.Add(productDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("Update/{productId}")]
        public async Task<ActionResult> Update(int productId, ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.Update(productId, productDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Remove/{productId}")]
        public async Task<ActionResult> Remove(int productId)
        {
            var result = await _productService.Remove(productId);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }
    }
}
