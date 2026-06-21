using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _service.GetSummaryAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("vehicle-stats")]
        public async Task<IActionResult> GetVehicleStats()
        {
            var result = await _service.GetVehicleStatsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("expiry-alerts")]
        public async Task<IActionResult> GetExpiryAlerts()
        {
            var result = await _service.GetExpiryAlertsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("fuel-analytics")]
        public async Task<IActionResult> GetFuelAnalytics()
        {
            var result = await _service.GetFuelAnalyticsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("maintenance-analytics")]
        public async Task<IActionResult> GetMaintenanceAnalytics()
        {
            var result = await _service.GetMaintenanceAnalyticsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("expense-analytics")]
        public async Task<IActionResult> GetExpenseAnalytics()
        {
            var result = await _service.GetExpenseAnalyticsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
