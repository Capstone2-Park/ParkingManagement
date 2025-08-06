using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;

namespace ParkingManagement.Forms
{
    public partial class ParkingSlot : Form
    {
        private List<Client> clients;
        private List<Vehicle> vehicles;
        private List<Parkingslot> slots;

        // Add this property to hold the current client
        private Client currentClient;

        // Add this property to hold the current vehicle being parked
        private Vehicle currentVehicle;

        // New property to keep track of the currently selected slot
        private Parkingslot currentSlot;

        // Add this field to your class
        private bool isInitializing = false;

        public ParkingSlot()
        {
            InitializeComponent();

            using (var db = new ParkingDbContext())
            {
                // Defensive: Only select clients with non-null ClientID
                currentClient = db.Clients
                    .Where(c => c.ClientID != null)
                    .OrderByDescending(c => c.ClientID)
                    .Include(c => c.VehicleList)
                    .FirstOrDefault();

                if (currentClient != null)
                {
                    vehicles = db.Vehicles
                        .Where(v => v.ClientID == currentClient.ClientID && v.VehicleID != null)
                        .ToList();
                }
                else
                {
                    vehicles = new List<Vehicle>();
                }
            }

            Load += ParkingSlot_Load;
            cbVehicle.SelectedIndexChanged += cbVehicle_SelectedIndexChanged;
           
        }

        public ParkingSlot(Client client)
        {
            InitializeComponent();
            currentClient = client;
            Load += ParkingSlot_Load;
        }

        // In ParkingSlot_Load, use the flag to suppress unwanted events
        private void ParkingSlot_Load(object sender, EventArgs e)
        {
            isInitializing = true; // Suppress event

            if (currentClient == null)
            {
                MessageBox.Show("Current client is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isInitializing = false;
                return;
            }

            using (var db = new ParkingDbContext())
            {
                clients = db.Clients
                    .Where(c => c.ClientID != null)
                    .Include(c => c.VehicleList)
                    .ToList();
                slots = db.Parkingslot.ToList();
            }

            lblCname.Text = currentClient.Name ?? "(No Name)";

            cbVehicle.DataSource = null;
            cbVehicle.DataSource = vehicles ?? new List<Vehicle>();
            cbVehicle.DisplayMember = "PlateNumber";
            cbVehicle.ValueMember = "VehicleID";
            cbVehicle.SelectedIndex = -1;
            cbVehicle.Text = "Select Vehicle";

            foreach (var slot in slots)
            {
                var panel = this.Controls.Find("pnl" + slot.SlotNumber, true).FirstOrDefault() as Panel;
                if (panel != null)
                {
                    panel.Click -= SlotPanel_Click;
                    panel.Click += SlotPanel_Click;

                    panel.MouseEnter -= SlotPanel_MouseEnter;
                    panel.MouseEnter += SlotPanel_MouseEnter;
                    panel.MouseLeave -= SlotPanel_MouseLeave;
                    panel.MouseLeave += SlotPanel_MouseLeave;
                }
            }

            currentVehicle = null;

            UpdateSlotPanelColors();
            UpdateSlotStatusLabels();

            isInitializing = false; // Allow event
        }

        // The issue arises because there are duplicate event handler methods with the same name and signature.
        // To resolve the ambiguity, remove the duplicate method definition.

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle first.");
                return;
            }

            currentVehicle = selectedVehicle;

            // Restrict slot panels based on vehicle type
            SetSlotPanelRestrictions(currentVehicle.VehicleType?.Trim() ?? "");

