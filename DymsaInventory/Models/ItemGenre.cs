using System;
using System.Collections.Generic;

namespace DymsaInventory.Models;

public partial class ItemGenre
{
    public int ItemGenreId { get; set; }

    public string? ItemGenreName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
