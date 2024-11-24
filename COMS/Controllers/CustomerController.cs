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
        //private readonly IAuthenticateService _authenticateService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, /*IAuthenticateService authenticateService,*/ ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            //_authenticateService = authenticateService;
            _logger = logger;
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
            {
                _logger.LogWarning("Invalid model state for user: {@UserDto}", customerDto);
                return BadRequest(ModelState);
            }

            var result = await _customerService.Add(customerDto);

            if (result.Success)
            {
                _logger.LogInformation("User created successfully: {@User}", result.Data);
                return Ok(result.Data);
                //return CreatedAtAction(nameof(Add), new { id = user.Id }, user);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Update/{customerId}")]
        public async Task<ActionResult> Update(int customerId, CustomerDTO customerDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user: {@user}", customerDto);
                return BadRequest(ModelState);
            }

            var result = await _customerService.Update(customerId, customerDto);

            if (result.Success)
            {
                _logger.LogInformation("User updated successfully: {@User}", result.Data);
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("Remove/{customerId}")]
        public async Task<ActionResult> Remove(int customerId)
        {
            var result = await _customerService.Remove(customerId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
