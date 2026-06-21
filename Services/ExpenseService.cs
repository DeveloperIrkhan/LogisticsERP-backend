using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Expense;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class ExpenseService : ServiceBaseFunctions, IExpenseService
    {
        private readonly IGenericRepo<Expense> _genericRepo;
        private readonly IExpenseRepo _expenseRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ExpenseService(
            IGenericRepo<Expense> genericRepo,
            IExpenseRepo expenseRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _expenseRepo = expenseRepo;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApiResponse<ExpenseResponseDto>> CreateAsync(ExpenseCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return Fail<ExpenseResponseDto>("please provide the fields data.");
                var expense = _mapper.Map<Expense>(dto);
                expense.ExpenseStatus = ExpenseStatus.Pending;
                await _genericRepo.AddAsync(expense);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ExpenseResponseDto>(expense), "Expense created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseResponseDto>> UpdateAsync(string id, ExpenseUpdateDto dto)
        {
            try
            {
                var expense = await _genericRepo.GetByIdAsync(id);
                if (expense == null)
                    return Fail<ExpenseResponseDto>("Expense not found.");

                if (dto.ExpenseName != null) expense.ExpenseName = dto.ExpenseName;
                if (dto.Amount.HasValue) expense.Amount = dto.Amount.Value;
                if (dto.ExpenseDate.HasValue) expense.ExpenseDate = dto.ExpenseDate.Value;
                if (dto.ExpenseCategory.HasValue) expense.ExpenseCategory = dto.ExpenseCategory.Value;
                if (dto.PaymentMode.HasValue) expense.PaymentMode = dto.PaymentMode.Value;
                if (dto.ExpenseStatus.HasValue) expense.ExpenseStatus = dto.ExpenseStatus.Value;
                if (dto.ReceiptNumber != null) expense.ReceiptNumber = dto.ReceiptNumber;
                if (dto.Notes != null) expense.Notes = dto.Notes;
                if (dto.ApprovedBy != null) expense.ApprovedBy = dto.ApprovedBy;

                await _genericRepo.Update(expense);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ExpenseResponseDto>(expense), "Expense updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var expense = await _genericRepo.GetByIdAsync(id);
                if (expense == null)
                    return Fail<ExpenseResponseDto>("Expense not found.");

                return Ok(_mapper.Map<ExpenseResponseDto>(expense), "Expense fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetAllAsync()
        {
            try
            {
                var expenses = await _genericRepo.GetAllAsync();
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var expense = await _genericRepo.GetByIdAsync(id);
                if (expense == null)
                    return Fail<bool>("Expense not found.");

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Expense deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try
            {
                var expenses = await _expenseRepo.GetByVehicleAsync(vehicleId);
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetByUserAsync(string userId)
        {
            try
            {
                var expenses = await _expenseRepo.GetByUserAsync(userId);
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found for user.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetByStatusAsync(ExpenseStatus status)
        {
            try
            {
                var expenses = await _expenseRepo.GetByStatusAsync(status);
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetByCategoryAsync(ExpenseCategory category)
        {
            try
            {
                var expenses = await _expenseRepo.GetByCategoryAsync(category);
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found in category {category}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ExpenseResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<ExpenseResponseDto>>("'From' date cannot be greater than 'To' date.");

                var expenses = await _expenseRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<ExpenseResponseDto>>(expenses);
                return Ok(result, $"{result.Count} expense(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseResponseDto>> ApproveAsync(string id, string approvedBy)
        {
            try
            {
                var expense = await _genericRepo.GetByIdAsync(id);
                if (expense == null)
                    return Fail<ExpenseResponseDto>("Expense not found.");

                if (expense.ExpenseStatus == ExpenseStatus.Approved)
                    return Fail<ExpenseResponseDto>("Expense is already approved.");

                expense.ExpenseStatus = ExpenseStatus.Approved;
                expense.ApprovedBy = approvedBy;

                await _genericRepo.Update(expense);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ExpenseResponseDto>(expense), "Expense approved successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseResponseDto>> RejectAsync(string id, string approvedBy)
        {
            try
            {
                var expense = await _genericRepo.GetByIdAsync(id);
                if (expense == null)
                    return Fail<ExpenseResponseDto>("Expense not found.");

                if (expense.ExpenseStatus == ExpenseStatus.Rejected)
                    return Fail<ExpenseResponseDto>("Expense is already rejected.");

                expense.ExpenseStatus = ExpenseStatus.Rejected;
                expense.ApprovedBy = approvedBy;

                await _genericRepo.Update(expense);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ExpenseResponseDto>(expense), "Expense rejected successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseMonthlyReportDto>> GetMonthlyReportAsync(int year, int month)
        {
            try
            {
                var expenses = await _expenseRepo.GetByMonthAsync(year, month);
                var total = await _expenseRepo.GetTotalByMonthAsync(year, month);

                var byCategory = expenses
                    .GroupBy(x => x.ExpenseCategory.ToString())
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

                var report = new ExpenseMonthlyReportDto
                {
                    Year = year,
                    Month = month,
                    TotalAmount = total,
                    TotalRecords = expenses.Count,
                    ByCategory = byCategory
                };

                return Ok(report, $"Monthly expense report for {year}/{month} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseMonthlyReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── CATEGORY REPORT ───────────────────────────────────
        public async Task<ApiResponse<List<ExpenseCategoryReportDto>>> GetCategoryReportAsync(int year, int month)
        {
            try
            {
                var expenses = await _expenseRepo.GetByMonthAsync(year, month);

                var report = expenses
                    .GroupBy(x => x.ExpenseCategory)
                    .Select(g => new ExpenseCategoryReportDto
                    {
                        Category = g.Key,
                        TotalAmount = g.Sum(x => x.Amount),
                        TotalRecords = g.Count()
                    })
                    .ToList();

                return Ok(report, $"{report.Count} category report(s) for {year}/{month}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseCategoryReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpenseMonthlyReportDto>> GetYearlyReportAsync(int year)
        {
            try
            {
                var total = await _expenseRepo.GetTotalByYearAsync(year);
                var expenses = await _expenseRepo.GetByDateRangeAsync(
                    new DateTime(year, 1, 1),
                    new DateTime(year, 12, 31));

                var byCategory = expenses
                    .GroupBy(x => x.ExpenseCategory.ToString())
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

                var report = new ExpenseMonthlyReportDto
                {
                    Year = year,
                    TotalAmount = total,
                    TotalRecords = expenses.Count,
                    ByCategory = byCategory
                };

                return Ok(report, $"Yearly expense report for {year} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseMonthlyReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
