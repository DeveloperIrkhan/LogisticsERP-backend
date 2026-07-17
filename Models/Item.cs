using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.Models
{
    public class Item
    {
        public string ItemId { get; set; } =$"PRCS-ITM-{Guid.NewGuid()}";
        [Required]
        public string ItemName { get; set; } = string.Empty;
        public ItemCategory ItemCategory { get; set; }
        public ItemUnit Unit { get; set; }



    }
}
