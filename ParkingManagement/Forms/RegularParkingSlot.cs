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
        private List<Models.RegularParkingSlot> slots;
        private string selectedPlateNumber = null;
        private string selectedVehicleType = null;

        public RegularParkingSlot()
        {
            InitializeComponent();
            Load += RegularParkingSlot_Load;
        }

        private void RegularParkingSlot_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var plateNumbers = db.RegularParkingSessions
                    .Select(s => s.PlateNumber)
                    .Distinct()
                    .ToList();
                cbVehicle.DataSource = plateNumbers;
            }

            // Attach click event to all panels
            for (int i = 1; i <= 24; i++)
            {
                var vPanel = this.Controls.Find($"pnlV{i}", true).FirstOrDefault() as Panel;
                var mPanel = this.Controls.Find($"pnlM{i}", true).FirstOrDefault() as Panel;
                if (vPanel != null) vPanel.Click += Panel_Click;
                if (mPanel != null) mPanel.Click += Panel_Click;
            }

            cbVehicle.SelectedIndexChanged += cbVehicle_SelectedIndexChanged;
            LoadSlots();
            UpdateSlotPanelColors();
            SetAllPanelsEnabled(false); // Disable all until vehicle is selected
        }

        private void LoadSlots()
        {
            using (var db = new ParkingDbContext())
            {
                slots = db.Set<Models.RegularParkingSlot>().ToList();
            }
        }

        private void cbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPlateNumber = cbVehicle.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPlateNumber))
                return;

            using (var db = new ParkingDbContext())
            {
                selectedVehicleType = db.RegularParkingSessions
                    .Where(s => s.PlateNumber == selectedPlateNumber)
                    .Select(s => s.VehicleType)
                    .FirstOrDefault();
            }

            if (string.IsNullOrEmpty(selectedVehicleType))
            {
                MessageBox.Show("Vehicle type not found for selected plate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetAllPanelsEnabled(false);
                return;
            }

            MessageBox.Show("Please choose a slot.", "Select Slot", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (selectedVehicleType == "2-Wheels")
            {
                SetPanelsEnabled("M", true);
                SetPanelsEnabled("V", false, Color.Gray);
            }
            else if (selectedVehicleType == "4-Wheels")
            {
                SetPanelsEnabled("V", true);
                SetPanelsEnabled("M", false, Color.Gray);
            }
            else
            {
                SetAllPanelsEnabled(false);
            }
        }

        private void SetPanelsEnabled(string prefix, bool enabled, Color? disabledColor = null)
        {
            for (int i = 1; i <= 24; i++)
            {
                var panel = this.Controls.Find($"pnl{prefix}{i}", true).FirstOrDefault() as Panel;
                if (panel != null)
                {
                    panel.Enabled = enabled;
                    if (!enabled && disabledColor.HasValue)
                        panel.BackColor = disabledColor.Value;
                    else
                        UpdatePanelColor(panel, $"pnl{prefix}{i}");
                }
            }
        }

        private void SetAllPanelsEnabled(bool enabled)
        {
            SetPanelsEnabled("V", enabled);
            SetPanelsEnabled("M", enabled);
        }

        private void UpdateSlotPanelColors()
        {
            for (int i = 1; i <= 24; i++)
            {
                UpdatePanelColor(this.Controls.Find($"pnlV{i}", true).FirstOrDefault() as Panel, $"pnlV{i}");
                UpdatePanelColor(this.Controls.Find($"pnlM{i}", true).FirstOrDefault() as Panel, $"pnlM{i}");
            }
        }

        private void UpdatePanelColor(Panel panel, string panelName)
        {
            if (panel == null) return;
            string slotNumber = panelName.Replace("pnl", "");
            var slot = slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
            if (slot == null)
            {
                panel.BackColor = Color.Gold;
                return;
            }
            if (slot.SlotStatus == "occupied")
                panel.BackColor = Color.Red;
            else
                panel.BackColor = Color.Gold;
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPlateNumber) || string.IsNullOrEmpty(selectedVehicleType))
            {
                MessageBox.Show("Please select a vehicle first.", "No Vehicle Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("This slot is already occupied. Please choose another slot.", "Slot Occupied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show($"Park vehicle {selectedPlateNumber} in slot {slotNumber}?", "Confirm Parking", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirm == DialogResult.OK)
            {
                using (var db = new ParkingDbContext())
                {
                    var dbSlot = db.Set<Models.RegularParkingSlot>().FirstOrDefault(s => s.SlotNumber == slotNumber);
                    if (dbSlot != null)
                    {
                        dbSlot.VehicleStatus = "Parked";
                        dbSlot.SlotStatus = "occupied";
                        db.SaveChanges();
                    }
                }
                LoadSlots();
                UpdateSlotPanelColors();
                MessageBox.Show($"Vehicle parked in slot {slotNumber}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetAllPanelsEnabled(false);
                cbVehicle.SelectedIndex = -1;
                selectedPlateNumber = null;
                selectedVehicleType = null;
            }
        }
    }
}
