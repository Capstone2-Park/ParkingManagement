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
            var latestClient = await db.Clients
                .OrderByDescending(c => c.ClientID)
                .FirstOrDefaultAsync();

            if (latestClient == null)
            {
                MessageBox.Show("No clients found in the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbReceipt.Clear();
                return;
            }

            await DisplayReceiptForClient(latestClient);
        }

        private async Task DisplayReceiptForClient(Client selectedClient)
        {
            try
            {
                rtbReceipt.Clear();

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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var clientManagementForm = new ClientManagement();
                homePage.ShowFormInPanel(clientManagementForm);
            }
        }
    }
}
