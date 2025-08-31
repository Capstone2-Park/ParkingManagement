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

            var dailySums = await db.DailyFinanceSums
                .Where(d => d.ReportDateSum >= fromDate && d.ReportDateSum <= toDate)
                .ToListAsync();

            var resultList = new List<DailyFinanceSum>();

            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var dailySum = dailySums.FirstOrDefault(d => d.ReportDateSum == date);
                if (dailySum == null)
                {
                    // If no record, create a zeroed summary for this date
                    dailySum = new DailyFinanceSum
                    {
                        ReportDateSum = date,
                        TotalTransaction = 0,
                        TotalRentTransaction = 0,
                        TotalRegTransaction = 0,
                        TotalRentRevenue = 0,
                        TotalRegRevenue = 0,
                        TotalRevenue = 0
                    };
                }
                resultList.Add(dailySum);
            }

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
