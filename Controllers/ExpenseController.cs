using LogisticsERP.API.DTOs.Expense;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        [HttpPost("add-expense")]
        public async Task<IActionResult> Create([FromBody] ExpenseCreateDto dto)
        {
            if (dto == null) return BadRequest("Expense data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-expense/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ExpenseUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-expense/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-expenses")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-expense/{id}")]
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

        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetByUser([FromRoute] string userId)
        {
            var result = await _service.GetByUserAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] ExpenseStatus status)
        {
            var result = await _service.GetByStatusAsync(status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetByCategory([FromQuery] ExpenseCategory category)
        {
            var result = await _service.GetByCategoryAsync(category);
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

        [HttpGet("report/monthly")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetMonthlyReportAsync(year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("report/category")]
        public async Task<IActionResult> GetCategoryReport([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetCategoryReportAsync(year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("report/yearly")]
        public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
        {
            var result = await _service.GetYearlyReportAsync(year);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
