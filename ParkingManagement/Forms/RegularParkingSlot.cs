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
                // Get all plate numbers from RegularParkingSession
                var plateNumbers = db.RegularParkingSessions
                    .Select(s => s.PlateNumber)
                    .Distinct()
                    .ToList();

                cbVehicle.DataSource = plateNumbers;

                // Load all slots from the database
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


            }
            else if (vehicleType == "4-Wheels")
            {
                var availableSlots = slots
                    .Where(s => s.SlotNumber.StartsWith("V") && s.SlotStatus == "available")
                    .Select(s => s.SlotNumber)
                    .ToList();

            }
            else
            {
            
            }
        }

       

      

        private void Panel_Click(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedItem == null) return;
            string selectedPlate = cbVehicle.SelectedItem.ToString();
            var panel = sender as Panel;
            if (panel == null) return;

            string slotNumber = panel.Name.Replace("pnl", ""); // e.g., "V1" or "M1"

            using (var db = new ParkingDbContext())
            {
                var slot = db.Set<Models.RegularParkingSlot>().FirstOrDefault(s => s.SlotNumber == slotNumber);
                if (slot == null) return;

                slot.VehicleStatus = "Parked";
                slot.SlotStatus = "occupied";
                db.SaveChanges();
            }

            panel.BackColor = Color.RoyalBlue;
            MessageBox.Show($"Vehicle parked in slot {slotNumber}.");
        }
    }
}
