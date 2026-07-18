using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemController(IItemService service)
        {
            _service = service;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> Create([FromBody] ItemCreateDto dto)
        {
            if (dto == null) return BadRequest("Item data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-item/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ItemUpdateDto dto)
        {
            if (dto == null) return BadRequest("Update data is required.");
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-item/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-items")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete-item/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetByCategory([FromQuery] ItemCategory category)
        {
            var result = await _service.GetByCategoryAsync(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _service.GetActiveAsync();
            return Ok(result);
        }

        [HttpGet("get-low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _service.GetLowStockAsync();
            return Ok(result);
        }

        [HttpGet("report-and-stock")]
        public async Task<IActionResult> GetStockReport()
        {
            var result = await _service.GetStockReportAsync();
            return Ok(result);
        }
    }
}
