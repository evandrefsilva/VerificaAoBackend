using Data.AuthEntities;
using Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class PermissionSeeder
    {
        /// <summary>
        /// Realiza o seeding das permissões no banco de dados.
        /// </summary>
        /// <param name="serviceProvider">ServiceProvider para obter o DataContext.</param>
        /// <returns></returns>
        public static async Task SeedPermissionsAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Definir os módulos que possuem permissões
            var modules = new[] { "News", "Users", "Categories", "Verifications" }; // Adicione mais módulos conforme necessário

            foreach (var module in modules)
            {
                var permissions = PermissionHelper.GetPermissionsForModule(module);

                foreach (var perm in permissions)
                {
                    // Verifica se a permissão já existe
                    if (!context.Permissions.Any(p => p.Name == perm.Name && p.Module == perm.Module))
                    {
                        context.Permissions.Add(new Permission
                        {
                            Name = perm.Name,
                            Module = perm.Module
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
