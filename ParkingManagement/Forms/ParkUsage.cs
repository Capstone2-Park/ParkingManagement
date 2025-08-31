using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingManagement.Models;
using Microsoft.EntityFrameworkCore;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace ParkingManagement.Forms
{
    public partial class ParkUsage : Form
    {
        public ParkUsage()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void ParkUsage_Load(object sender, EventArgs e)
        {
            // Optionally, initialize the chart here or on a button click
            DisplayParkUsageChart();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DisplayParkUsageChart();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DisplayParkUsageChart();
        }

        private void DisplayParkUsageChart()
        {
            pnlParkUsage.Controls.Clear();
            pnlParkUsage.AutoScroll = true;

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            // Prepare a list of all days in the range as string labels
            var dateLabels = new List<string>();
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
                dateLabels.Add(date.ToString("yyyy-MM-dd"));

            // Initialize counts dictionary with all dates in range
            var dailyCounts = dateLabels.ToDictionary(label => label, label => 0);

            using (var db = new ParkingDbContext())
            {
                var vehicleSessionGroups = db.VehicleSessions
                    .Where(v => v.StartDate >= fromDate && v.StartDate <= toDate)
                    .AsEnumerable()
                    .GroupBy(v => v.StartDate.Date.ToString("yyyy-MM-dd"))
                    .ToDictionary(g => g.Key, g => g.Count());

                var regularSessionGroups = db.RegularParkingSessions
                    .Where(r => r.TimeIn.Date >= fromDate && r.TimeIn.Date <= toDate)
                    .AsEnumerable()
                    .GroupBy(r => r.TimeIn.Date.ToString("yyyy-MM-dd"))
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var label in dateLabels)
                {
                    int count = 0;
                    if (vehicleSessionGroups.ContainsKey(label))
                        count += vehicleSessionGroups[label];
                    if (regularSessionGroups.ContainsKey(label))
                        count += regularSessionGroups[label];
                    dailyCounts[label] = count;
                }
            }

            var values = dailyCounts.Values.Select(c => (double)c).ToArray();
            var labels = dailyCounts.Keys.ToArray();

            var barSeries = new ColumnSeries<double>
            {
                Values = values,
                Name = "Total Parked Vehicles"
            };

            var cartesianChart = new CartesianChart
            {
                Series = new ISeries[] { barSeries },
                XAxes = new[]
                {
                    new Axis
                    {
                        Labels = labels,
                        Name = "Date",
                        LabelsRotation = 45
                    }
                },
                YAxes = new[] { new Axis { Name = "Count" } },
                Dock = DockStyle.None
            };

            int dayWidth = 120;
            int minWidth = 800;
            int chartWidth = Math.Max(minWidth, dayWidth * labels.Length);
            cartesianChart.Width = chartWidth;
            cartesianChart.Height = pnlParkUsage.Height;
            cartesianChart.Left = 0;
            cartesianChart.Top = 0;

            pnlParkUsage.Controls.Add(cartesianChart);

            MessageBox.Show($"From: {dtpFrom.Value:yyyy-MM-dd} To: {dtpTo.Value:yyyy-MM-dd}");
        }

        private void dtpFrom_ValueChanged_1(object sender, EventArgs e)
        {
            DisplayParkUsageChart();
        }

        private void dtpTo_ValueChanged_1(object sender, EventArgs e)
        {
            DisplayParkUsageChart();
        }

        private void btnPeakHours_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var reportManagement = new ReportManagement();
                homePage.ShowFormInPanel(reportManagement);
            }
        }

        private void btnAvailSlot_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var availableSlotsForm = new AvailableSlots();
                homePage.ShowFormInPanel(availableSlotsForm);
            }
        }
    }
}
