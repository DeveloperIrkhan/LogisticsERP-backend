using LogisticsERP.API.DTOs.Fuel;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelController : ControllerBase
    {
        private readonly IFuelService _fuelService;

        public FuelController(IFuelService fuelService)
        {
            _fuelService = fuelService;
        }


        [HttpPost("add-fuel")]
        public async Task<IActionResult> Create([FromBody] FuelCreateDto dto)
        {
            if (dto == null) return BadRequest("Fuel data is required.");
            var result = await _fuelService.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-fuel/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FuelUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _fuelService.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-fuel/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _fuelService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-fuel")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _fuelService.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-fuel/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _fuelService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(string vehicleId)
        {
            var result = await _fuelService.GetByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-driver/{driverId}")]
        public async Task<IActionResult> GetByDriver(string driverId)
        {
            var result = await _fuelService.GetByDriverAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _fuelService.GetByDateRangeAsync(from, to);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("consumption/vehicle/{vehicleId}")]
        public async Task<IActionResult> GetConsumptionByVehicle(string vehicleId)
        {
            var result = await _fuelService.GetConsumptionByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("cost/monthly")]
        public async Task<IActionResult> GetMonthlyCost([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");
            var result = await _fuelService.GetMonthlyCostAsync(year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("cost/yearly")]
        public async Task<IActionResult> GetYearlyCost([FromQuery] int year)
        {
            var result = await _fuelService.GetYearlyCostAsync(year);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
