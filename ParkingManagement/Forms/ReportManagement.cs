using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingManagement.Models; // Add this for model access
using Microsoft.EntityFrameworkCore; // For async/EF methods
using System.Text.Json; // For JSON serialization
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
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private async void ReportManagement_Load(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();
            DateTime today = DateTime.Today;

            // Count VehicleSessions that started today
            int vehicleSessionCount = await db.VehicleSessions
                .CountAsync(vs => vs.StartDate == today);

            // Count RegularParkingSessions that started today
            int regularParkingSessionCount = await db.RegularParkingSessions
                .CountAsync(rps => rps.TimeIn.Date == today);

            int totalParked = vehicleSessionCount + regularParkingSessionCount;

            string parkUseText = $"Total vehicles parked today: {totalParked}\n" +
                                 $"- Vehicle Sessions: {vehicleSessionCount}\n" +
                                 $"- Regular Parking Sessions: {regularParkingSessionCount}";

            rtbParkUse.Text = parkUseText;

            // --- Available Slots Section ---
            var regSlot = await db.Set<RegularSlot>().FirstOrDefaultAsync();
            int availableSlotM = regSlot?.AvailableSlotM ?? 0;
            int availableSlotV = regSlot?.AvailableSlotV ?? 0;

            int availableParkingSlots = await db.Parkingslot
                .CountAsync(ps => ps.SlotStatus == "Available");

            int totalAvailable = availableSlotM + availableSlotV + availableParkingSlots;

            string availSlotText = $"Available Regular Slots:\n" +
                                 $"- Motorcycle (2-Wheels): {availableSlotM}\n" +
                                 $"- Vehicle (4-Wheels): {availableSlotV}\n" +
                                 $"Other Available Parking Slots: {availableParkingSlots}\n" +
                                 $"-----------------------------\n" +
                                 $"Total Available Slots: {totalAvailable}";

            rtbAvailSlot.Text = availSlotText;

            // --- Total Revenue Section ---
            decimal vehicleSessionRevenue = await db.VehicleSessions
                .Where(vs => vs.StartDate == today)
                .SumAsync(vs => (decimal?)vs.TotalAmount ?? 0);

            decimal regularParkingSessionRevenue = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date == today && rps.TotalAmount != null)
                .SumAsync(rps => (decimal?)rps.TotalAmount ?? 0);

            decimal totalRevenue = vehicleSessionRevenue + regularParkingSessionRevenue;

            // Only display the overall total revenue, large and bold
            rtbTotalRevenue.Text = $"₱{totalRevenue:N2}";
            rtbTotalRevenue.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            rtbTotalRevenue.ReadOnly = true;

            // Set rtbParkUse and rtbAvailSlot as read-only
            rtbParkUse.ReadOnly = true;
            rtbAvailSlot.ReadOnly = true;

            // --- Save to DailyReport ---
            var dailyReport = await db.Set<DailyReport>()
                .FirstOrDefaultAsync(dr => dr.ReportDate == today);

            if (dailyReport == null)
            {
                dailyReport = new DailyReport
                {
                    ReportDate = today,
                    ParkingUsage = parkUseText,
                    AvailableSlots = availSlotText,
                    TotalRevenue = Math.Round(totalRevenue, 2)
                };
                db.Set<DailyReport>().Add(dailyReport);
            }
            else
            {
                dailyReport.ParkingUsage = parkUseText;
                dailyReport.AvailableSlots = availSlotText;
                dailyReport.TotalRevenue = Math.Round(totalRevenue, 2);
            }

            var peakHours = await CalculatePeakHoursAsync();
            string peakHoursJson = JsonSerializer.Serialize(peakHours);

            // Save to DailyReport
            dailyReport.PeakHoursJson = peakHoursJson;

            await db.SaveChangesAsync();

            ShowPeakHoursChart(); // Make sure this is called
        }

        private void rtbParkUse_TextChanged(object sender, EventArgs e)
        {

        }

        private async Task<Dictionary<int, int>> CalculatePeakHoursAsync()
        {
            using var db = new ParkingDbContext();
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            // Get all relevant times for today
            var vehicleSessionHours = await db.VehicleSessions
                .Where(vs => vs.StartDate == today)
                .Select(vs => vs.EndDateTime.Hour)
                .ToListAsync();

            var regularParkingSessionHours = await db.RegularParkingSessions
                .Where(rps => rps.TimeIn.Date == today)
                .Select(rps => rps.TimeIn.Hour)
                .ToListAsync();

            // Combine and count per hour
            var allHours = vehicleSessionHours.Concat(regularParkingSessionHours);

            var hourlyCounts = Enumerable.Range(0, 24)
                .ToDictionary(
                    hour => hour,
                    hour => allHours.Count(h => h == hour)
                );

            return hourlyCounts;
        }

        private async void ShowPeakHoursChart()
        {
            var peakHours = await CalculatePeakHoursAsync();

            var values = peakHours.OrderBy(kvp => kvp.Key).Select(kvp => (double)kvp.Value).ToArray();
            var hours = peakHours.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Key.ToString("D2") + ":00").ToArray();

            var lineSeries = new LineSeries<double>
            {
                Values = values,
                Name = "Vehicles Parked"
            };

            var cartesianChart = new CartesianChart
            {
                Series = new ISeries[] { lineSeries },
                XAxes = new[] { new Axis { Labels = hours, Name = "Hour" } },
                YAxes = new[] { new Axis { Name = "Count" } }
            };

            cartesianChart.Dock = DockStyle.Fill;
            panelChart.Controls.Clear();
            panelChart.Controls.Add(cartesianChart);
        }

        private void rtbAvailSlot_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
