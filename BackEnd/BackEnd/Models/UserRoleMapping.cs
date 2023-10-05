using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class UserRoleMapping
{
    public int MappingId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual UserRole? Role { get; set; }

    public virtual User? User { get; set; }
}
