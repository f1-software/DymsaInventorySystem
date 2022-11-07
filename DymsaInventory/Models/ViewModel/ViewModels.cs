namespace DymsaInventory.Models.ViewModel
{
    public class PurchaseViewModels
    {
        public int ItemId { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Cost { get; set; }
        public decimal? PriceCost { get; set; }
        public int TotalQty { get; set; }
    }
    public class IventoryStocksViewModels
    {
        public int ItemId { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? Qty { get; set; }

    }
    public class ProfitViewModels
    {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int TotalQty { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Net { get; set; }
        public decimal? Profit { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}

