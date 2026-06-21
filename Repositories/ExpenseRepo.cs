using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class ExpenseRepo : IExpenseRepo
    {
        private readonly AppDbContext _context;

        public ExpenseRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetByVehicleAsync(string vehicleId)
        {
            return await _context.Expenses
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByUserAsync(string userId)
        {
            return await _context.Expenses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByStatusAsync(ExpenseStatus status)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseStatus == status)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByCategoryAsync(ExpenseCategory category)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseCategory == category)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseDate >= from && x.ExpenseDate <= to)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByMonthAsync(int year, int month)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseDate.Year == year && x.ExpenseDate.Month == month)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByMonthAsync(int year, int month)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseDate.Year == year && x.ExpenseDate.Month == month)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalByYearAsync(int year)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseDate.Year == year)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalByCategoryAsync(ExpenseCategory category)
        {
            return await _context.Expenses
                .Where(x => x.ExpenseCategory == category)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalByVehicleAsync(string vehicleId)
        {
            return await _context.Expenses
                .Where(x => x.VehicleId == vehicleId)
                .SumAsync(x => x.Amount);
        }
    }
}
