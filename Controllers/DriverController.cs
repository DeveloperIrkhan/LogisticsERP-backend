using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _service;

        public DriverController(IDriverService service)
        {
            _service = service;
        }
        [HttpGet("get-all-driver")]
        public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetDrivers()
        {
            return Ok();
        }

        [HttpPost("add-driver")]
        public async Task<ActionResult<DriverResponseDto>> AddNewDriver([FromBody] DriverCreateDto driverCreateDto)
        {
            if (driverCreateDto == null)
            {
                return BadRequest("Invalid driver data.");
            }
            var result = await _service.CreateDriver(driverCreateDto);
            if (result == null)
            {
                return BadRequest("Failed to create driver.");
            }
            return Ok(result);
        }
    }
}
