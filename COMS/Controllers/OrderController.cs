using COMS.Application.DTOs.Order;
using COMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        //private readonly IAuthenticateService _authenticateService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, /*IAuthenticateService authenticateService,*/ ILogger<OrderController> logger)
        {
            _orderService = orderService;
            //_authenticateService = authenticateService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrderDetailedDTO>>> GetAll()
        {

            var result = await _orderService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetById/{orderId}")]
        public async Task<ActionResult> GetById(int orderId)
        {
            var result = await _orderService.GetById(orderId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }



        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@OrderDto}", orderDto);
                return BadRequest(ModelState);
            }

            var result = await _orderService.Add(orderDto);

            if (result.Success)
            {
                _logger.LogInformation("User created successfully: {@Order}", result.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Update/{orderId}")]
        public async Task<ActionResult> Update(int orderId, OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@OrderDto}", orderDto);
                return BadRequest(ModelState);
            }

            var result = await _orderService.Update(orderId, orderDto);

            if (result.Success)
            {
                _logger.LogInformation("User updated successfully: {@Order}", result.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("Remove/{orderId}")]
        public async Task<ActionResult> Remove(int orderId)
        {
            var result = await _orderService.Remove(orderId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
