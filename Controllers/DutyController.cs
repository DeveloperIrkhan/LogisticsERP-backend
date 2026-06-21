using LogisticsERP.API.DTOs.DutyLogs;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DutyController : ControllerBase
    {
        private readonly IDutyLogService _service;

        public DutyController(IDutyLogService service)
        {
            _service = service;
        }
        [HttpPost("add-duty")]
        public async Task<IActionResult> Create([FromBody] DutyCreateDto dto)
        {
            if (dto == null) return BadRequest("Duty data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-duty/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] DutyUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-duty/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-duties")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-duty/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle([FromRoute] string vehicleId)
        {
            var result = await _service.GetByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-driver/{driverId}")]
        public async Task<IActionResult> GetByDriver([FromRoute] string driverId)
        {
            var result = await _service.GetByDriverAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] DutyStatus status)
        {
            var result = await _service.GetByStatusAsync(status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-by-date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetByDateRangeAsync(from, to);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-active-duties")]
        public async Task<IActionResult> GetActiveDuties()
        {
            var result = await _service.GetActiveDutiesAsync();
            return Ok(result);
        }

        [HttpPut("start-duty/{id}")]
        public async Task<IActionResult> StartDuty([FromRoute] string id)
        {
            var result = await _service.StartDutyAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("end-duty/{id}")]
        public async Task<IActionResult> EndDuty([FromRoute] string id, [FromBody] EndDutyDto dto)
        {
            if (dto == null) return BadRequest("End duty data is required.");
            var result = await _service.EndDutyAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("approve-duty/{id}")]
        public async Task<IActionResult> ApproveDuty([FromRoute] string id, [FromQuery] string approvedBy)
        {
            if (string.IsNullOrEmpty(approvedBy)) return BadRequest("ApprovedBy is required.");
            var result = await _service.ApproveDutyAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("cancel-duty/{id}")]
        public async Task<IActionResult> CancelDuty([FromRoute] string id, [FromQuery] string reason)
        {
            if (string.IsNullOrEmpty(reason)) return BadRequest("Cancellation reason is required.");
            var result = await _service.CancelDutyAsync(id, reason);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
