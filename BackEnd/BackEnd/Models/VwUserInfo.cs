using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class VwUserInfo
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string PasswordHash { get; set; } = null!;
}
