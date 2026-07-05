using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class Expense
    {
        [Key]
        public string ExpenseId { get; set; } = $"PRCS-EXP-{Guid.NewGuid()}";
        [Required]
        public string ExpenseName { set; get; } = string.Empty;
        [Required]
        public decimal Amount { set; get; }
        public DateTime ExpenseDate { set; get; }
        public ExpenseCategory ExpenseCategory { set; get; }
        public PaymentMode PaymentMode { get; set; }
        public ExpenseStatus ExpenseStatus { set; get; }
        public string? Notes { set; get; }
        public string? ReceiptNumber { set; get; }
        public string? ApprovedBy { get; set; }
        // Navigation properties
        public string? UserId { get; set; } 
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public string? VehicleId { get; set; } 
        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
