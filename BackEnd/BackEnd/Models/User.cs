using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class User
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

    public virtual ICollection<AddToCart> AddToCarts { get; set; } = new List<AddToCart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
}
