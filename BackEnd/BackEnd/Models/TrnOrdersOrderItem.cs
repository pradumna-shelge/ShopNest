using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class TrnOrdersOrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual TrnOrder? Order { get; set; }

    public virtual MstProduct? Product { get; set; }
}
