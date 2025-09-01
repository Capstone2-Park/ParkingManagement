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
            };

            // Make DataGridViews read-only
            dgvDailyRep.ReadOnly = true;
        }

        private async void FinanceSum_Load(object sender, EventArgs e)
        {
            await UpdateDailyFinanceSumAsync();
            _updateTimer.Start();
        }

        private async Task UpdateDailyFinanceSumAsync()
        {
            using var db = new ParkingDbContext();
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            var resultList = new List<DailyFinanceSum>();

            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                // Calculate counts and sums for VehicleSession (Rent)
                var rentSessions = await db.VehicleSessions
                    .Where(vs => vs.StartDate.Date == date)
                    .ToListAsync();
                int totalRentTransaction = rentSessions.Count;
                decimal totalRentRevenue = rentSessions.Sum(vs => vs.TotalAmount);

                // Calculate counts for RegularParkingSession
                var regSessions = await db.RegularParkingSessions
                    .Where(rps => rps.TimeIn.Date == date)
                    .ToListAsync();
                int totalRegTransaction = regSessions.Count;

                // Calculate revenue for RegularParkingTotal
                var regTotals = await db.RegularParkingTotal
                    .Where(rpt => rpt.TimeIn.Date == date)
                    .ToListAsync();
                decimal totalRegRevenue = regTotals.Sum(rpt => rpt.TotalAmount ?? 0);

                // Total transactions and revenue
                int totalTransaction = totalRentTransaction + totalRegTransaction;
                decimal totalRevenue = totalRentRevenue + totalRegRevenue;

                // Try to get existing DailyFinanceSum
                var dailySum = await db.DailyFinanceSums.FirstOrDefaultAsync(d => d.ReportDateSum == date);

                if (dailySum == null)
                {
                    dailySum = new DailyFinanceSum
                    {
                        ReportDateSum = date,
                        TotalTransaction = totalTransaction,
                        TotalRentTransaction = totalRentTransaction,
                        TotalRegTransaction = totalRegTransaction,
                        TotalRentRevenue = totalRentRevenue,
                        TotalRegRevenue = totalRegRevenue,
                        TotalRevenue = totalRevenue
                    };
                    db.DailyFinanceSums.Add(dailySum);
                }
                else
                {
                    dailySum.TotalTransaction = totalTransaction;
                    dailySum.TotalRentTransaction = totalRentTransaction;
                    dailySum.TotalRegTransaction = totalRegTransaction;
                    dailySum.TotalRentRevenue = totalRentRevenue;
                    dailySum.TotalRegRevenue = totalRegRevenue;
                    dailySum.TotalRevenue = totalRevenue;
                    db.DailyFinanceSums.Update(dailySum);
                }

                resultList.Add(dailySum);
            }

            await db.SaveChangesAsync();

            // Sort by date descending (latest first)
            dgvDailyRep.DataSource = resultList.OrderByDescending(d => d.ReportDateSum).ToList();
        }


        private async void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            await UpdateDailyFinanceSumAsync();
        }

        private async void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            await UpdateDailyFinanceSumAsync();
        }

        private void btnWeeklysums_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var weekly = new WeeklySum();
                homePage.ShowFormInPanel(weekly);
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
    }
}
