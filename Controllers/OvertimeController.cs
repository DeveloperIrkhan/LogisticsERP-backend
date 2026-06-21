using LogisticsERP.API.DTOs.Overtime;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimeController : ControllerBase
    {
        private readonly IOvertimeService _service;

        public OvertimeController(IOvertimeService service)
        {
            _service = service;
        }

        [HttpPost("add-overtime")]
        public async Task<IActionResult> Create([FromBody] OvertimeCreateDto dto)
        {
            if (dto == null) return BadRequest("Overtime data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-overtime/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] OvertimeUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-overtime/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-overtime")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-overtime/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-driver/{driverId}")]
        public async Task<IActionResult> GetByDriver([FromRoute] string driverId)
        {
            var result = await _service.GetByDriverAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-duty/{dutyId}")]
        public async Task<IActionResult> GetByDuty([FromRoute] string dutyId)
        {
            var result = await _service.GetByDutyAsync(dutyId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] OvertimeStatus status)
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

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve([FromRoute] string id, [FromQuery] string approvedBy)
        {
            if (string.IsNullOrEmpty(approvedBy)) return BadRequest("ApprovedBy is required.");
            var result = await _service.ApproveAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject([FromRoute] string id, [FromQuery] string approvedBy)
        {
            if (string.IsNullOrEmpty(approvedBy)) return BadRequest("ApprovedBy is required.");
            var result = await _service.RejectAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("report/monthly/{driverId}")]
        public async Task<IActionResult> GetMonthlyReport([FromRoute] string driverId, [FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetMonthlyReportByDriverAsync(driverId, year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("report/total/{driverId}")]
        public async Task<IActionResult> GetTotalByDriver([FromRoute] string driverId)
        {
            var result = await _service.GetTotalByDriverAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
