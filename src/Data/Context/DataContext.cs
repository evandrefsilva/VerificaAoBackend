using System;
using Data.Entities;
using System.Linq;
using Data.AuthEntities;
using Data.Configuration;
using Data.Candidates;
using Data.Entities.GeneralEntities;
using Microsoft.EntityFrameworkCore;
using Data.NewsVerfication;

namespace Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
         .SelectMany(t => t.GetForeignKeys())
         .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);

            ModelConfig(modelBuilder);
        }
        private void ModelConfig(ModelBuilder modelBuilder)
        {
            new AppSettingsConfiguration(modelBuilder.Entity<AppSettings>());
            new UserConfiguration(modelBuilder.Entity<User>());
            new VerificationStatusConfiguration(modelBuilder.Entity<VerificationStatus>());
            new RoleConfiguration(modelBuilder.Entity<Role>());
            // Configuração da chave primária composta para RolePermission
            modelBuilder.Entity<UserCategory>()
                .HasKey(uc => new { uc.CategoryId, uc.UserId });
            // Configuração da chave primária composta para RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // Configuração dos relacionamentos
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AppSettings> AppConfigurations { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<VerificationStatus> VerificationStatus { get; set; }
    }
}
