using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Item
{
    public class ItemPurchaseCreateDto
    {
        public string ItemId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public string? VehicleId { get; set; } = null;
        public string? AddedBy { get; set; }
        public string? Notes { get; set; }
    }

    public class ItemPurchaseUpdateDto
    {
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public PaymentMode? PaymentMode { get; set; }
        public ItemTransactionStatus? Status { get; set; }
        public string? VehicleId { get; set; }
        public string? Notes { get; set; }
        public string? ApprovedBy { get; set; }
    }

    public class ItemPurchaseResponseDto
    {
        public string ItemPurchaseId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public ItemTransactionStatus Status { get; set; }
        public string? VehicleId { get; set; }
        public string? AddedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
