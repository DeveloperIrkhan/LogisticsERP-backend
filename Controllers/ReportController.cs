using LogisticsERP.API.DTOs.Vehicle.LogisticsERP.API.DTOs.Reports;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service=service;
        }

        [HttpGet("vehicle-report")]
        public async Task<IActionResult> GetVehicleReport()
        {
            var result = await _service.GetVehicleReportAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("fuel-report")]
        public async Task<IActionResult> GetFuelReport([FromQuery] ReportFilterDto filter)
        {
            var result = await _service.GetFuelReportAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("maintenance-report")]
        public async Task<IActionResult> GetMaintenanceReport([FromQuery] ReportFilterDto filter)
        {
            var result = await _service.GetMaintenanceReportAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("expense-report")]
        public async Task<IActionResult> GetExpenseReport([FromQuery] ReportFilterDto filter)
        {
            var result = await _service.GetExpenseReportAsync(filter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── PDF EXPORTS ───────────────────────────────────────
        [HttpGet("export/vehicle-pdf")]
        public async Task<IActionResult> ExportVehiclePdf()
        {
            var pdf = await _service.ExportVehicleReportPdfAsync();
            return File(pdf, "application/pdf", $"VehicleReport_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/fuel-pdf")]
        public async Task<IActionResult> ExportFuelPdf([FromQuery] ReportFilterDto filter)
        {
            var pdf = await _service.ExportFuelReportPdfAsync(filter);
            return File(pdf, "application/pdf", $"FuelReport_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/maintenance-pdf")]
        public async Task<IActionResult> ExportMaintenancePdf([FromQuery] ReportFilterDto filter)
        {
            var pdf = await _service.ExportMaintenanceReportPdfAsync(filter);
            return File(pdf, "application/pdf", $"MaintenanceReport_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/expense-pdf")]
        public async Task<IActionResult> ExportExpensePdf([FromQuery] ReportFilterDto filter)
        {
            var pdf = await _service.ExportExpenseReportPdfAsync(filter);
            return File(pdf, "application/pdf", $"ExpenseReport_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/monthly-roster-pdf")]
        public async Task<IActionResult> ExportMonthlyRosterPdf([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Month must be between 1 and 12.");
            var pdf = await _service.ExportMonthlyRosterPdfAsync(month, year);
            if (pdf.Length == 0) return NotFound("No roster found for the given month and year.");
            return File(pdf, "application/pdf", $"MonthlyRoster_{month}_{year}.pdf");
        }

        [HttpGet("export/daily-roster-pdf")]
        public async Task<IActionResult> ExportDailyRosterPdf([FromQuery] DateTime date)
        {
            var pdf = await _service.ExportDailyRosterPdfAsync(date);
            if (pdf.Length == 0) return NotFound("No roster entries found for the given date.");
            return File(pdf, "application/pdf", $"DailyRoster_{date:yyyyMMdd}.pdf");
        }
    }
}
