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
        private List<Parkingslot> slots;
        private int sessionId;
        private string selectedPlateNumber = null;
        private string selectedVehicleType = null;

        public RegularParkingSlot(int sessionId)
        {
            InitializeComponent();
            this.sessionId = sessionId;
            Load += RegularParkingSlot_Load;
        }

        private void RegularParkingSlot_Load(object sender, EventArgs e)
        {
            try
            {
                LoadSlots();

                using (var db = new ParkingDbContext())
                {
                    var session = db.RegularParkingSessions
                        .OrderByDescending(s => s.SessionID)
                        .FirstOrDefault();

                    if (session == null)
                    {
                        lblVehicle.Text = "No vehicle found";
                        SetAllPanelsEnabled(false);
                        MessageBox.Show("No parking session found. Please create a session first.", "No Session", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    selectedPlateNumber = session.PlateNumber ?? string.Empty;
                    selectedVehicleType = session.VehicleType ?? string.Empty;
                    lblVehicle.Text = !string.IsNullOrEmpty(selectedPlateNumber) ? selectedPlateNumber : "No Plate Number";

                    if (string.IsNullOrEmpty(selectedPlateNumber) || string.IsNullOrEmpty(selectedVehicleType))
                    {
                        SetAllPanelsEnabled(false);
                        MessageBox.Show("Session data is incomplete. Please check vehicle information.", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("Please select a Slot.", "Select Slot", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (selectedVehicleType == "2-Wheels")
                    {
                        SetPanelsEnabled("V", false, Color.Gray);
                        SetPanelsEnabled("M", true);
                    }
                    else if (selectedVehicleType == "4-Wheels")
                    {
                        SetPanelsEnabled("M", false, Color.Gray);
                        SetPanelsEnabled("V", true);
                    }
                    else
                    {
                        SetAllPanelsEnabled(false);
                    }
                }

                for (int i = 1; i <= 24; i++)
                {
                    var vPanel = this.Controls.Find($"pnlV{i}", true).FirstOrDefault() as Panel;
                    var mPanel = this.Controls.Find($"pnlM{i}", true).FirstOrDefault() as Panel;
                    if (vPanel != null) vPanel.Click += Panel_Click;
                    if (mPanel != null) mPanel.Click += Panel_Click;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetAllPanelsEnabled(false);
                lblVehicle.Text = "Error";
            }
        }

        private void LoadSlots()
        {
            using (var db = new ParkingDbContext())
            {
                var slotSet = db.Set<Parkingslot>();
                slots = slotSet != null ? slotSet.ToList() : new List<Parkingslot>();
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

            // Find or create a label for the panel
            Label statusLabel = panel.Controls.OfType<Label>().FirstOrDefault();
            if (statusLabel == null)
            {
                statusLabel = new Label
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
                panel.Controls.Add(statusLabel);
                statusLabel.BringToFront();
            }

            if (slot == null)
            {
                panel.BackColor = Color.Gold;
                statusLabel.Text = "Available";
                return;
            }

            // Determine slot status
            if (!string.IsNullOrEmpty(slot.ClientID))
            {
                // Slot is rented to a client
                panel.BackColor = Color.Red;
                statusLabel.Text = "Rented";
            }
            else if (!string.IsNullOrEmpty(slot.SlotStatus) && 
                     slot.SlotStatus.Equals("occupied", StringComparison.OrdinalIgnoreCase) &&
                     slot.SessionID != null)
            {
                // Slot is occupied by a regular parking session
                panel.BackColor = Color.Red;
                statusLabel.Text = "Occupied";
            }
            else
            {
                // Slot is available
                panel.BackColor = Color.Gold;
                statusLabel.Text = "Available";
            }
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
                    var dbSlot = db.Set<Parkingslot>().FirstOrDefault(s => s.SlotNumber == slotNumber);
                    if (dbSlot != null)
                    {
                        dbSlot.VehicleStatus = "Parked";
                        dbSlot.SlotStatus = "occupied";
                        dbSlot.SessionID = sessionId; // Link to RegularParkingSession
                        
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

                // Navigate to QRcode form
                var homePage = this.ParentForm as HomePage;
                if (homePage != null)
                {
                    var qrForm = new QRcode(sessionId); // Pass sessionId if QRcode expects it
                    homePage.ShowFormInPanel(qrForm);
                }
                else
                {
                    // Fallback: just show QRcode as a dialog if not hosted in HomePage
                    var qrForm = new QRcode(sessionId);
                    qrForm.ShowDialog();
                }
            }
        }
    }
}
