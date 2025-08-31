using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class MonthlySum : Form
    {
        private System.Windows.Forms.Timer _updateTimer;

        public MonthlySum()
        {
            InitializeComponent();
            // Set up a timer to update every 1 minute (or as needed)
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 60000; // 1 minute
            _updateTimer.Tick += async (s, e) =>
            {
                await UpdateMonthlyFinanceSumAsync();
            };

            dtpFrom.ValueChanged += dtpFrom_ValueChanged_1;
            dtpTo.ValueChanged += dtpTo_ValueChanged_1;
        }

        private async void MonthlySum_Load(object sender, EventArgs e)
        {
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.CustomFormat = "MMMM yyyy";
            dtpFrom.ShowUpDown = true;

            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.CustomFormat = "MMMM yyyy";
            dtpTo.ShowUpDown = true;

            await UpdateMonthlyFinanceSumAsync();
            _updateTimer.Start();
        }

        private async Task UpdateMonthlyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime fromDate = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, 1);
            DateTime toDate = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, 1);

            // Get all monthly summaries in the range
            var monthlySums = await db.MonthlyFinanceSums
                .Where(m => m.MonthStart >= fromDate && m.MonthStart <= toDate)
                .ToListAsync();

            var resultList = new List<MonthlyFinanceSum>();

            for (var month = fromDate; month <= toDate; month = month.AddMonths(1))
            {
                DateTime monthStart = new DateTime(month.Year, month.Month, 1);
                DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

                // Try to find an existing summary
                var monthlySum = await db.MonthlyFinanceSums
                    .FirstOrDefaultAsync(m => m.MonthStart == monthStart && m.MonthEnd == monthEnd);

                // Calculate values for this month
                int totalRegTransaction = await db.RegularParkingSessions
                    .CountAsync(rps => rps.TimeIn.Date >= monthStart && rps.TimeIn.Date <= monthEnd);
                int totalRentTransaction = await db.VehicleSessions
                    .CountAsync(vs => vs.StartDate >= monthStart && vs.StartDate <= monthEnd);
                int totalTransaction = totalRegTransaction + totalRentTransaction;

                decimal totalRegRevenue = await db.RegularParkingSessions
                    .Where(rps => rps.TimeIn.Date >= monthStart && rps.TimeIn.Date <= monthEnd)
                    .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

                decimal totalRentRevenue = await db.VehicleSessions
                    .Where(vs => vs.StartDate >= monthStart && vs.StartDate <= monthEnd)
                    .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

                decimal totalRevenue = totalRegRevenue + totalRentRevenue;

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
            }

            dgvMonthlyRep.DataSource = resultList.OrderByDescending(m => m.MonthStart).ToList();
        }



        private void DtpTo_ValueChanged(object sender, EventArgs e)
        {
            var firstOfMonth = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, 1);
            if (dtpTo.Value != firstOfMonth)
                dtpTo.Value = firstOfMonth;
            _ = UpdateMonthlyFinanceSumAsync();
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

        private void btnWeeklySums_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var weekly = new WeeklySum();
                homePage.ShowFormInPanel(weekly);
            }
        }

        private void dtpFrom_ValueChanged_1(object sender, EventArgs e)
        {
            var firstOfMonth = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, 1);
            if (dtpFrom.Value != firstOfMonth)
                dtpFrom.Value = firstOfMonth;
            _ = UpdateMonthlyFinanceSumAsync();
        }

        private void dtpTo_ValueChanged_1(object sender, EventArgs e)
        {
            var firstOfMonth = new DateTime(dtpTo.Value.Year, dtpTo.Value.Month, 1);
            if (dtpTo.Value != firstOfMonth)
                dtpTo.Value = firstOfMonth;
            _ = UpdateMonthlyFinanceSumAsync();
        }
    }
}
