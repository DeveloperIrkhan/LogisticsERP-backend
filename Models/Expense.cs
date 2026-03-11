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
        public string ExpenseName { set; get; }
        [Required]
        public decimal Amount { set; get; }
        public DateTime ExpenseDate { set; get; }
        public ExpenseType ExpenseType { set; get; }
        public PaymentMode PaymentMode { get; set; }
        public ExpenseStatus ExpenseStatus { set; get; }
        public string Notes { set; get; }

        // Navigation properties
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("VehicleId")]
        public string? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
