using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemPurchaseController : ControllerBase
    {
        private readonly IItemPurchaseService _service;

        public ItemPurchaseController(IItemPurchaseService service)
        {
            _service = service;
        }

        [HttpPost("add-purchase")]
        public async Task<IActionResult> Create([FromBody] ItemPurchaseCreateDto dto)
        {
            if (dto == null) return BadRequest("Purchase data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-purchase/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ItemPurchaseUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-purchase/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-purchases")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-purchase/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-item/{itemId}")]
        public async Task<IActionResult> GetByItem([FromRoute] string itemId)
        {
            var result = await _service.GetByItemAsync(itemId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle([FromRoute] string vehicleId)
        {
            var result = await _service.GetByVehicleAsync(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] ItemTransactionStatus status)
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

        [HttpPut("mark-paid/{id}")]
        public async Task<IActionResult> MarkPaid([FromRoute] string id, [FromQuery] string approvedBy)
        {
            if (string.IsNullOrEmpty(approvedBy)) return BadRequest("ApprovedBy is required.");
            var result = await _service.MarkPaidAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("report/monthly")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetMonthlyReportAsync(year, month);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
