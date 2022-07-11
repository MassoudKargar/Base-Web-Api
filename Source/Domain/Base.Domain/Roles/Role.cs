using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Roles;
public class Role : IEntity
{
    public Role()
    {
        RoleName = string.Empty;
        RoleCaption = string.Empty;
    }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public string RoleCaption { get; set; }
    public long PersonRoleId { get; set; }
}