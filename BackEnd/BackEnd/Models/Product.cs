using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductImage { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? Mrprice { get; set; }

    public virtual ICollection<AddToCart> AddToCarts { get; set; } = new List<AddToCart>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
