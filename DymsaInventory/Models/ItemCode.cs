using System;
using System.Collections.Generic;

namespace DymsaInventory.Models;

public partial class ItemCode
{
    public int ItemCodeId { get; set; }

    public string? ItemCodeName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
