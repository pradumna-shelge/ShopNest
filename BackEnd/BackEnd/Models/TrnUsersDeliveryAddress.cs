using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class TrnUsersDeliveryAddress
{
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? AddressLine { get; set; }

    public string? Zip { get; set; }

    public virtual MstUser? User { get; set; }
}
