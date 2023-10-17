using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class MstLocation
{
    public int LocationId { get; set; }

    public int? ParentLocationId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MstLocation> InverseParentLocation { get; set; } = new List<MstLocation>();

    public virtual MstLocation? ParentLocation { get; set; }
}
