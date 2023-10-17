using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class MstUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public string? Otp { get; set; }

    public DateTime? OtpdateTime { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? LoginPcno { get; set; }

    public string? ResetLink { get; set; }

    public DateTime? ResetLinkExpiration { get; set; }

    public virtual ICollection<TrnAddToCart> TrnAddToCarts { get; set; } = new List<TrnAddToCart>();

    public virtual ICollection<TrnOrder> TrnOrders { get; set; } = new List<TrnOrder>();

    public virtual ICollection<TrnUserRoleMapping> TrnUserRoleMappings { get; set; } = new List<TrnUserRoleMapping>();

    public virtual ICollection<TrnUsersDeliveryAddress> TrnUsersDeliveryAddresses { get; set; } = new List<TrnUsersDeliveryAddress>();
}
