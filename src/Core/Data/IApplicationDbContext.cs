using Core.Activities.Domain;
using Core.Auth.Domain;
using Core.Catalog.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> User { get; set; }
        DbSet<LoginHistory> LoginHistory { get; set; }
        DbSet<Role> Role{ get; set; }
        DbSet<Permission> Permission { get; set; }
        DbSet<RolePermission> RolePermission { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<ProductAttribute> ProductAttribute { get; set; }
        DbSet<Manufacturer> Manufacturer { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<ProductImage> ProductImage { get; set; }
        DbSet<AdminActivity> Activity { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    }
}