            MessageBox.Show($"Vehicle {currentVehicle.PlateNumber} selected. Now click a slot to park.", "Vehicle Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Panel click event handler
        private async void SlotPanel_Click(object sender, EventArgs e)
        {
            if (currentVehicle == null)
            {
                MessageBox.Show("No vehicle selected for parking.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var panel = sender as Panel;
            if (panel == null) return;

            string slotNumber = panel.Name.Replace("pnl", "");
            var slot = slots.FirstOrDefault(s => s.SlotNumber == slotNumber);

            if (slot == null)
            {
                MessageBox.Show("Slot not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (slot.SlotStatus == "occupied")
            {
                MessageBox.Show("Slot is already occupied.", "Occupied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Vehicle type/slot type validation
            bool isVehicle2W = currentVehicle.VehicleType.Trim().Equals("2-Wheels", StringComparison.OrdinalIgnoreCase);
            bool isVehicle4W = currentVehicle.VehicleType.Trim().Equals("4-Wheels", StringComparison.OrdinalIgnoreCase);

            if (isVehicle2W && slotNumber.StartsWith("V"))
            {
                MessageBox.Show("2-Wheels vehicles can only park in M slots.", "Invalid Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isVehicle4W && slotNumber.StartsWith("M"))
            {
                MessageBox.Show("4-Wheels vehicles can only park in V slots.", "Invalid Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm parking
            var result = MessageBox.Show($"Do you want to park {currentVehicle.PlateNumber} in slot {slotNumber}?", "Confirm Parking", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) return;

            // Save to database
            using (var db = new ParkingDbContext())
            {
                var dbSlot = await db.Parkingslot.FirstOrDefaultAsync(s => s.SlotNumber == slotNumber);
                if (dbSlot != null)
                {
                    dbSlot.SlotStatus = "occupied";
                    dbSlot.VehicleStatus = "parked";
                    dbSlot.VehicleID = currentVehicle.VehicleID;
                    dbSlot.ClientID = currentClient.ClientID;
                    db.Parkingslot.Update(dbSlot);
                    await db.SaveChangesAsync();
                }
            }

            // Update local slot and UI
            slot.SlotStatus = "occupied";
            var label = this.Controls.Find("lblStat" + slotNumber, true).FirstOrDefault() as Label;
            if (label != null) label.Text = "Rented";
            panel.BackColor = Color.Red;

            MessageBox.Show("Vehicle parked successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Remove the parked vehicle from the list and update cbVehicle
            vehicles.Remove(currentVehicle);
            cbVehicle.DataSource = null;
            cbVehicle.DataSource = vehicles;
            cbVehicle.DisplayMember = "PlateNumber";
            cbVehicle.ValueMember = "VehicleID";
            cbVehicle.SelectedIndex = -1;

            // Restore all slot panels to normal state
            RestoreAllSlotPanels();

            currentVehicle = null;

            // If all vehicles are parked, navigate to ParkRental
            if (vehicles.Count == 0)
            {
                var homePage = this.ParentForm as HomePage;
                if (homePage != null)
                {
                    var rentForm = new ParkRental();
                    homePage.ShowFormInPanel(rentForm);
                }
            }
        }

        private void UpdateSlotPanelColors()
        {
            foreach (var slot in slots)
            {
                var panel = this.Controls.Find("pnl" + slot.SlotNumber, true).FirstOrDefault() as Panel;
                if (panel != null)
                {
                    if (!string.IsNullOrEmpty(slot.ClientID))
                    {
                        panel.BackColor = Color.Red; // Rented
                    }
                    else if (slot.SessionID != null)
                    {
                        panel.BackColor = Color.Red; // Occupied
                    }
                    else
                    {
                        panel.BackColor = Color.Green; // Available
                    }
                }
            }
        }

        private void UpdateSlotStatusLabels()
        {
            foreach (var slot in slots)
            {
                var label = this.Controls.Find("lblStat" + slot.SlotNumber, true).FirstOrDefault() as Label;
                if (label != null)
                {
                    if (!string.IsNullOrEmpty(slot.ClientID))
                    {
                        label.Text = "Rented";
                    }
                    else if (slot.SessionID != null)
                    {
                        label.Text = "Occupied";
                    }
                    else
                    {
                        label.Text = "Available";
                    }
                }
            }
        }

        // Update cbVehicle_SelectedIndexChanged to check the flag
        private void cbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null)
            {
                currentVehicle = null;
                RestoreAllSlotPanels();
                return;
            }

            currentVehicle = selectedVehicle;
            SetSlotPanelRestrictions(currentVehicle.VehicleType?.Trim() ?? "");
            MessageBox.Show($"{currentVehicle.PlateNumber} has been selected", "Vehicle Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //private void cbSlot_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedSlot = (sender == cbSlotV) ? cbSlotV.SelectedItem?.ToString() : cbSlotM.Enabled ? cbSlotM.SelectedItem?.ToString() : null;
        //    if (string.IsNullOrEmpty(selectedSlot)) return;

        //    var slot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
        //    var panel = this.Controls.Find("panel" + selectedSlot, true).FirstOrDefault() as Panel;
        //    if (panel != null)
        //    {
        //        panel.BackColor = (slot != null && slot.SlotStatus == "occupied") ? Color.Red : Color.Green;
        //    }
        //}







        private void label67_Click(object sender, EventArgs e)
        {

        }

        private void label85_Click(object sender, EventArgs e)
        {

        }


        private void btnPrev_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var clientManagementForm = new ClientManagement();
                homePage.ShowFormInPanel(clientManagementForm);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Do you want to cancel the progress?",
                "Cancel Progress",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK)
                return;

            using (var db = new ParkingDbContext())
            {
                // Get the latest client (by highest ClientID)
                var latestClient = db.Clients
                    .OrderByDescending(c => c.ClientID)
                    .Include(c => c.VehicleList)
                    .FirstOrDefault();

                if (latestClient != null)
                {
                    // Get all vehicles for this client
                    var vehicles = db.Vehicles
                        .Where(v => v.ClientID == latestClient.ClientID)
                        .ToList();

                    var vehicleIds = vehicles.Select(v => v.VehicleID).ToList();

                    // For each slot where one of the client's vehicles is parked, reset statuses
                    var slotsToUpdate = db.Parkingslot
                        .Where(s => vehicleIds.Contains(s.VehicleID))
                        .ToList();

                    foreach (var slot in slotsToUpdate)
                    {
                        slot.VehicleStatus = "Not Parked";
                        slot.SlotStatus = "Available";
                        // Do NOT delete SlotNumber
                        slot.VehicleID = null;
                        slot.ClientID = null;
                        db.Parkingslot.Update(slot);
                    }

                    // Remove vehicles
                    if (vehicles.Any())
                    {
                        db.Vehicles.RemoveRange(vehicles);
                    }

                    // Remove client
                    db.Clients.Remove(latestClient);

                    await db.SaveChangesAsync();
                }
            }

            // Navigate to ClientManagement
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var clientManagementForm = new ClientManagement();
                homePage.ShowFormInPanel(clientManagementForm);
            }
            else
            {
                this.Close();
            }
        }

      
        private void SlotPanel_MouseEnter(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel != null)
            {
                // Set to semi-transparent white
                panel.BackColor = Color.FromArgb(128, Color.White);
            }
        }

        private void SlotPanel_MouseLeave(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel != null)
            {
                // Restore color based on slot status
                string slotNumber = panel.Name.Replace("pnl", "");
                var slot = slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
                if (slot != null)
                {
                    if (!string.IsNullOrEmpty(slot.ClientID))
                    {
                        panel.BackColor = Color.Red; // Rented
                    }
                    else if (slot.SessionID != null)
                    {
                        panel.BackColor = Color.Red; // Occupied
                    }
                    else
                    {
                        panel.BackColor = Color.Green; // Available
                    }
                }
                else
                {
                    panel.BackColor = Color.Green;
                }
            }
        }

        private void pnlV6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSlot_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SetSlotPanelRestrictions(string vehicleType)
        {
            // 2-Wheels: disable V slots, 4-Wheels: disable M slots
            for (int i = 1; i <= 24; i++)
            {
                string vPanelName = $"pnlV{i}";
                string mPanelName = $"pnlM{i}";

                var vPanel = this.Controls.Find(vPanelName, true).FirstOrDefault() as Panel;
                var mPanel = this.Controls.Find(mPanelName, true).FirstOrDefault() as Panel;

                if (vehicleType.Equals("2-Wheels", StringComparison.OrdinalIgnoreCase))
                {
                    if (vPanel != null)
                    {
                        vPanel.Enabled = false;
                        vPanel.BackColor = Color.Gray;
                    }
                    if (mPanel != null)
                    {
                        mPanel.Enabled = true;
                        // Restore color based on slot status
                        var slot = slots.FirstOrDefault(s => s.SlotNumber == $"M{i}");
                        if (slot != null)
                            mPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                    }
                }
                else if (vehicleType.Equals("4-Wheels", StringComparison.OrdinalIgnoreCase))
                {
                    if (mPanel != null)
                    {
                        mPanel.Enabled = false;
                        mPanel.BackColor = Color.Gray;
                    }
                    if (vPanel != null)
                    {
                        vPanel.Enabled = true;
                        var slot = slots.FirstOrDefault(s => s.SlotNumber == $"V{i}");
                        if (slot != null)
                            vPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                    }
                }
                else
                {
                    // If vehicle type is unknown, enable all panels
                    if (vPanel != null)
                    {
                        vPanel.Enabled = true;
                        var slot = slots.FirstOrDefault(s => s.SlotNumber == $"V{i}");
                        if (slot != null)
                            vPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                    }
                    if (mPanel != null)
                    {
                        mPanel.Enabled = true;
                        var slot = slots.FirstOrDefault(s => s.SlotNumber == $"M{i}");
                        if (slot != null)
                            mPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                    }
                }
            }
        }

        private void RestoreAllSlotPanels()
        {
            for (int i = 1; i <= 24; i++)
            {
                string vPanelName = $"pnlV{i}";
                string mPanelName = $"pnlM{i}";

                var vPanel = this.Controls.Find(vPanelName, true).FirstOrDefault() as Panel;
                var mPanel = this.Controls.Find(mPanelName, true).FirstOrDefault() as Panel;

                if (vPanel != null)
                {
                    vPanel.Enabled = true;
                    var slot = slots.FirstOrDefault(s => s.SlotNumber == $"V{i}");
                    if (slot != null)
                        vPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                }
                if (mPanel != null)
                {
                    mPanel.Enabled = true;
                    var slot = slots.FirstOrDefault(s => s.SlotNumber == $"M{i}");
                    if (slot != null)
                        mPanel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                }
            }
        }
    }
}
