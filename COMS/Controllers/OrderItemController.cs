using COMS.Application.DTOs.OrderItem;
using COMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        //private readonly IAuthenticateService _authenticateService;
        private readonly ILogger<OrderItemController> _logger;

        public OrderItemController(IOrderItemService orderItemService, /*IAuthenticateService authenticateService,*/ ILogger<OrderItemController> logger)
        {
            _orderItemService = orderItemService;
            //_authenticateService = authenticateService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrderItemDetailedDTO>>> GetAll()
        {

            var result = await _orderItemService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetById/{orderitemId}")]
        public async Task<ActionResult> GetById(int orderitemId)
        {
            var result = await _orderItemService.GetById(orderitemId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] OrderItemDTO orderitemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@UserDto}", orderitemDto);
                return BadRequest(ModelState);
            }

            var result = await _orderItemService.Add(orderitemDto);

            if (result.Success)
            {
                _logger.LogInformation("User created successfully: {@User}", result.Data);
                return Ok(result.Data);
                //return CreatedAtAction(nameof(Add), new { id = user.Id }, user);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Update/{orderitemId}")]
        public async Task<ActionResult> Update(int orderitemId, OrderItemDTO orderitemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@user}", orderitemDto);
                return BadRequest(ModelState);
            }

            var result = await _orderItemService.Update(orderitemId, orderitemDto);

            if (result.Success)
            {
                _logger.LogInformation("User updated successfully: {@User}", result.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("Remove/{orderitemId}")]
        public async Task<ActionResult> Remove(int orderitemId)
        {
            var result = await _orderItemService.Remove(orderitemId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
