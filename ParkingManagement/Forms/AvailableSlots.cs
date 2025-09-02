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
    public partial class AvailableSlots : Form
    {
        public AvailableSlots()
        {
            InitializeComponent();
        }

        private void AvailableSlots_Load(object sender, EventArgs e)
        {
            
            DisplayAvailableSlotsChart();
        }



        private void DisplayAvailableSlotsChart()
        {
            pnlAvailSlot.Controls.Clear();
            pnlAvailSlot.AutoScroll = true;

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            var dateLabels = new List<string>();
            var dailyAvailableCounts = new List<double>();

            using (var db = new ParkingDbContext())
            {
                // Get all SlotHistory records in the range
                var histories = db.SlotHistories
                    .Where(h => h.Date >= fromDate && h.Date <= toDate)
                    .ToList();

                for (var date = fromDate; date <= toDate; date = date.AddDays(1))
                {
                    var history = histories.FirstOrDefault(h => h.Date.Date == date);
                    int totalAvailable = 0;

                    if (history != null)
                    {
                        // If record exists, update it if it's today
                        if (date == DateTime.Today)
                        {
                            // Get current slot counts from RegSlot and Parkingslot
                            int availableSlotM = db.RegularSlot.Sum(rs => rs.AvailableSlotM);
                            int availableSlotV = db.RegularSlot.Sum(rs => rs.AvailableSlotV);
                            int availableParkingSlots = db.Parkingslot.Count(ps => ps.SlotStatus == "Available");

                            history.AvailableSlotM = availableSlotM;
                            history.AvailableSlotV = availableSlotV;
                            history.AvailableParkingSlots = availableParkingSlots;
                            db.SaveChanges();

                            totalAvailable = availableSlotM + availableSlotV + availableParkingSlots;
                        }
                        else
                        {
                            // Use stored value for past dates
                            totalAvailable = history.AvailableSlotM + history.AvailableSlotV + history.AvailableParkingSlots;
                        }
                    }
                    else
                    {
                        // No record: show default value (e.g., 0), but do not save to DB
                        totalAvailable = 0;
                    }

                    dateLabels.Add(date.ToString("yyyy-MM-dd"));
                    dailyAvailableCounts.Add(totalAvailable);
                }
            }

            var barSeries = new ColumnSeries<double>
            {
                Values = dailyAvailableCounts.ToArray(),
                Name = "Available Slots"
            };

            var cartesianChart = new CartesianChart
            {
                Series = new ISeries[] { barSeries },
                XAxes = new[] { new Axis { Labels = dateLabels.ToArray(), Name = "Date", LabelsRotation = 45 } },
                YAxes = new[] { new Axis { Name = "Available Slots" } },
                Dock = DockStyle.None
            };

            // Set chart width based on number of days (e.g., 120px per day, min 800px)
            int dayWidth = 120;
            int minWidth = 800;
            int chartWidth = Math.Max(minWidth, dayWidth * dateLabels.Count);
            cartesianChart.Width = chartWidth;
            cartesianChart.Height = pnlAvailSlot.Height;
            cartesianChart.Left = 0;
            cartesianChart.Top = 0;

            pnlAvailSlot.Controls.Add(cartesianChart);
        }

        private void dtpFrom_ValueChanged_1(object sender, EventArgs e)
        {
            DisplayAvailableSlotsChart();

        }

        private void dtpTo_ValueChanged_1(object sender, EventArgs e)
        {
            DisplayAvailableSlotsChart();
        }
    }
}
