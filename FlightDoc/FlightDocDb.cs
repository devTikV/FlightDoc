﻿using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightDoc
{
    public class FlightDocDb : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public FlightDocDb(DbContextOptions<FlightDocDb> options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
      
        /*public DbSet<Permission> Permissions { get; set; }*/
        public DbSet<FlightPassenger> FlightPassengers { get; set; }
        public DbSet<FlightCrew> FlightCrews { get; set; }

        public DbSet<Pilot> Pilots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");

            // set id user và role
            modelBuilder.Entity<Role>()
             .Property(u => u.Id)
              .HasColumnType("nvarchar(100)");

            modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Id)
            .HasColumnType("nvarchar(100)");
            // ///////////////////////////////////////////////////////
            modelBuilder.Entity<UserRole>()
             .Property(u => u.CreatedAt)
             .HasDefaultValue(DateTime.UtcNow);

           


            /////////////////////////////////////////////////////////////
            modelBuilder.Entity<ApplicationUser>()
             .Property(u => u.EmailConfirmed)
             .HasDefaultValue(true);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.PhoneNumberConfirmed)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.TwoFactorEnabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.AccessFailedCount)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LockoutEnd)
              .HasDefaultValue(null);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LockoutEnabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.SecurityStamp)
                .HasDefaultValue(Guid.NewGuid().ToString());

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.RefreshToken)
                .HasDefaultValue(null);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.AccessFailedCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<FlightCrew>()
                .HasOne(fc => fc.Flight)
                .WithMany(f => f.FlightCrew)
                .HasForeignKey(fc => fc.FlightId);

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
        }
    }
}
