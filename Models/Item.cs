using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class Item
    {
        [Key]
        public string ItemId { get; set; } = $"PRCS-ITM-{Guid.NewGuid()}";
        [Required]
        public string ItemName { get; set; } = string.Empty;
        public ItemCategory ItemCategory { get; set; }
        public ItemUnit Unit { get; set; }
        public decimal CurrentStock { get; set; }
        //When stock drops to 10L or below → system triggers an alert to reorder
        public decimal? ReorderLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //Nevigation Properties
        public ICollection<ItemPurchase> ItemPurchase { get; set; } = [];
        public ICollection<ItemSale> Sales { get; set; } = [];
    }


}
