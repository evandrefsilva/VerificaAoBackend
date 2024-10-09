using System;
using System.Collections.Generic;
using System.Text;

namespace Data.AuthEntities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Module { get; set; } 

        // Relação com RolePermission
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
