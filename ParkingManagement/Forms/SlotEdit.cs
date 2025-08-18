using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class SlotEdit : Form
    {
        private Parkingslot slot;
        private ParkingDbContext db;

        public SlotEdit()
        {
            InitializeComponent();
            db = new ParkingDbContext();

            // Attach validation
            txtMotorcycle.KeyPress += TxtInt_KeyPress;
            txtVehicle.KeyPress += TxtInt_KeyPress;

            // Attach TextChanged event for instant save
            txtMotorcycle.TextChanged += txtMotorcycle_TextChanged;
            txtVehicle.TextChanged += txtVehicle_TextChanged;
        }

        private void SlotEdit_Load(object sender, EventArgs e)
        {
            slot = db.Parkingslot.FirstOrDefault();
            if (slot != null)
            {
                txtMotorcycle.Text = slot.AvailableSlotM.ToString();
                txtVehicle.Text = slot.AvailableSlotV.ToString();
            }
            else
            {
                MessageBox.Show("No Parkingslot record found in the database.");
            }
        }

        private void TxtInt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and control keys (e.g., backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void SaveSlots()
        {
            if (slot != null)
            {
                // Parse and save values
                if (int.TryParse(txtMotorcycle.Text, out int m))
                    slot.AvailableSlotM = m;
                if (int.TryParse(txtVehicle.Text, out int v))
                    slot.AvailableSlotV = v;

                db.SaveChanges();
            }
        }

        private void btnSlotAddM_Click(object sender, EventArgs e)
        {
            if (slot != null)
            {
                slot.AvailableSlotM++;
                txtMotorcycle.Text = slot.AvailableSlotM.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotRemoveM_Click(object sender, EventArgs e)
        {
            if (slot != null && slot.AvailableSlotM > 0)
            {
                slot.AvailableSlotM--;
                txtMotorcycle.Text = slot.AvailableSlotM.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotAddV_Click(object sender, EventArgs e)
        {
            if (slot != null)
            {
                slot.AvailableSlotV++;
                txtVehicle.Text = slot.AvailableSlotV.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotRemoveV_Click(object sender, EventArgs e)
        {
            if (slot != null && slot.AvailableSlotV > 0)
            {
                slot.AvailableSlotV--;
                txtVehicle.Text = slot.AvailableSlotV.ToString();
                db.SaveChanges();
            }
        }

        // New event handlers for instant save
        private void txtMotorcycle_TextChanged(object sender, EventArgs e)
        {
            SaveSlots();
        }

        private void txtVehicle_TextChanged(object sender, EventArgs e)
        {
            SaveSlots();
        }

        private void SlotEdit_Load_1(object sender, EventArgs e)
        {
            slot = db.Parkingslot.FirstOrDefault();
            if (slot != null)
            {
                txtMotorcycle.Text = slot.AvailableSlotM.ToString();
                txtVehicle.Text = slot.AvailableSlotV.ToString();
            }
            else
            {
                MessageBox.Show("No Parkingslot record found in the database.");
            }
        }
    }
}
