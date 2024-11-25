using COMS.Application.DTOs.Customer;
using COMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CustomerDetailedDTO>>> GetAll()
        {

            var result = await _customerService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetById/{customerId}")]
        public async Task<ActionResult> GetById(int customerId)
        {
            var result = await _customerService.GetById(customerId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] CustomerDTO customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.Add(customerDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("Update/{customerId}")]
        public async Task<ActionResult> Update(int customerId, CustomerDTO customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.Update(customerId, customerDto);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Remove/{customerId}")]
        public async Task<ActionResult> Remove(int customerId)
        {
            var result = await _customerService.Remove(customerId);

            if (result.Status == "Success")
                return Ok(result);

            return BadRequest(result);
        }
    }
}
