using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class WeeklySum : Form
    {
        private System.Windows.Forms.Timer _updateTimer;
        private bool _suppressEvents = false;

        public WeeklySum()
        {
            InitializeComponent();
            // Set up a timer to update every 1 minute (or as needed)
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 60000; // 1 minute
            _updateTimer.Tick += async (s, e) =>
            {
                await UpdateWeeklyFinanceSumAsync();
            };
            dtpFrom.ValueChanged += dtpFrom_ValueChanged_1;
            dtpTo.ValueChanged += dtpTo_ValueChanged_1;
        }


        private async void WeeklySum_Load(object sender, EventArgs e)
        {
            await UpdateWeeklyFinanceSumAsync();
            _updateTimer.Start();
        }

        private async Task UpdateWeeklyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            var resultList = new List<WeeklyFinanceSum>();
            DateTime weekStart = fromDate.StartOfWeek(DayOfWeek.Monday);

            while (weekStart <= toDate)
            {
                DateTime weekEnd = weekStart.AddDays(6);
                if (weekEnd > toDate) weekEnd = toDate;

                // Try to find an existing summary
                var weeklySum = await db.WeeklyFinanceSums
                    .FirstOrDefaultAsync(w => w.WeekStart == weekStart && w.WeekEnd == weekEnd);

                // Calculate values for this week
                int totalRegTransaction = await db.RegularParkingSessions
                    .CountAsync(rps => rps.TimeIn.Date >= weekStart && rps.TimeIn.Date <= weekEnd);
                int totalRentTransaction = await db.VehicleSessions
                    .CountAsync(vs => vs.StartDate >= weekStart && vs.StartDate <= weekEnd);
                int totalTransaction = totalRegTransaction + totalRentTransaction;

                decimal totalRegRevenue = await db.RegularParkingSessions
                    .Where(rps => rps.TimeIn.Date >= weekStart && rps.TimeIn.Date <= weekEnd)
                    .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

                decimal totalRentRevenue = await db.VehicleSessions
                    .Where(vs => vs.StartDate >= weekStart && vs.StartDate <= weekEnd)
                    .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

                decimal totalRevenue = totalRegRevenue + totalRentRevenue;

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

                resultList.Add(weeklySum);
                weekStart = weekStart.AddDays(7);
            }

            dgvWeeklyRep.DataSource = resultList.OrderByDescending(w => w.WeekStart).ToList();
        }

        private void btnDailysums_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var daily = new FinanceSum();
                homePage.ShowFormInPanel(daily);
            }
        }

        private void btnMonthlySums_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var monthly = new MonthlySum();
                homePage.ShowFormInPanel(monthly);
            }
        }

        private void dtpFrom_ValueChanged_1(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            _suppressEvents = true;

            // Snap to Monday
            var monday = dtpFrom.Value.StartOfWeek(DayOfWeek.Monday);
            if (dtpFrom.Value != monday)
                dtpFrom.Value = monday;

            // Ensure dtpTo is at least the same week
            var sunday = monday.AddDays(6);
            if (dtpTo.Value < monday || dtpTo.Value > sunday)
                dtpTo.Value = sunday;

            _suppressEvents = false;
            _ = UpdateWeeklyFinanceSumAsync();
        }

        private void dtpTo_ValueChanged_1(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            _suppressEvents = true;

            // Snap to Sunday
            var sunday = dtpTo.Value.StartOfWeek(DayOfWeek.Monday).AddDays(6);
            if (dtpTo.Value != sunday)
                dtpTo.Value = sunday;

            // Ensure dtpFrom is at most the same week
            var monday = sunday.AddDays(-6);
            if (dtpFrom.Value > sunday || dtpFrom.Value < monday)
                dtpFrom.Value = monday;

            _suppressEvents = false;
            _ = UpdateWeeklyFinanceSumAsync();
        }
    }

    // Helper extension for week calculation
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
