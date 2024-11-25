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

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
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

        [HttpGet("GetById/{orderItemId}")]
        public async Task<ActionResult> GetById(int orderItemId)
        {
            var result = await _orderItemService.GetById(orderItemId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] OrderItemDTO orderItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderItemService.Add(orderItemDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("Update/{orderItemId}")]
        public async Task<ActionResult> Update(int orderItemId, OrderItemDTO orderItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderItemService.Update(orderItemId, orderItemDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Remove/{orderItemId}")]
        public async Task<ActionResult> Remove(int orderItemId)
        {
            var result = await _orderItemService.Remove(orderItemId);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }
    }
}
