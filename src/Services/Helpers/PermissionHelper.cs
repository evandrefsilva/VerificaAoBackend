using Services.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public static class PermissionHelper
    {
        /// <summary>
        /// Gera uma lista de permissões para um módulo específico.
        /// </summary>
        /// <param name="module">Nome do módulo (ex: "News", "Users", "Tags").</param>
        /// <returns>Lista de permissões.</returns>
        public static List<PermissionDTO> GetPermissionsForModule(string module)
        {
            var permissions = new List<PermissionDTO>
            {
                new PermissionDTO { Name = $"CAN_CREATE_{module.ToUpper()}", Module = module.ToUpper() },
                new PermissionDTO { Name = $"CAN_EDIT_{module.ToUpper()}", Module = module.ToUpper() },
                new PermissionDTO { Name = $"CAN_DELETE_{module.ToUpper()}", Module = module.ToUpper() },
                new PermissionDTO { Name = $"CAN_PUBLISH_{module.ToUpper()}", Module = module.ToUpper() }
            };
            return permissions;
        }
    }
}
