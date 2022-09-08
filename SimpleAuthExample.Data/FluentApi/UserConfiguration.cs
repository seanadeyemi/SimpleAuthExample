
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAuthExample.Data.Models;
using System;

namespace SimpleAuthExample.Data.FluentApi
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            //Column Type Configuration
            builder.ToTable("Users");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasColumnType("varchar(100)").IsRequired();
            builder.Property(p => p.LastName).HasColumnType("varchar(100)").IsRequired();
            builder.Property(f => f.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(f => f.Email).HasMaxLength(50).IsRequired();
            builder.Property(p => p.PasswordHash).HasColumnName("Password").HasColumnType("varchar(250)");
            builder.Property(p => p.PhoneNumber).HasColumnType("varchar(15)");
            builder.Property(p => p.Email).HasColumnType("varchar(100)");
            builder.Property(p => p.NormalizedEmail).HasColumnType("varchar(100)");
            builder.Property(p => p.UserName).HasColumnType("varchar(100)");
            builder.Property(p => p.NormalizedUserName).HasColumnType("varchar(100)");
            builder.Property(p => p.ConcurrencyStamp).HasMaxLength(250).HasColumnType("varchar");
            builder.Property(p => p.SecurityStamp).HasColumnType("varchar(250)");
            builder.Property(p => p.FirstName).HasColumnType("varchar(100)");
            builder.Property(p => p.LastName).HasColumnType("varchar(100)");



            builder.HasData(new User
            {
                Id = 1,
                FirstName = "Adedeji",
                LastName = "Adeyemi",
                Email = "admin@test.ng",
                NormalizedEmail = "admin@test.ng".ToUpper(),
                UserName = "admin@test.ng",
                NormalizedUserName = "admin@test.ng".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PhoneNumber = "08031234567",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                LockoutEnd = null,
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAEAACcQAAAAEHz9jeDAGD5NrInBBafBqFjW3XbnNG4w08PuNblIMvwdU1kzpGQd8mX3ca28HlBPkA=="
            });

        }
    }
}
