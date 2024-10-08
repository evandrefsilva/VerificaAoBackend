﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data.AuthEntities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
