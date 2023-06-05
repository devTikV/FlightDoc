global using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FlightDoc
{

    public class FlightDocDb : IdentityDbContext<ApplicationUser>
    {
        public FlightDocDb(DbContextOptions<FlightDocDb> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pilot> Pilots { get; set; }
        public DbSet<FlightAttendant> FlightAttendants { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<FlightPassenger> FlightPassengers { get; set; }
        public DbSet<FlightCrew> FlightCrews { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
       
       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            // Thiết lập các quan hệ, ràng buộc và chỉnh sửa bảng (nếu cần)
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();

            modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<FlightCrew>()
            .HasOne(fc => fc.Flight)
            .WithMany(f => f.FlightCrew)
            .HasForeignKey(fc => fc.FlightId);

            modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<FlightPassenger>()
            .HasOne(fp => fp.Flight)
            .WithMany(f => f.FlightPassengers)
            .HasForeignKey(fp => fp.FlightId);

        modelBuilder.Entity<FlightPassenger>()
            .HasOne(fp => fp.Passenger)
            .WithMany(p => p.FlightPassengers)
            .HasForeignKey(fp => fp.PassengerId);

        modelBuilder.Entity<FlightCrew>()
            .HasOne(fc => fc.Flight)
            .WithMany(f => f.FlightCrew)
            .HasForeignKey(fc => fc.FlightId);

        modelBuilder.Entity<FlightCrew>()
            .HasOne(fc => fc.Pilot)
            .WithMany(p => p.FlightCrew)
            .HasForeignKey(fc => fc.PilotId);

        modelBuilder.Entity<FlightCrew>()
            .HasOne(fc => fc.FlightAttendant)
            .WithMany(fa => fa.FlightCrew)
            .HasForeignKey(fc => fc.FlightAttendantId);

            modelBuilder.Entity<JwtToken>()
          .HasOne(jt => jt.User)
          .WithMany(u => u.JwtTokens)
          .HasForeignKey(jt => jt.UserId);
        }

    }
}
