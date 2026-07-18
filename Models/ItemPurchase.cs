using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class ItemPurchase
    {
        [Key]
        public string ItemPurchaseId { get; set; } = $"PRCS-PUR-{Guid.NewGuid()}";

        public string ItemId { get; set; } = string.Empty;
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; } = new();

        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public ItemTransactionStatus Status { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public string? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }

        public string? AddedBy { get; set; }
        [ForeignKey(nameof(AddedBy))]
        public User? User { get; set; }

        public string? ApprovedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
