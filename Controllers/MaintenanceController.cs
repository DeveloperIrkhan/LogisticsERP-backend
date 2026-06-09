using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _service;
        public MaintenanceController(IMaintenanceService maintenaceService)
        {
            _service = maintenaceService;
        }

        
        [HttpPost("add-maintenance")]
        public async Task<ActionResult<MaintenanceCreateDto>> CreateMaintenance([FromBody] MaintenanceCreateDto dto)
        {
            if (dto == null) return BadRequest("maintenace data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPut("update-maintenance/{id}")]
        public async Task<ActionResult<MaintenanceCreateDto>> UpdateMaintenance(string id,
            [FromBody] MaintenanceUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("id is required.");
            if (dto == null) return BadRequest("maintenace data is required.");

            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("get-maintenance/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-maintenance")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-maintenance/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(string vehicleId)
        {
            var result = await _service.GetByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetByDateRangeAsync(from, to);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("cost/vehicle/{vehicleId}")]
        public async Task<IActionResult> GetTotalCostByVehicle(string vehicleId)
        {
            var result = await _service.GetTotalCostByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("cost/monthly")]
        public async Task<IActionResult> GetMonthlyCost([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetMonthlyCostAsync(year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("cost/yearly")]
        public async Task<IActionResult> GetYearlyCost([FromQuery] int year)
        {
            var result = await _service.GetYearlyCostAsync(year);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming([FromQuery] int days = 30)
        {
            var result = await _service.GetUpcomingMaintenanceAsync(days);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
