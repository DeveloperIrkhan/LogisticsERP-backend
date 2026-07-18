using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Item
{
    public class ItemCreateDto
    {
        public string ItemName { get; set; } = string.Empty;
        public ItemCategory ItemCategory { get; set; }
        public ItemUnit ItemUnit { get; set; }
        public decimal? RecorderLevel { get; set; }
        public string? Description { get; set; }
        public decimal OpeningStock { get; set; } = 0;
    }
    public class ItemUpdateDto
    {
        public string? ItemName { get; set; }
        public ItemCategory? ItemCategory { get; set; }
        public ItemUnit? Unit { get; set; }
        public decimal? ReorderLevel { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }


    public class ItemResponseDto
    {
        public string ItemId { get; set; }
        public string ItemName
        {
            get; set;
        } = string.Empty;
        public ItemCategory ItemCategory { get; set; }
        public ItemUnit Unit { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ItemStockReportDto
    {
        public string ItemId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public ItemCategory ItemCategory { get; set; }
        public ItemUnit Unit { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public bool IsLowStock { get; set; }
        public decimal TotalPurchasedQty { get; set; }
        public decimal TotalSoldQty { get; set; }
    }

}
