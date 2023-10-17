using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class TrnAddToCart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime AddedDateTime { get; set; }

    public decimal Price { get; set; }

    public virtual MstProduct? Product { get; set; }

    public virtual MstUser? User { get; set; }
}
