using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
using ParkingManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement
{
    internal class ParkingDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<VehicleSession> VehicleSessions { get; set; }
        // ADD THIS NEW DbSet:
        public DbSet<RegularParkingSession> RegularParkingSessions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=SystemAdmin\\SQLEXPRESS;Initial Catalog=ParkingSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Client>().HasKey(c => c.ClientID);
            modelBuilder.Entity<Client>().Property(c => c.ClientID).HasMaxLength(10);
            modelBuilder.Entity<Client>().Property(c => c.IDPicture).HasColumnType("NVARCHAR(MAX)");

            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleID);
            modelBuilder.Entity<Vehicle>().Property(v => v.VehicleID).HasMaxLength(10);
            modelBuilder.Entity<Vehicle>().Property(v => v.PlateNumber).IsRequired();
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(c => c.VehicleList)
                .HasForeignKey(v => v.ClientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Fee>().ToTable("Fee");
            modelBuilder.Entity<Fee>().HasKey(f => f.FeeID);
            modelBuilder.Entity<Fee>().Property(f => f.VehicleType).HasMaxLength(50);
            modelBuilder.Entity<Fee>().Property(f => f.FeePerHour).HasColumnType("DECIMAL(10,2)");

            // Configure VehicleSession entity
            modelBuilder.Entity<VehicleSession>().ToTable("VehicleSessions");
            modelBuilder.Entity<VehicleSession>().HasKey(vs => vs.SessionID);
            // This line is CORRECT for an INT IDENTITY column:
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.SessionID).ValueGeneratedOnAdd();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.VehicleID).HasColumnType("VARCHAR(50)").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.DurationType).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.StartTime).HasColumnType("TIME").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.StartDate).HasColumnType("DATE").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.EndDateTime).HasColumnType("DATETIME").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.TotalAmount).HasColumnType("DECIMAL(10,2)").IsRequired();

            // ADD CONFIGURATION FOR RegularParkingSession
            modelBuilder.Entity<RegularParkingSession>().ToTable("RegularParkingSessions"); // Match table name in DB
            modelBuilder.Entity<RegularParkingSession>().HasKey(rps => rps.SessionID);
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.SessionID).ValueGeneratedOnAdd(); // Auto-increment
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.RegularVehicleID).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.PlateNumber).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.VehicleType).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TimeIn).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TimeOut).IsRequired(false); // Nullable
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TotalAmount).HasColumnType("DECIMAL(10,2)").IsRequired(false); // Nullable
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.QRCodeData).HasColumnType("NVARCHAR(MAX)").IsRequired(false); // Nullable
        }
    }
}

