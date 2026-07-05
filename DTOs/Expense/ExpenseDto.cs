using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Expense
{
    public class ExpenseCreateDto
    {
        public string ExpenseName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public string? UserId { get; set; }
        public string? VehicleId { get; set; } = null;
        public string? ReceiptNumber { get; set; }
        public string? Notes { get; set; }
    }

    public class ExpenseUpdateDto
    {
        public string? ExpenseName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public ExpenseCategory? ExpenseCategory { get; set; }
        public PaymentMode? PaymentMode { get; set; }
        public ExpenseStatus? ExpenseStatus { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? Notes { get; set; }
        public string? ApprovedBy { get; set; }
    }

    public class ExpenseResponseDto
    {
        public string ExpenseId { get; set; } = string.Empty;
        public string ExpenseName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public ExpenseStatus ExpenseStatus { get; set; }
        public string? UserId { get; set; }
        public string? VehicleId { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? Notes { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ExpenseMonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
        public Dictionary<string, decimal> ByCategory { get; set; } = new();
    }

    public class ExpenseCategoryReportDto
    {
        public ExpenseCategory Category { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
    }
}
