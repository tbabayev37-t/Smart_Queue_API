using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Queue_API.DTOs;
using Smart_Queue_API.Services.Abstractions;

namespace Smart_Queue_API.Controllers
{
    [Route("api/queue")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        /// <summary> Retrieves all customers in the queue system.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResultDto<IEnumerable<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<IEnumerable<CustomerDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllinQueue();
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        /// <summary> Retrieves details of a specific customer by unique identifier.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GerById(Guid id)
        {
            var result = await _customerService.GetById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        /// <summary> Adds a new customer to the queue. </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]CustomerCreateDto dto)
        {
            var result = await _customerService.AddCustomerAsync(dto);
            if(!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        /// <summary>Updates an existing customer's information or queue status.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody]CustomerUpdateeDto dto)
        {
            var result  = await _customerService.UpdateCustomerAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        /// <summary>Removes a customer from the queue system by ID. </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>Serves the next waiting customer in the queue line. </summary>
        [HttpPost("next")]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<CustomerDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ServeNext()
        {
            var result = await _customerService.ServeNextCustomerAsync();
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
        /// <summary> Calculates the current queue position for a waiting customer.</summary>
        [HttpGet("{id}/position")]
        [ProducesResponseType(typeof(ResultDto<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<int>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPosition(Guid id)
        {
            var result = await _customerService.GetCustomerPositionAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

    }
}