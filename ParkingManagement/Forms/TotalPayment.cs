using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
using ParkingManagement.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement.Forms
{
    public partial class TotalPayment : Form
    {
        public TotalPayment()
        {
            InitializeComponent();
            this.Load += TotalPayment_Load;
            btnSelect.Click += btnSelect_Click;

            // Set form properties for panel display
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // Set smaller font for rtbReceipt
            rtbReceipt.Font = new Font(rtbReceipt.Font.FontFamily, 8.0f);
        }

        private async void TotalPayment_Load(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();
            var clients = await db.Clients.ToListAsync();
            cbClient.DataSource = clients;
            cbClient.DisplayMember = "Name";
            cbClient.ValueMember = "ClientID";
            cbClient.SelectedIndex = -1;

            if (!clients.Any())
            {
                MessageBox.Show("No clients found in the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbClient.Enabled = false;
            }
        }

        private async void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                rtbReceipt.Clear();

                if (cbClient.SelectedItem is not Client selectedClient)
                {
                    MessageBox.Show("Please select a client.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var db = new ParkingDbContext();

                // Load vehicles with sessions in a single query
                var vehicles = await db.Vehicles
                    .Where(v => v.ClientID == selectedClient.ClientID)
                    .ToListAsync();

                StringBuilder receipt = new StringBuilder();
                receipt.AppendLine("Carbon Market Parking Receipt");
                receipt.AppendLine("=================");
                receipt.AppendLine($"Client Name: {selectedClient.Name}");
                receipt.AppendLine($"Contact Number: {selectedClient.CpNumber}");
                receipt.AppendLine($"Address: {selectedClient.Address}");
                receipt.AppendLine($"No. of Vehicles: {vehicles.Count}");
                receipt.AppendLine();
                receipt.AppendLine("LIST OF VEHICLES");
                receipt.AppendLine("===============");

                decimal totalFee = 0;
                foreach (var vehicle in vehicles)
                {
                    receipt.AppendLine($"Vehicle Details:");
                    receipt.AppendLine($"- Brand: {vehicle.Brand}");
                    receipt.AppendLine($"- Color: {vehicle.Color}");
                    receipt.AppendLine($"- Plate No.: {vehicle.PlateNumber}");
                    receipt.AppendLine($"- Type: {vehicle.VehicleType}");

                    // Get all sessions for this vehicle
                    var sessions = await db.VehicleSessions
                        .Where(s => s.VehicleID == vehicle.VehicleID)
                        .ToListAsync();

                    foreach (var session in sessions)
                    {
                        receipt.AppendLine($"  Time In : {session.StartDate:yyyy-MM-dd HH:mm}");
                        receipt.AppendLine($"  Time Out: {session.EndDateTime:yyyy-MM-dd HH:mm}");
                        receipt.AppendLine($"  Session Amount: ₱ {session.TotalAmount:N2}");
                        totalFee += session.TotalAmount;
                    }

                    if (sessions.Count == 0)
                    {
                        receipt.AppendLine("  No sessions found.");
                    }

                    receipt.AppendLine();
                }

                receipt.AppendLine("=================");
                receipt.AppendLine($"Total Payment: ₱ {totalFee:N2}");

                rtbReceipt.Text = receipt.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading client information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }
    }
}
