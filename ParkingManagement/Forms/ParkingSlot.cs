using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingManagement.Model;
using ParkingManagement.Models; // Make sure to include the namespace for your Client class
using Microsoft.EntityFrameworkCore; // Add this at the top if not present

namespace ParkingManagement.Forms
{
    public partial class ParkingSlot : Form
    {
        List<Client> clients = new List<Client>();
        List<Parkingslot> slots = new List<Parkingslot>();

        public ParkingSlot()
        {
            InitializeComponent();
            this.Load += ParkingSlot_Load;
            cbName.SelectedIndexChanged += cbName_SelectedIndexChanged;
            cbSlotV.SelectedIndexChanged += cbSlot_SelectedIndexChanged;
            cbSlotM.SelectedIndexChanged += cbSlot_SelectedIndexChanged;
            btnAdd.Click += btnPark_Click; // Add a button for parking action
        }

        private void ParkingSlot_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                clients = db.Clients.Include(c => c.VehicleList).ToList();
                slots = db.Parkingslots.ToList();
            }

            cbName.DataSource = clients;
            cbName.DisplayMember = "Name";
            cbName.ValueMember = "ClientID";


            cbSlotV.DataSource = slots;
            cbSlotV.DisplayMember = "SlotNumber";
            cbSlotV.ValueMember = "SlotNumber";
            
            cbSlotM.DataSource = slots;
            cbSlotM.DisplayMember = "SlotNumber";
            cbSlotM.ValueMember = "SlotNumber";
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClient = cbName.SelectedItem as Client;
            if (selectedClient != null)
            {
                cbVehicle.DataSource = selectedClient.VehicleList.ToList();
                cbVehicle.DisplayMember = "PlateNumber"; // or any property you want to show
                cbVehicle.ValueMember = "VehicleID";
            }
            else
            {
                cbVehicle.DataSource = null;
            }
        }

        private void cbSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSlot = (sender == cbSlotV) ? cbSlotV.SelectedItem?.ToString() : cbSlotM.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedSlot)) return;

            // Find the panel by name (e.g., panelV1, panelM1)
            var panel = this.Controls.Find("panel" + selectedSlot, true).FirstOrDefault() as Panel;
            if (panel != null)
            {
                // Check if slot is occupied
                var slot = slots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
                if (slot != null && slot.SlotStatus == "occupied")
                {
                    panel.BackColor = Color.Red;
                }
                else
                {
                    panel.BackColor = Color.Green;
                }
            }
        }

        private void btnPark_Click(object sender, EventArgs e)
        {
            var selectedClient = cbName.SelectedItem as Client;
            var selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            string selectedSlot = cbSlotV.SelectedItem?.ToString() ?? cbSlotM.SelectedItem?.ToString();

            if (selectedClient == null || selectedVehicle == null || string.IsNullOrEmpty(selectedSlot))
            {
                MessageBox.Show("Please select client, vehicle, and slot.");
                return;
            }

            using (var db = new ParkingDbContext())
            {
                // Check if slot is already occupied
                var slot = db.Parkingslots.FirstOrDefault(s => s.SlotNumber == selectedSlot);
                if (slot != null && slot.SlotStatus == "occupied")
                {
                    MessageBox.Show("Slot is already occupied.");
                    return;
                }

                // If slot doesn't exist, create it
                if (slot == null)
                {
                    slot = new Parkingslot
                    {
                        SlotNumber = selectedSlot,
                        VehicleType = selectedVehicle.VehicleType,
                        VehicleStatus = "parked",
                        SlotStatus = "occupied",
                        VehicleID = selectedVehicle.VehicleID,
                        ClientID = selectedClient.ClientID
                    };
                    db.Parkingslots.Add(slot);
                }
                else
                {
                    slot.VehicleType = selectedVehicle.VehicleType;
                    slot.VehicleStatus = "parked";
                    slot.SlotStatus = "occupied";
                    slot.VehicleID = selectedVehicle.VehicleID;
                    slot.ClientID = selectedClient.ClientID;
                    db.Parkingslots.Update(slot);
                }

                // Update vehicle status
                var vehicle = db.Vehicles.FirstOrDefault(v => v.VehicleID == selectedVehicle.VehicleID);
                if (vehicle != null)
                {
                    vehicle.VehicleType = selectedVehicle.VehicleType;
                }

                db.SaveChanges();
            }

            // Update panel color
            var panel = this.Controls.Find("panel" + selectedSlot, true).FirstOrDefault() as Panel;
            if (panel != null)
            {
                panel.BackColor = Color.Red;
            }

            MessageBox.Show("Vehicle parked successfully.");
        }
    }
}

