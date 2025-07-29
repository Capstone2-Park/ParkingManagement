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
            LoadSlots();

            using (var db = new ParkingDbContext())
            {
                var recentSession = db.RegularParkingSessions
                    .OrderByDescending(s => s.SessionID)
                    .FirstOrDefault();

                if (recentSession != null)
                {
                    selectedPlateNumber = recentSession.PlateNumber;
                    selectedVehicleType = recentSession.VehicleType;
                    lblVehicle.Text = selectedPlateNumber;

                    MessageBox.Show("Please select a Slot.", "Select Slot", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (selectedVehicleType == "2-Wheels")
                    {
                        SetPanelsEnabled("V", false, Color.Gray); // disables and sets gray
                        SetPanelsEnabled("M", true);              // enables and updates color
                    }
                    else if (selectedVehicleType == "4-Wheels")
                    {
                        SetPanelsEnabled("M", false, Color.Gray); // disables and sets gray
                        SetPanelsEnabled("V", true);              // enables and updates color
                    }
                    else
                    {
                        SetAllPanelsEnabled(false);
                    }
                }
                else
                {
                    lblVehicle.Text = "No vehicle found";
                    SetAllPanelsEnabled(false);
                }
            }

            // Attach click event to all panels
            for (int i = 1; i <= 24; i++)
            {
                var vPanel = this.Controls.Find($"pnlV{i}", true).FirstOrDefault() as Panel;
                var mPanel = this.Controls.Find($"pnlM{i}", true).FirstOrDefault() as Panel;
                if (vPanel != null) vPanel.Click += Panel_Click;
                if (mPanel != null) mPanel.Click += Panel_Click;
            }

            // Only update slot panel colors after loading slots or parking
            // Remove this line from here:
            // UpdateSlotPanelColors();
        }

        private void LoadSlots()
        {
            using (var db = new ParkingDbContext())
            {
                var slotSet = db.Set<Models.RegularParkingSlot>();
                slots = slotSet != null ? slotSet.ToList() : new List<Models.RegularParkingSlot>();
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
                panel.BackColor = Color.Red;
                MessageBox.Show($"Vehicle parked in slot {slotNumber}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetAllPanelsEnabled(false);

                selectedPlateNumber = null;
                selectedVehicleType = null;
            }
        }
    }
}
