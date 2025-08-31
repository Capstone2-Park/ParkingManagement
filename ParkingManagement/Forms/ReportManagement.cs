using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingManagement.Models;
using Microsoft.EntityFrameworkCore;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace ParkingManagement.Forms
{
    public partial class ReportManagement : Form
    {
        public ReportManagement()
        {
            InitializeComponent();
            dtpFrom.ValueChanged += DateRangeChanged;
            dtpTo.ValueChanged += DateRangeChanged;
        }

        private async void ReportManagement_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            await LoadReportDataAsync();
        }

        private async void DateRangeChanged(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        private async Task LoadReportDataAsync()
        {
            using var db = new ParkingDbContext();
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            await ShowPeakHoursChart(from, to);
        }

        private async Task<Dictionary<string, int>> CalculatePeakHoursAsync(DateTime from, DateTime to)
        {
            using var db = new ParkingDbContext();

            // Get all sessions in range
            var vehicleSessions = await db.VehicleSessions
                .Where(vs => vs.StartDate >= from && vs.StartDate <= to && vs.EndDateTime != null)
                .Select(vs => vs.EndDateTime)
                .ToListAsync();

            var regularSessions = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date >= from && rps.TimeIn.Date <= to)
                .Select(rps => rps.TimeIn)
                .ToListAsync();

            // Combine all datetimes
            var allTimes = vehicleSessions.Concat(regularSessions);

            // Prepare all possible day-hour slots in the range
            var allSlots = new List<string>();
            for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    allSlots.Add($"{date:yyyy-MM-dd} {hour:D2}:00");
                }
            }

            // Count occurrences per slot
            var counts = allSlots.ToDictionary(
                slot => slot,
                slot =>
                {
                    var parts = slot.Split(' ');
                    var day = DateTime.Parse(parts[0]);
                    var hour = int.Parse(parts[1].Substring(0, 2));
                    return allTimes.Count(dt => dt.Date == day && dt.Hour == hour);
                });

            return counts;
        }

        private async Task ShowPeakHoursChart(DateTime from, DateTime to)
        {
            var peakHours = await CalculatePeakHoursAsync(from, to);

            var values = peakHours.OrderBy(kvp => kvp.Key).Select(kvp => (double)kvp.Value).ToArray();
            var hours = peakHours.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Key).ToArray();

            var barSeries = new ColumnSeries<double>
            {
                Values = values,
                Name = "Vehicles Parked"
            };

            var cartesianChart = new CartesianChart
            {
                Series = new ISeries[] { barSeries },
                XAxes = new[] { new Axis { Labels = hours, Name = "Date Hour", LabelsRotation = 90 } },
                YAxes = new[] { new Axis { Name = "Count" } }
            };

            // Set chart width based on number of slots (e.g., 60px per slot, min 800px)
            int slotWidth = 60;
            int minWidth = 800;
            int chartWidth = Math.Max(minWidth, slotWidth * hours.Length);
            cartesianChart.Width = chartWidth;
            cartesianChart.Height = panelChart.Height;

            // Enable horizontal scrolling on the panel
            panelChart.AutoScroll = true;
            panelChart.Controls.Clear();
            panelChart.Controls.Add(cartesianChart);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            // Save only the overall values
            var dailyReport = new DailyReport
            {
                ReportDateFrom = from,
                ReportDateTo = to,
                PeakHoursJson = JsonSerializer.Serialize(await CalculatePeakHoursAsync(from, to))
            };

            db.Set<DailyReport>().Add(dailyReport);
            await db.SaveChangesAsync();

            MessageBox.Show("Report saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnParkUse_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var parkUsageForm = new ParkUsage();
                homePage.ShowFormInPanel(parkUsageForm);
            }
        }

        private void btnAvailSlot_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var AvailableSlotsForm = new AvailableSlots();
                homePage.ShowFormInPanel(AvailableSlotsForm);
            }
        }
    }
}
