namespace LogisticsERP.API.DTOs.Item
{
    public class ItemPurchaseMonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
    }

    public class ItemSaleMonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
    }

}
