using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class MstProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductImage { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal Mrprice { get; set; }

    public virtual ICollection<TrnAddToCart> TrnAddToCarts { get; set; } = new List<TrnAddToCart>();

    public virtual ICollection<TrnOrdersOrderItem> TrnOrdersOrderItems { get; set; } = new List<TrnOrdersOrderItem>();
}
