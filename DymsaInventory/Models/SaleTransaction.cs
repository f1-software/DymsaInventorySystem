using System;
using System.Collections.Generic;

namespace DymsaInventory.Models;

public partial class SaleTransaction
{
    public int SaleTransactionId { get; set; }

    public DateTime TransactionDate { get; set; }

    public int? ItemId { get; set; }

    public int? Qty { get; set; }

    public decimal? DiscountOrCommission { get; set; }

    public decimal? Net { get; set; }
    public decimal? Cost { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public bool? Status { get; set; }
    public virtual Item? Item { get; set; }
}
