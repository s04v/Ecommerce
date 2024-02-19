using Core.Auth.Domain;
using Core.Catalog.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public partial class ApplicationDbContext
    { 
        private void ConfigureUserEntity(EntityTypeBuilder<User> builder)
        {
            builder
                 .ToTable("User")
                 .HasKey(o => o.Uuid);

            builder
                .HasOne<Role>(o => o.Role)
                .WithMany()
                .HasForeignKey(o => o.RoleId);
        }

        private void ConfigureLoginHistoryEntity(EntityTypeBuilder<LoginHistory> builder)
        {
            builder
                .ToTable("LoginHistory")
                .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne<User>()
                .WithMany(o => o.LoginHistory)
                .HasForeignKey(o => o.UserUuid);
        }

        private void ConfigureRoleEntity(EntityTypeBuilder<Role> builder)
        {
            builder
                .ToTable("Role")
                .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder
               .HasMany(e => e.Permissions)
               .WithMany()
               .UsingEntity<RolePermission>(
                    l => l.HasOne<Permission>().WithMany().HasForeignKey(o => o.PermissionId),
                    r => r.HasOne<Role>().WithMany().HasForeignKey(o => o.RoleId)
                );
        }

        private void ConfigurePermissionEntity(EntityTypeBuilder<Permission> builder)
        {
            builder
                .ToTable("Permission")
                .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }

        private void ConfigureRolePermissionEntity(EntityTypeBuilder<RolePermission> builder)
        {
            builder
                .ToTable("RolePermission")
                .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne<Role>(o => o.Role)
                .WithMany()
                .HasForeignKey(o => o.RoleId);

            builder
                .HasOne<Permission>(o => o.Permission)
                .WithMany()
                .HasForeignKey(o => o.PermissionId);
        }

        public void ConfigureCategoryEntity(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable("Category")
                .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }

        public void ConfigureProductAttributeEntity(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder
                .ToTable("ProductAttribute")
                .HasKey(o => o.Id);

            builder
                .HasOne<Category>()
                .WithMany(o => o.Attributes)
                .HasForeignKey(o => o.CategoryId);
        }

        public void ConfigureManufacturerEntity(EntityTypeBuilder<Manufacturer> builder)
        {
            builder
               .ToTable("Manufacturer")
               .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }

        public void ConfigureProductEntity(EntityTypeBuilder<Product> builder)
        {
            builder
               .ToTable("Product")
               .HasKey(o => o.Uuid);

            builder
                .HasOne<Category>(o => o.Category)
                .WithMany()
                .HasForeignKey(o => o.CategoryId);

            builder
               .HasOne<Manufacturer>(o => o.Manufacturer)
               .WithMany()
               .HasForeignKey(o => o.ManufacturerId);
        }

        public void ConfigureProductImageEntity(EntityTypeBuilder<ProductImage> builder)
        {
            builder
               .ToTable("ProductImage")
               .HasKey(o => o.Id);

            builder
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne<Product>()
                .WithMany(o => o.Images)
                .HasForeignKey(o => o.ProductUuid);
        }
    }
}
