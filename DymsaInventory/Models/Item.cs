using System;
using System.Collections.Generic;

namespace DymsaInventory.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int? ItemGenreId { get; set; }

    public int? ItemCodeId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemDescription { get; set; }

    public bool? IsActive { get; set; }

    public virtual ItemCode? ItemCode { get; set; }

    public virtual ItemGenre? ItemGenre { get; set; }

    public virtual ICollection<PurchaseTransaction> PurchaseTransactions { get; } = new List<PurchaseTransaction>();
}
