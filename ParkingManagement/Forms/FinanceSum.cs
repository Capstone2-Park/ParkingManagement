using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class FinanceSum : Form
    {
        private System.Windows.Forms.Timer _updateTimer;

        public FinanceSum()
        {
            InitializeComponent();
            // Set up a timer to update every 1 minute (or as needed)
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 60000; // 1 minute
            _updateTimer.Tick += async (s, e) =>
            {
                await UpdateDailyFinanceSumAsync();
                await UpdateWeeklyFinanceSumAsync();
                await UpdateMonthlyFinanceSumAsync();
            };
        }

        private async void FinanceSum_Load(object sender, EventArgs e)
        {
            await UpdateDailyFinanceSumAsync();
            await UpdateWeeklyFinanceSumAsync();
            await UpdateMonthlyFinanceSumAsync();
            _updateTimer.Start();
        }

        private async Task UpdateDailyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime today = DateTime.Today;

            // Count transactions
            int totalRegTransaction = await db.RegularParkingSessions.CountAsync(rps => rps.TimeIn.Date == today);
            int totalRentTransaction = await db.VehicleSessions.CountAsync(vs => vs.StartDate == today);
            int totalTransaction = totalRegTransaction + totalRentTransaction;

            // Calculate revenues
            decimal totalRegRevenue = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date == today)
                .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

            decimal totalRentRevenue = await db.VehicleSessions
                .Where(vs => vs.StartDate == today)
                .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

            decimal totalRevenue = totalRegRevenue + totalRentRevenue;

            // Find or create today's report
            var dailySum = await db.DailyFinanceSums.FirstOrDefaultAsync(d => d.ReportDateSum == today);
            if (dailySum == null)
            {
                dailySum = new DailyFinanceSum
                {
                    ReportDateSum = today
                };
                db.DailyFinanceSums.Add(dailySum);
            }

            // Update values
            dailySum.TotalTransaction = totalTransaction;
            dailySum.TotalRentTransaction = totalRentTransaction;
            dailySum.TotalRegTransaction = totalRegTransaction;
            dailySum.TotalRentRevenue = totalRentRevenue;
            dailySum.TotalRegRevenue = totalRegRevenue;
            dailySum.TotalRevenue = totalRevenue;

            await db.SaveChangesAsync();

            // Display all reports in DataGridView
            var allReports = await db.DailyFinanceSums.OrderByDescending(d => d.ReportDateSum).ToListAsync();
            dgvDailyRep.DataSource = allReports;
        }

        private async Task UpdateWeeklyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime today = DateTime.Today;

            // Calculate start (Monday) and end (Sunday) of the current week
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-1 * diff).Date;
            DateTime weekEnd = weekStart.AddDays(6).Date;

            // Count transactions
            int totalRegTransaction = await db.RegularParkingSessions
                .CountAsync(rps => rps.TimeIn.Date >= weekStart && rps.TimeIn.Date <= weekEnd);
            int totalRentTransaction = await db.VehicleSessions
                .CountAsync(vs => vs.StartDate >= weekStart && vs.StartDate <= weekEnd);
            int totalTransaction = totalRegTransaction + totalRentTransaction;

            // Calculate revenues
            decimal totalRegRevenue = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date >= weekStart && rps.TimeIn.Date <= weekEnd)
                .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

            decimal totalRentRevenue = await db.VehicleSessions
                .Where(vs => vs.StartDate >= weekStart && vs.StartDate <= weekEnd)
                .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

            decimal totalRevenue = totalRegRevenue + totalRentRevenue;

            // Find or create this week's report
            var weeklySum = await db.WeeklyFinanceSums
                .FirstOrDefaultAsync(w => w.WeekStart == weekStart && w.WeekEnd == weekEnd);
            if (weeklySum == null)
            {
                weeklySum = new WeeklyFinanceSum
                {
                    WeekStart = weekStart,
                    WeekEnd = weekEnd
                };
                db.WeeklyFinanceSums.Add(weeklySum);
            }

            // Update values
            weeklySum.TotalTransaction = totalTransaction;
            weeklySum.TotalRentTransaction = totalRentTransaction;
            weeklySum.TotalRegTransaction = totalRegTransaction;
            weeklySum.TotalRentRevenue = totalRentRevenue;
            weeklySum.TotalRegRevenue = totalRegRevenue;
            weeklySum.TotalRevenue = totalRevenue;

            await db.SaveChangesAsync();

            // Display all weekly reports in DataGridView
            var allWeeklyReports = await db.WeeklyFinanceSums.OrderByDescending(w => w.WeekStart).ToListAsync();
            dgvWeeklyRep.DataSource = allWeeklyReports;
        }

        private async Task UpdateMonthlyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime today = DateTime.Today;

            // Get first and last day of the current month
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // Count transactions
            int totalRegTransaction = await db.RegularParkingSessions
                .CountAsync(rps => rps.TimeIn.Date >= monthStart && rps.TimeIn.Date <= monthEnd);
            int totalRentTransaction = await db.VehicleSessions
                .CountAsync(vs => vs.StartDate >= monthStart && vs.StartDate <= monthEnd);
            int totalTransaction = totalRegTransaction + totalRentTransaction;

            // Calculate revenues
            decimal totalRegRevenue = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date >= monthStart && rps.TimeIn.Date <= monthEnd)
                .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

            decimal totalRentRevenue = await db.VehicleSessions
                .Where(vs => vs.StartDate >= monthStart && vs.StartDate <= monthEnd)
                .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

            decimal totalRevenue = totalRegRevenue + totalRentRevenue;

            // Find or create this month's report
            var monthlySum = await db.MonthlyFinanceSums
                .FirstOrDefaultAsync(m => m.MonthStart == monthStart && m.MonthEnd == monthEnd);
            if (monthlySum == null)
            {
                monthlySum = new MonthlyFinanceSum
                {
                    MonthStart = monthStart,
                    MonthEnd = monthEnd
                };
                db.MonthlyFinanceSums.Add(monthlySum);
            }

            // Update values
            monthlySum.TotalTransaction = totalTransaction;
            monthlySum.TotalRentTransaction = totalRentTransaction;
            monthlySum.TotalRegTransaction = totalRegTransaction;
            monthlySum.TotalRentRevenue = totalRentRevenue;
            monthlySum.TotalRegRevenue = totalRegRevenue;
            monthlySum.TotalRevenue = totalRevenue;

            await db.SaveChangesAsync();

            // Display all monthly reports in DataGridView
            var allMonthlyReports = await db.MonthlyFinanceSums.OrderByDescending(m => m.MonthStart).ToListAsync();
            dgvMonthlyRep.DataSource = allMonthlyReports;
        }
    }
}
