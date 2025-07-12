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

                // Seed missing slots
                var allSlotNumbers = Enumerable.Range(1, 24).Select(i => "V" + i)
                    .Concat(Enumerable.Range(1, 18).Select(i => "M" + i)).ToList();

                foreach (var slotNumber in allSlotNumbers)
                {
                    if (!db.Parkingslot.Any(s => s.SlotNumber == slotNumber))
                    {
                        db.Parkingslot.Add(new Parkingslot
                        {
                            SlotNumber = slotNumber,
                            SlotStatus = "available",
                            VehicleStatus = "NotParked"
                        });
                    }
                }
                db.SaveChanges();

                // **Reload slots after seeding**
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

                // Always trigger the vehicle selection logic
                if (cbVehicle.Items.Count > 0)
                {
                    cbVehicle.SelectedIndex = 0;
                    // This will automatically trigger cbVehicle_SelectedIndexChanged
                    // If you want to ensure it triggers, you can call it explicitly:
                    // cbVehicle_SelectedIndexChanged(cbVehicle, EventArgs.Empty);
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
            //var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            //if (selectedVehicle == null) return;

            //var vehicleType = selectedVehicle.VehicleType;

            //if (vehicleType == "2-wheels")
            //{
            //    var availableSlots = slots
            //        .Where(s => s.SlotNumber.StartsWith("M") && s.SlotStatus == "available")
            //        .Select(s => s.SlotNumber)
            //        .ToList();

            //    cbSlotM.DataSource = availableSlots;
            //    cbSlotM.Enabled = availableSlots.Count > 0;
            //    cbSlotV.Enabled = false;
            //}
            //else if (vehicleType == "4-wheels")
            //{
            //    var availableSlots = slots
            //        .Where(s => s.SlotNumber.StartsWith("V") && s.SlotStatus == "available")
            //        .Select(s => s.SlotNumber)
            //        .ToList();

            //    cbSlotV.DataSource = availableSlots;
            //    cbSlotV.Enabled = availableSlots.Count > 0;
            //    cbSlotM.Enabled = false;
            //}
            //else
            //{
            //    cbSlotV.Enabled = false;
            //    cbSlotM.Enabled = false;
            //}
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle first.");
                return;
            }

            var vehicleType = selectedVehicle.VehicleType;

            if (vehicleType == "2-wheels")
            {
                var availableSlots = slots
                    .Where(s => s.SlotNumber.StartsWith("M") && s.SlotStatus == "available")
                    .Select(s => s.SlotNumber)
                    .ToList();

                cbSlotM.DataSource = availableSlots;
                cbSlotM.Enabled = availableSlots.Count > 0;
                cbSlotV.Enabled = false;
                cbSlotV.DataSource = null;
            }
            else if (vehicleType == "4-wheels")
            {
                var availableSlots = slots
                    .Where(s => s.SlotNumber.StartsWith("V") && s.SlotStatus == "available")
                    .Select(s => s.SlotNumber)
                    .ToList();

                cbSlotV.DataSource = availableSlots;
                cbSlotV.Enabled = availableSlots.Count > 0;
                cbSlotM.Enabled = false;
                cbSlotM.DataSource = null;
            }
            else
            {
                cbSlotV.Enabled = false;
                cbSlotM.Enabled = false;
                cbSlotV.DataSource = null;
                cbSlotM.DataSource = null;
            }
        }
    }
}
