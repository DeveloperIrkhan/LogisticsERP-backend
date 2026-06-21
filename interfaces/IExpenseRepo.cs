using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IExpenseRepo
    {
        Task<List<Expense>> GetByVehicleAsync(string vehicleId);
        Task<List<Expense>> GetByUserAsync(string userId);
        Task<List<Expense>> GetByStatusAsync(ExpenseStatus status);
        Task<List<Expense>> GetByCategoryAsync(ExpenseCategory category);
        Task<List<Expense>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<Expense>> GetByMonthAsync(int year, int month);
        Task<decimal> GetTotalByMonthAsync(int year, int month);
        Task<decimal> GetTotalByYearAsync(int year);
        Task<decimal> GetTotalByCategoryAsync(ExpenseCategory category);
        Task<decimal> GetTotalByVehicleAsync(string vehicleId);
    }
}
