using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightDoc
{
    public class FlightDocDb : IdentityDbContext<ApplicationUser>
    {
        public FlightDocDb(DbContextOptions<FlightDocDb> options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Role> Roles { get; set; }
        /*public DbSet<Permission> Permissions { get; set; }*/
        public DbSet<FlightPassenger> FlightPassengers { get; set; }
        public DbSet<FlightCrew> FlightCrews { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole<string>>()
    .HasKey(ur => new { ur.UserId, ur.RoleId });
 
            modelBuilder.Entity<UserRole>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

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
