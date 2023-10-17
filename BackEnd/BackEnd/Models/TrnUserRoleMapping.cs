using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class TrnUserRoleMapping
{
    public int MappingId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual MstUserRole? Role { get; set; }

    public virtual MstUser? User { get; set; }
}
