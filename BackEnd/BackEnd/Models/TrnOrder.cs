using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class TrnOrder
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public long InvoiceNo { get; set; }

    public virtual ICollection<TrnOrdersOrderItem> TrnOrdersOrderItems { get; set; } = new List<TrnOrdersOrderItem>();

    public virtual MstUser? User { get; set; }
}
