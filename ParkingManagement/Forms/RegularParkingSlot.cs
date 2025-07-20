using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;
using ParkingManagement.Model;

namespace ParkingManagement.Forms
{
    public partial class RegularParkingSlot : Form
    {
        private List<Models.RegularParkingSlot> slots; // Fix: Specify the correct namespace for RegularParkingSlot  

        public RegularParkingSlot()
        {
            InitializeComponent();
            Load += RegularParkingSlot_Load;
        }

        private void RegularParkingSlot_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                // Seed V1–V24 and M1–M24
                var allSlotNumbers = Enumerable.Range(1, 24).Select(i => "V" + i)
                    .Concat(Enumerable.Range(1, 24).Select(i => "M" + i))
                    .ToList();
                foreach (var slotNumber in allSlotNumbers)
                {
                    if (!db.Set<Models.RegularParkingSlot>().Any(s => s.SlotNumber == slotNumber))
                    {
                        db.Set<Models.RegularParkingSlot>().Add(new Models.RegularParkingSlot
                        {
                            SlotNumber = slotNumber,
                            SlotStatus = "available",
                            VehicleStatus = "NotParked"
                        });
                    }
                }
                db.SaveChanges();
                slots = db.Set<Models.RegularParkingSlot>().ToList();
            }
            UpdateSlotPanelColors();
        }

        private void UpdateSlotPanelColors()
        {
            foreach (var slot in slots)
            {
                // Panel names assumed to be "pnl" + SlotNumber (e.g., pnlR1)  
                var panel = this.Controls.Find("pnl" + slot.SlotNumber, true).FirstOrDefault() as Panel;
                if (panel != null)
                {
                    panel.BackColor = slot.SlotStatus == "occupied" ? Color.RoyalBlue : Color.Gold;
                }
            }
        }

        private void MarkSlotOccupied(string slotNumber)
        {
            using (var db = new ParkingDbContext())
            {
                var slot = db.Set<Models.RegularParkingSlot>().FirstOrDefault(s => s.SlotNumber == slotNumber); // Fix: Use db.Set<T>() to access DbSet  
                if (slot != null && slot.SlotStatus != "occupied")
                {
                    slot.SlotStatus = "occupied";
                    slot.VehicleStatus = "Parked";
                    db.SaveChanges();
                }
            }
            using (var db = new ParkingDbContext())
            {
                slots = db.Set<Models.RegularParkingSlot>().ToList(); // Fix: Use db.Set<T>() to access DbSet  
            }
            UpdateSlotPanelColors();
        }

        private void MarkSlotAvailable(string slotNumber)
        {
            using (var db = new ParkingDbContext())
            {
                var slot = db.Set<Models.RegularParkingSlot>().FirstOrDefault(s => s.SlotNumber == slotNumber); // Fix: Use db.Set<T>() to access DbSet  
                if (slot != null && slot.SlotStatus != "available")
                {
                    slot.SlotStatus = "available";
                    slot.VehicleStatus = "NotParked";
                    db.SaveChanges();
                }
            }
            using (var db = new ParkingDbContext())
            {
                slots = db.Set<Models.RegularParkingSlot>().ToList(); // Fix: Use db.Set<T>() to access DbSet  
            }
            UpdateSlotPanelColors();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Assume cbVehicle contains vehicle types: "2-Wheels" or "4-Wheels"
            string vehicleType = cbVehicle.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(vehicleType))
            {
                MessageBox.Show("Please select a vehicle type.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filter available slots based on vehicle type
            if (vehicleType == "2-Wheels")
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
            else if (vehicleType == "4-Wheels")
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string selectedSlot = cbSlotV.Enabled ? cbSlotV.SelectedItem?.ToString() : cbSlotM.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedSlot))
            {
                MessageBox.Show("Please select a slot.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var slot = db.Set<Models.RegularParkingSlot>().FirstOrDefault(s => s.SlotNumber == selectedSlot);

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

                // Update slot info
                slot.SlotStatus = "occupied";
                slot.VehicleStatus = "Parked";
                db.SaveChanges();
            }

            // Update local slot and panel color
            var updatedSlot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
            if (updatedSlot != null)
            {
                updatedSlot.SlotStatus = "occupied";
            }
            var panel = this.Controls.Find("pnl" + selectedSlot, true).FirstOrDefault() as Panel;
            if (panel != null)
            {
                panel.BackColor = Color.RoyalBlue;
            }

            MessageBox.Show("Vehicle parked successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Refresh the slot list and UI
            using (var db = new ParkingDbContext())
            {
                slots = db.Set<Models.RegularParkingSlot>().ToList();
            }
            UpdateSlotPanelColors();
        }
    }
}
