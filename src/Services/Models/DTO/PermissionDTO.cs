using Data.AuthEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    /// <summary>
    /// DTO para permissões.
    /// </summary>
    public class PermissionDTO
    {
        public PermissionDTO() { }
        public PermissionDTO(Permission permission)
        {
            Name = permission.Name;
            Module = permission.Module;
        }
        public string Name { get; set; }
        public string Module { get; set; }
    }
}
