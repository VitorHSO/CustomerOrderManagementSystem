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
        //private readonly IAuthenticateService _authenticateService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, /*IAuthenticateService authenticateService,*/ ILogger<ProductController> logger)
        {
            _productService = productService;
            //_authenticateService = authenticateService;
            _logger = logger;
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

            if (result.Success)
            {
                _logger.LogInformation("User created successfully: {@User}", result.Data);
                return Ok(result.Data);
                //return CreatedAtAction(nameof(Add), new { id = user.Id }, user);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Update/{productId}")]
        public async Task<ActionResult> Update(int productId, ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@user}", productDto);
                return BadRequest(ModelState);
            }

            var result = await _productService.Update(productId, productDto);

            if (result.Success)
            {
                _logger.LogInformation("User updated successfully: {@User}", result.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("Remove/{productId}")]
        public async Task<ActionResult> Remove(int productId)
        {
            var result = await _productService.Remove(productId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
