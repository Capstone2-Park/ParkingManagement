using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
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
        private ParkingDbContext _context = new ParkingDbContext();

        public TotalPayment()
        {
            InitializeComponent();
            this.Load += TotalPayment_Load;
            btnSelect.Click += btnSelect_Click;
        }

        private async void TotalPayment_Load(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();
            var clients = await db.Clients.ToListAsync();
            cbClient.DataSource = clients;
            cbClient.DisplayMember = "Name";
            cbClient.ValueMember = "ClientID";
            cbClient.SelectedIndex = -1;
        }

        private async void btnSelect_Click(object sender, EventArgs e)
        {
            lstClientInfo.Items.Clear();
            rtbTotal.Clear();

            if (cbClient.SelectedItem is not Client selectedClient)
            {
                MessageBox.Show("Please select a client.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new ParkingDbContext();

            // Show client info
            lstClientInfo.Items.Add($"Client Name: {selectedClient.Name}");
            lstClientInfo.Items.Add($"Contact No.: {selectedClient.CpNumber}");
            lstClientInfo.Items.Add($"Address: {selectedClient.Address}");

            // Get all vehicles for the client, ensuring uniqueness by VehicleID
            var vehicles = await db.Vehicles
                .Where(v => v.ClientID == selectedClient.ClientID)
                .GroupBy(v => v.VehicleID)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            lstClientInfo.Items.Add($"Total Vehicles: {vehicles.Count}");

            decimal totalFee = 0;
            var displayedVehicleIds = new HashSet<string>();

            foreach (var vehicle in vehicles)
            {
                if (vehicle == null || displayedVehicleIds.Contains(vehicle.VehicleID))
                    continue;

                lstClientInfo.Items.Add($"- Plate: {vehicle.PlateNumber}, Type: {vehicle.VehicleType}, Brand: {vehicle.Brand}, Color: {vehicle.Color}");
                displayedVehicleIds.Add(vehicle.VehicleID);

                // Sum all TotalAmount for this vehicle's sessions
                var vehicleTotal = await db.VehicleSessions
                    .Where(s => s.VehicleID == vehicle.VehicleID)
                    .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

                totalFee += vehicleTotal;
            }

            rtbTotal.Text = $"₱ {totalFee:N2}";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
