using Core.Auth.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
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
    }
}
