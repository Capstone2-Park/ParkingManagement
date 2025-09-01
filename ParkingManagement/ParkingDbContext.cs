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
        public DbSet<RegularParkingSession> RegularParkingSessions { get; set; }
        public DbSet<Parkingslot> Parkingslot { get; set; }
        public DbSet<RegularParkingTotals> RegularParkingTotal { get; set; }

        public DbSet<RegularSlot> RegularSlot { get; set; }

        public DbSet<DailyReport> DailyReport { get; set; }
        public DbSet<DailyFinanceSum> DailyFinanceSums { get; set; }
        public DbSet<WeeklyFinanceSum> WeeklyFinanceSums { get; set; }
        public DbSet<MonthlyFinanceSum> MonthlyFinanceSums { get; set; }
        public DbSet<SlotHistory> SlotHistories { get; set; }

        public DbSet<TransactionsRent> TransactionsRent { get; set; }
        public DbSet<TransactionsReg> TransactionsReg { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=CECILE030103\\SQLEXPRESS;Initial Catalog=ParkingSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                sqlOptions => sqlOptions.EnableRetryOnFailure()
            );
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
            modelBuilder.Entity<Fee>().Property(f => f.FixedPrice).HasColumnType("DECIMAL(10,2)");

            // Configure VehicleSession entity to match the new table structure
            modelBuilder.Entity<VehicleSession>().ToTable("VehicleSessions");
            modelBuilder.Entity<VehicleSession>().HasKey(vs => vs.SessionID);
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.SessionID).ValueGeneratedOnAdd();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.VehicleID).HasColumnType("VARCHAR(50)").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.DurationType).HasMaxLength(50).IsRequired();
            // Removed StartTime property mapping
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.StartDate).HasColumnType("DATE").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.EndDateTime).HasColumnType("DATETIME").IsRequired();
            modelBuilder.Entity<VehicleSession>().Property(vs => vs.TotalAmount).HasColumnType("DECIMAL(10,2)").IsRequired();

            // RegularParkingSession configuration
            modelBuilder.Entity<RegularParkingSession>().ToTable("RegularParkingSessions");
            modelBuilder.Entity<RegularParkingSession>().HasKey(rps => rps.SessionID);
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.SessionID).ValueGeneratedOnAdd();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.RegularVehicleID).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.VehicleType).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TimeIn).IsRequired();
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TimeOut).IsRequired(false);
            modelBuilder.Entity<RegularParkingSession>().Property(rps => rps.TotalAmount).HasColumnType("DECIMAL(10,2)").IsRequired(false);
          

            modelBuilder.Entity<Parkingslot>().ToTable("Parkingslot");
            modelBuilder.Entity<Parkingslot>().HasKey(ps => ps.SlotID);

            modelBuilder.Entity<Parkingslot>().Property(ps => ps.VehicleStatus).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Parkingslot>().Property(ps => ps.VehicleID).HasMaxLength(10).IsRequired(false);
            modelBuilder.Entity<Parkingslot>().Property(ps => ps.ClientID).HasMaxLength(10).IsRequired(false);
            modelBuilder.Entity<Parkingslot>().Property(ps => ps.SlotNumber).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Parkingslot>().Property(ps => ps.SlotStatus).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Parkingslot>()
                .HasOne(ps => ps.Vehicle)
                .WithMany()
                .HasForeignKey(ps => ps.VehicleID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Parkingslot>()
                .HasOne(ps => ps.Client)
                .WithMany()
                .HasForeignKey(ps => ps.ClientID)
                .OnDelete(DeleteBehavior.SetNull);





            modelBuilder.Entity<RegularParkingTotals>().ToTable("RegularParkingTotals");
            modelBuilder.Entity<RegularParkingTotals>().HasKey(rpt => rpt.TotalID);

            modelBuilder.Entity<RegularSlot>().ToTable("RegularSlot");
            modelBuilder.Entity<RegularSlot>().HasKey(rs => rs.RegSlotID);

            modelBuilder.Entity<DailyFinanceSum>().ToTable("DailyFinanceSum");
            modelBuilder.Entity<DailyFinanceSum>().HasKey(dfs => dfs.DailyFinanceID);

            modelBuilder.Entity<WeeklyFinanceSum>().ToTable("WeeklyFinanceSum");
            modelBuilder.Entity<WeeklyFinanceSum>().HasKey(wfs => wfs.WeeklyFinanceID);

            modelBuilder.Entity<MonthlyFinanceSum>().ToTable("MonthlyFinanceSum");
            modelBuilder.Entity<MonthlyFinanceSum>().HasKey(mfs => mfs.MonthlyFinanceID);

            modelBuilder.Entity<SlotHistory>().ToTable("SlotHistory");
            modelBuilder.Entity<SlotHistory>().HasKey(mfs => mfs.SlotHistoryID);

            modelBuilder.Entity<TransactionsRent>().ToTable("TransactionsRent");
            modelBuilder.Entity<TransactionsRent>().HasKey(tr => tr.TransactionID);

            modelBuilder.Entity<TransactionsReg>().ToTable("TransactionsReg");
            modelBuilder.Entity<TransactionsReg>().HasKey(trr => trr.TransactionID);
        }
    }
}

