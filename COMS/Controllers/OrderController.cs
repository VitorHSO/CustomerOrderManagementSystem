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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
                return BadRequest(ModelState);

            var result = await _orderService.Add(orderDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("Update/{orderId}")]
        public async Task<ActionResult> Update(int orderId, OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.Update(orderId, orderDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Remove/{orderId}")]
        public async Task<ActionResult> Remove(int orderId)
        {
            var result = await _orderService.Remove(orderId);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }
    }
}
