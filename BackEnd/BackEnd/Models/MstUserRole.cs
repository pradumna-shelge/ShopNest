using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class MstUserRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<TrnUserRoleMapping> TrnUserRoleMappings { get; set; } = new List<TrnUserRoleMapping>();
}
