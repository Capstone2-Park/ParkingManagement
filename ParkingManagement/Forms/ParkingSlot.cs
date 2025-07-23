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
        private List<Parkingslot> slots;
        private List<Vehicle> vehicles;
        private Client currentClient; // Store the current client
        private string selectedSlot; // Track the currently selected slot

        public ParkingSlot(Client client)
        {
            InitializeComponent();
            currentClient = client; // Set the current client
            Load += ParkingSlot_Load;
            btnAdd.Click += btnPark_Click;
            btnSelect.Click += btnSelect_Click;
        }

        private void ParkingSlot_Load(object sender, EventArgs e)
        {
            lblName.Text = currentClient?.Name ?? "No Client Selected";

            using (var db = new ParkingDbContext())
            {
                // Load all slots
                slots = db.Parkingslot.ToList();

                // Load the current client's vehicles
                vehicles = db.Vehicles
                    .Where(v => v.ClientID == currentClient.ClientID)
                    .ToList();

                // Get all vehicle IDs that are already parked
                var parkedVehicleIds = slots
                    .Where(s => s.SlotStatus == "occupied")
                    .Select(s => s.VehicleID)
                    .ToHashSet();

                // Filter vehicles to only those not parked
                vehicles = vehicles
                    .Where(v => !parkedVehicleIds.Contains(v.VehicleID))
                    .ToList();

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

                // Reload slots after seeding
                slots = db.Parkingslot.ToList();
            }

            // Bind filtered vehicle list for the current client
            cbVehicle.DataSource = vehicles;
            cbVehicle.DisplayMember = "PlateNumber";
            cbVehicle.ValueMember = "VehicleID";
            cbVehicle.Enabled = vehicles.Any();

            if (vehicles.Any())
            {
                cbVehicle.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("All vehicles for this client are already parked.");
            }

            // Initialize panel click events with recursive search
            InitializePanelClickEvents();

            UpdateSlotPanelColors();
            btnAdd.Enabled = false; // Disable btnAdd until a slot is selected
        }

        private void InitializePanelClickEvents()
        {
            foreach (Control control in GetAllControls(this))
            {
                if (control is Panel panel && panel.Name.StartsWith("pnl"))
                {
                    panel.Enabled = true; // Ensure panel is enabled
                    panel.BringToFront(); // Ensure panel is not overlapped
                    panel.Click -= Panel_Click; // Prevent duplicate handlers
                    panel.Click += Panel_Click;
                    Console.WriteLine($"Attached click event to panel: {panel.Name} at {DateTime.Now}");
                }
            }
        }

        private IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control c in container.Controls)
            {
                yield return c;
                foreach (Control child in GetAllControls(c))
                {
                    yield return child;
                }
            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                Console.WriteLine($"Panel_Click triggered for: {panel.Name} at {DateTime.Now}");

                // Check if a vehicle is selected
                var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
                if (selectedVehicle == null)
                {
                    MessageBox.Show("Please select a vehicle first.");
                    return;
                }

                // Check if the selected vehicle is already parked
                using (var db = new ParkingDbContext())
                {
                    var isVehicleParked = db.Parkingslot.Any(s => s.VehicleID == selectedVehicle.VehicleID && s.SlotStatus == "occupied");
                    if (isVehicleParked)
                    {
                        MessageBox.Show($"Vehicle {selectedVehicle.PlateNumber} is already parked in a slot.");
                        return;
                    }
                }

                string slotNumber = panel.Name.Replace("pnl", "");
                var slot = slots.FirstOrDefault(s => s.SlotNumber == slotNumber);

                if (slot == null || slot.SlotStatus == "occupied")
                {
                    MessageBox.Show($"Slot {slotNumber} is not available for parking.");
                    return;
                }

                // Check if the slot is compatible with the vehicle type
                bool isCompatible = (selectedVehicle.VehicleType?.Trim() == "2-Wheels" && slotNumber.StartsWith("M")) ||
                                   (selectedVehicle.VehicleType?.Trim() == "4-Wheels" && slotNumber.StartsWith("V"));
                if (!isCompatible)
                {
                    MessageBox.Show($"Slot {slotNumber} is not compatible with the selected vehicle type.");
                    return;
                }

                // Reset other panels
                foreach (Control control in GetAllControls(this))
                {
                    if (control is Panel p && p != panel && p.Name.StartsWith("pnl"))
                    {
                        string otherSlotNumber = p.Name.Replace("pnl", "");
                        var otherSlot = slots.FirstOrDefault(s => s.SlotNumber == otherSlotNumber);
                        p.BorderStyle = BorderStyle.None;
                        p.BackColor = (otherSlot?.SlotStatus == "occupied") ? Color.Red : Color.Green;
                    }
                }

                // Update the clicked panel immediately
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = Color.Red;
                panel.Refresh(); // Force immediate UI update

                selectedSlot = slotNumber; // Set the selected slot
                btnAdd.Enabled = true; // Enable btnAdd

                Console.WriteLine($"Panel clicked: {panel.Name}, Selected Slot: {selectedSlot}, Color: {panel.BackColor} at {DateTime.Now}");
                this.Refresh(); // Force form refresh to ensure UI updates
            }
        }

        private void UpdateSlotPanelColors()
        {
            foreach (var slot in slots)
            {
                var panel = this.Controls.Find("pnl" + slot.SlotNumber, true).FirstOrDefault() as Panel;
                if (panel != null)
                {
                    // Set panel color based on slot status
                    panel.BackColor = slot.SlotStatus == "occupied" ? Color.Red : Color.Green;
                    panel.BorderStyle = BorderStyle.None; // Reset border
                    panel.Refresh(); // Force immediate UI update

                    // Find or create a label for the panel
                    var label = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblStatus" + slot.SlotNumber);
                    if (label == null)
                    {
                        label = new Label
                        {
                            Name = "lblStatus" + slot.SlotNumber,
                            Text = slot.SlotStatus == "occupied" ? "Occupied" : "Available",
                            AutoSize = true,
                            ForeColor = Color.White,
                            BackColor = Color.Transparent,
                            Location = new Point(5, 5), // Position label in top-left corner
                            Font = new Font("Arial", 10, FontStyle.Bold)
                        };
                        panel.Controls.Add(label);
                        label.BringToFront();
                    }
                    else
                    {
                        // Update existing label
                        label.Text = slot.SlotStatus == "occupied" ? "Occupied" : "Available";
                    }
                }
            }
            this.Refresh(); // Force form refresh
        }

       
     
        private void btnPark_Click(object sender, EventArgs e)
        {
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null || string.IsNullOrEmpty(selectedSlot))
            {
                MessageBox.Show("Please select a vehicle and slot.");
                btnAdd.Enabled = false;
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var slot = db.Parkingslot.FirstOrDefault(s => s.SlotNumber == selectedSlot);

                if (slot == null)
                {
                    MessageBox.Show("Slot not found.");
                    btnAdd.Enabled = false;
                    return;
                }

                if (slot.SlotStatus == "occupied")
                {
                    MessageBox.Show("Slot is already occupied.");
                    btnAdd.Enabled = false;
                    return;
                }

                // Update slot info
                slot.VehicleID = selectedVehicle.VehicleID;
                slot.ClientID = currentClient.ClientID;
                slot.VehicleStatus = "parked";
                slot.SlotStatus = "occupied";
                db.Parkingslot.Update(slot);
                db.SaveChanges();

                Console.WriteLine($"Parked vehicle {selectedVehicle.VehicleID} in slot {selectedSlot} at {DateTime.Now}");
            }

            // Update local slot
            var updatedSlot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
            if (updatedSlot != null)
            {
                updatedSlot.SlotStatus = "occupied";
            }
            selectedSlot = null; // Clear selection after parking
            btnAdd.Enabled = false; // Disable btnAdd after parking

            MessageBox.Show("Vehicle parked successfully.");
            UpdateSlotPanelColors(); // Update colors and labels based on database state
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle first.");
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var isVehicleParked = db.Parkingslot.Any(s => s.VehicleID == selectedVehicle.VehicleID && s.SlotStatus == "occupied");
                if (isVehicleParked)
                {
                    MessageBox.Show($"Vehicle {selectedVehicle.PlateNumber} is already parked in a slot.");
                    return;
                }
            }

            var vehicleType = selectedVehicle.VehicleType?.Trim();

            foreach (Control control in GetAllControls(this))
            {
                if (control is Panel panel && panel.Name.StartsWith("pnl"))
                {
                    string slotNumber = panel.Name.Replace("pnl", "");
                    var slot = slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
                    bool isCompatible = (vehicleType == "2-Wheels" && slotNumber.StartsWith("M")) ||
                                       (vehicleType == "4-Wheels" && slotNumber.StartsWith("V"));
                    panel.Enabled = isCompatible && slot?.SlotStatus == "available";
                    panel.BackColor = panel.Enabled ? Color.Green : (slot?.SlotStatus == "occupied" ? Color.Red : Color.Green);
                    Console.WriteLine($"Panel: {panel.Name}, Enabled: {panel.Enabled}, Color: {panel.BackColor} at {DateTime.Now}");
                }
            }
            this.Refresh(); // Force UI update
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var parkRental = new ParkRental();
                homePage.ShowFormInPanel(parkRental);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var clientM = new ClientManagement();
                homePage.ShowFormInPanel(clientM);
            }
        }
    }
}
