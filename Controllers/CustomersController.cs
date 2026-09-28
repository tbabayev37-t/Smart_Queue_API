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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllinQueue();
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GerById(Guid id)
        {
            var result = await _customerService.GetById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CustomerCreateDto dto)
        {
            var result = await _customerService.AddCustomerAsync(dto);
            if(!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]CustomerUpdateeDto dto)
        {
            var result  = await _customerService.UpdateCustomerAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost("next")]
        public async Task<IActionResult> ServeNext()
        {
            var result = await _customerService.ServeNextCustomerAsync();
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{id}/position")]
        public async Task<IActionResult> GetPosition(Guid id)
        {
            var result = await _customerService.GetCustomerPositionAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

    }
}
