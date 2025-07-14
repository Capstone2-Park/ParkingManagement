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
                lstClientInfo.Items.Clear();
                rtbTotal.Clear();

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

                // Display client information only once at the top
                lstClientInfo.Items.Add($"Client Name: {selectedClient.Name}");
                lstClientInfo.Items.Add($"Contact Number: {selectedClient.CpNumber}");
                lstClientInfo.Items.Add($"Address: {selectedClient.Address}");
                lstClientInfo.Items.Add($"Number of Vehicles: {vehicles.Count}");
                lstClientInfo.Items.Add("");
                lstClientInfo.Items.Add("List of Vehicles:");

                decimal totalFee = 0;
                foreach (var vehicle in vehicles)
                {
                    // Format each vehicle on a single line with consistent spacing
                    lstClientInfo.Items.Add($"- Plate: {vehicle.PlateNumber}, Type: {vehicle.VehicleType}, Brand: {vehicle.Brand}, Color: {vehicle.Color}");

                    var sessionTotal = await db.VehicleSessions
                        .Where(s => s.VehicleID == vehicle.VehicleID)
                        .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

                    totalFee += sessionTotal;
                }

                rtbTotal.Text = $"₱ {totalFee:N2}";
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
