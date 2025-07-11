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

        public ParkingSlot()
        {
            InitializeComponent();
            Load += ParkingSlot_Load;
            cbName.SelectedIndexChanged += cbName_SelectedIndexChanged;
            cbVehicle.SelectedIndexChanged += cbVehicle_SelectedIndexChanged;
            cbSlotV.SelectedIndexChanged += cbSlot_SelectedIndexChanged;
            cbSlotM.SelectedIndexChanged += cbSlot_SelectedIndexChanged;
            btnAdd.Click += btnPark_Click;
        }

        private void ParkingSlot_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                // Load clients and vehicles
                clients = db.Clients.Include(c => c.VehicleList).ToList();
                vehicles = db.Vehicles.ToList();
                slots = db.Parkingslot.ToList();
            }

            cbName.DataSource = clients;
            cbName.DisplayMember = "Name";
            cbName.ValueMember = "ClientID";
            cbVehicle.Enabled = false;
            cbSlotV.Enabled = false;
            cbSlotM.Enabled = false;

          
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClient = cbName.SelectedItem as Client;
            if (selectedClient != null)
            {
                cbVehicle.DataSource = selectedClient.VehicleList.ToList();
                cbVehicle.DisplayMember = "PlateNumber";
                cbVehicle.ValueMember = "VehicleID";
                cbVehicle.Enabled = true;

                // Select first vehicle if available and trigger event
                if (cbVehicle.Items.Count > 0)
                {
                    cbVehicle.SelectedIndex = 0;
                    cbVehicle_SelectedIndexChanged(cbVehicle, EventArgs.Empty);
                }
                else
                {
                    cbSlotV.Enabled = false;
                    cbSlotM.Enabled = false;
                }
            }
            else
            {
                cbVehicle.DataSource = null;
                cbVehicle.Enabled = false;
                cbSlotV.Enabled = false;
                cbSlotM.Enabled = false;
            }
        }

        private void cbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null) return;

            var vehicleType = selectedVehicle.VehicleType;

            if (vehicleType == "2-wheels")
            {
                cbSlotM.DataSource = slots
                    .Where(s => s.SlotNumber.StartsWith("M") && s.SlotStatus == "available")
                    .Select(s => s.SlotNumber)
                    .ToList();
                cbSlotM.Enabled = true;
                cbSlotV.Enabled = false;
            }
            else if (vehicleType == "4-wheels")
            {
                cbSlotV.DataSource = slots
                    .Where(s => s.SlotNumber.StartsWith("V") && s.SlotStatus == "available")
                    .Select(s => s.SlotNumber)
                    .ToList();
                cbSlotV.Enabled = true;
                cbSlotM.Enabled = false;
            }
            else
            {
                cbSlotV.Enabled = false;
                cbSlotM.Enabled = false;
            }
        }

        private void cbSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSlot = (sender == cbSlotV) ? cbSlotV.SelectedItem?.ToString() : cbSlotM.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSlot)) return;

            var slot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
            var panel = this.Controls.Find("panel" + selectedSlot, true).FirstOrDefault() as Panel;
            if (panel != null)
            {
                panel.BackColor = (slot != null && slot.SlotStatus == "occupied") ? Color.Red : Color.Green;
            }
        }

        private void btnPark_Click(object sender, EventArgs e)
        {
            var selectedClient = cbName.SelectedItem as Client;
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            string selectedSlot = cbSlotV.Enabled ? cbSlotV.SelectedItem?.ToString() : cbSlotM.SelectedItem?.ToString();

            if (selectedClient == null || selectedVehicle == null || string.IsNullOrEmpty(selectedSlot))
            {
                MessageBox.Show("Please select client, vehicle, and slot.");
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var slot = db.Parkingslot.FirstOrDefault(s => s.SlotNumber == selectedSlot);

                if (slot == null)
                {
                    MessageBox.Show("Slot not found.");
                    return;
                }

                if (slot.SlotStatus == "occupied")
                {
                    MessageBox.Show("Slot is already occupied.");
                    return;
                }

                // Update slot info
                slot.VehicleID = selectedVehicle.VehicleID;
                slot.ClientID = selectedClient.ClientID;
                slot.VehicleStatus = "parked";
                slot.SlotStatus = "occupied";
                db.Parkingslot.Update(slot);
                db.SaveChanges();
            }

            // Update local slot and panel color
            var updatedSlot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
            if (updatedSlot != null)
            {
                updatedSlot.SlotStatus = "occupied";
            }
            var panel = this.Controls.Find("panel" + selectedSlot, true).FirstOrDefault() as Panel;
            if (panel != null)
            {
                panel.BackColor = Color.Red;
            }

            MessageBox.Show("Vehicle parked successfully.");
        }
    }
}





