﻿using Common.Data;
using Core.Activities.Domain;
using Core.Auth.Domain;
using Core.Catalog.Domain;
using Core.Users;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<AdminActivity> Activity { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValue{ get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapColumnName(modelBuilder);
            SetDecimalType(modelBuilder);

            modelBuilder.Entity<User>(ConfigureUserEntity);
            modelBuilder.Entity<LoginHistory>(ConfigureLoginHistoryEntity);
            modelBuilder.Entity<Role>(ConfigureRoleEntity);
            modelBuilder.Entity<Permission>(ConfigurePermissionEntity);
            modelBuilder.Entity<RolePermission>(ConfigureRolePermissionEntity);
            modelBuilder.Entity<Category>(ConfigureCategoryEntity);
            modelBuilder.Entity<ProductAttribute>(ConfigureProductAttributeEntity);
            modelBuilder.Entity<Manufacturer>(ConfigureManufacturerEntity);
            modelBuilder.Entity<Product>(ConfigureProductEntity);
            modelBuilder.Entity<ProductImage>(ConfigureProductImageEntity);
            modelBuilder.Entity<AdminActivity>(ConfigureActivityEntity);
            modelBuilder.Entity<ProductAttributeValue>(ConfigureProductAttributeValue);
        }

        protected void MapColumnName(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name);
                }
            }
        }

        protected void SetDecimalType(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
             .SelectMany(t => t.GetProperties())
             .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
    }
}
