using LogisticsERP.API.DTOs.LogisticsERP.API.DTOs.Dashboard;
using LogisticsERP.API.DTOs.Vehicle.LogisticsERP.API.DTOs.Reports;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<List<VehicleReportDto>>> GetVehicleReportAsync();
        Task<ApiResponse<List<FuelReportDto>>> GetFuelReportAsync(ReportFilterDto filter);
        Task<ApiResponse<List<MaintenanceReportDto>>> GetMaintenanceReportAsync(ReportFilterDto filter);
        Task<ApiResponse<List<ExpenseReportDto>>> GetExpenseReportAsync(ReportFilterDto filter);

        Task<byte[]> ExportVehicleReportPdfAsync();
        Task<byte[]> ExportFuelReportPdfAsync(ReportFilterDto filter);
        Task<byte[]> ExportMaintenanceReportPdfAsync(ReportFilterDto filter);
        Task<byte[]> ExportExpenseReportPdfAsync(ReportFilterDto filter);
        Task<byte[]> ExportMonthlyRosterPdfAsync(int month, int year);
        Task<byte[]> ExportDailyRosterPdfAsync(DateTime date);
    }
}
