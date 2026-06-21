using LogisticsERP.API.DTOs.Roster;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RosterController : ControllerBase
    {
        private readonly IRosterService _service;

        public RosterController(IRosterService service)
        {
            _service = service;
        }

        // ── ROSTER ────────────────────────────────────────────
        [HttpPost("create-roster")]
        public async Task<IActionResult> CreateRoster([FromBody] RosterCreateDto dto)
        {
            if (dto == null) return BadRequest("Roster data is required.");
            var result = await _service.CreateRosterAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-roster/{id}")]
        public async Task<IActionResult> UpdateRoster([FromRoute] string id, [FromBody] RosterUpdateDto dto)
        {
            var result = await _service.UpdateRosterAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-roster/{id}")]
        public async Task<IActionResult> GetRosterById([FromRoute] string id)
        {
            var result = await _service.GetRosterByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all-rosters")]
        public async Task<IActionResult> GetAllRosters()
        {
            var result = await _service.GetAllRostersAsync();
            return Ok(result);
        }

        [HttpDelete("delete-roster/{id}")]
        public async Task<IActionResult> DeleteRoster([FromRoute] string id)
        {
            var result = await _service.DeleteRosterAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("publish-roster/{id}")]
        public async Task<IActionResult> PublishRoster([FromRoute] string id)
        {
            var result = await _service.PublishRosterAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("approve-roster/{id}")]
        public async Task<IActionResult> ApproveRoster([FromRoute] string id, [FromQuery] string approvedBy)
        {
            if (string.IsNullOrEmpty(approvedBy)) return BadRequest("ApprovedBy is required.");
            var result = await _service.ApproveRosterAsync(id, approvedBy);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("close-roster/{id}")]
        public async Task<IActionResult> CloseRoster([FromRoute] string id)
        {
            var result = await _service.CloseRosterAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── ENTRIES ───────────────────────────────────────────
        [HttpPost("add-entry")]
        public async Task<IActionResult> CreateEntry([FromBody] RosterEntryCreateDto dto)
        {
            if (dto == null) return BadRequest("Entry data is required.");
            var result = await _service.CreateEntryAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("bulk-add-entries")]
        public async Task<IActionResult> BulkCreateEntries([FromBody] RosterBulkCreateDto dto)
        {
            if (dto == null) return BadRequest("Entries data is required.");
            var result = await _service.BulkCreateEntriesAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-entry/{id}")]
        public async Task<IActionResult> UpdateEntry([FromRoute] string id, [FromBody] RosterEntryUpdateDto dto)
        {
            var result = await _service.UpdateEntryAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-entry/{id}")]
        public async Task<IActionResult> DeleteEntry([FromRoute] string id)
        {
            var result = await _service.DeleteEntryAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── ENTRY STATUS ──────────────────────────────────────
        [HttpPut("entry-in-progress/{id}")]
        public async Task<IActionResult> MarkInProgress([FromRoute] string id)
        {
            var result = await _service.MarkAsInProgressAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("entry-completed/{id}")]
        public async Task<IActionResult> MarkCompleted([FromRoute] string id, [FromQuery] string dutyLogId)
        {
            if (string.IsNullOrEmpty(dutyLogId)) return BadRequest("DutyLogId is required.");
            var result = await _service.MarkAsCompletedAsync(id, dutyLogId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("entry-missed/{id}")]
        public async Task<IActionResult> MarkMissed([FromRoute] string id)
        {
            var result = await _service.MarkAsMissedAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("cancel-entry/{id}")]
        public async Task<IActionResult> CancelEntry([FromRoute] string id, [FromQuery] string reason)
        {
            if (string.IsNullOrEmpty(reason)) return BadRequest("Cancellation reason is required.");
            var result = await _service.CancelEntryAsync(id, reason);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── VIEWS ─────────────────────────────────────────────
        [HttpGet("monthly-view")]
        public async Task<IActionResult> GetMonthlyView([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetMonthlyViewAsync(month, year);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("daily-view")]
        public async Task<IActionResult> GetDailyView([FromQuery] DateTime date)
        {
            var result = await _service.GetDailyViewAsync(date);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("driver-view/{driverId}")]
        public async Task<IActionResult> GetDriverView([FromRoute] string driverId, [FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var result = await _service.GetDriverRosterViewAsync(driverId, month, year);
            return result.Success ? Ok(result) : NotFound(result);
        }
}
}
