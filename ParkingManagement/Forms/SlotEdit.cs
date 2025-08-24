using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class SlotEdit : Form
    {
        private RegularSlot regSlot;
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
            regSlot = db.Set<RegularSlot>().FirstOrDefault();
            if (regSlot != null)
            {
                txtMotorcycle.Text = regSlot.AvailableSlotM.ToString();
                txtVehicle.Text = regSlot.AvailableSlotV.ToString();
            }
            else
            {
                MessageBox.Show("No RegularSlot record found in the database.");
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
            if (regSlot != null)
            {
                // Parse and save values
                if (int.TryParse(txtMotorcycle.Text, out int m))
                    regSlot.AvailableSlotM = m;
                if (int.TryParse(txtVehicle.Text, out int v))
                    regSlot.AvailableSlotV = v;

                db.SaveChanges();
            }
        }

        private void btnSlotAddM_Click(object sender, EventArgs e)
        {
            if (regSlot != null)
            {
                regSlot.AvailableSlotM++;
                txtMotorcycle.Text = regSlot.AvailableSlotM.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotRemoveM_Click(object sender, EventArgs e)
        {
            if (regSlot != null && regSlot.AvailableSlotM > 0)
            {
                regSlot.AvailableSlotM--;
                txtMotorcycle.Text = regSlot.AvailableSlotM.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotAddV_Click(object sender, EventArgs e)
        {
            if (regSlot != null)
            {
                regSlot.AvailableSlotV++;
                txtVehicle.Text = regSlot.AvailableSlotV.ToString();
                db.SaveChanges();
            }
        }

        private void btnSlotRemoveV_Click(object sender, EventArgs e)
        {
            if (regSlot != null && regSlot.AvailableSlotV > 0)
            {
                regSlot.AvailableSlotV--;
                txtVehicle.Text = regSlot.AvailableSlotV.ToString();
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
            regSlot = db.Set<RegularSlot>().FirstOrDefault();
            if (regSlot != null)
            {
                txtMotorcycle.Text = regSlot.AvailableSlotM.ToString();
                txtVehicle.Text = regSlot.AvailableSlotV.ToString();
            }
            else
            {
                MessageBox.Show("No RegularSlot record found in the database.");
            }
        }
    }
}
