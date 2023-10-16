using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public int? ParentLocationId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Location> InverseParentLocation { get; set; } = new List<Location>();

    public virtual Location? ParentLocation { get; set; }
}
