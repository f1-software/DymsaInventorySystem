using System;
using System.Collections.Generic;

namespace DymsaInventory.Models;

public partial class PurchaseTransaction
{
    public int PurchaseTransactionId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? ItemId { get; set; }

    public decimal? Cost { get; set; }

    public int? Qty { get; set; }

    public decimal? AdditionalFee { get; set; }

    public string? Comment { get; set; }

    public bool? IsActive { get; set; }

    public virtual Item? Item { get; set; }
}
