using ParkingManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement.Forms
{
    public partial class SecurityControl : Form
    {
        private List<EnterLogRent> enterLogs = new();
        private List<EnterLogRent> exitLogs = new();

        public SecurityControl()
        {
            InitializeComponent();
        }

        private async void SecurityControl_Load(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();

            // Fill cbRent with PlateNumbers whose EndDateTime is today (non-nullable)
            var today = DateTime.Today;
            var duePlateNumbers = await (
                from vs in db.VehicleSessions
                where vs.EndDateTime.Date == today
                join v in db.Vehicles on vs.VehicleID equals v.VehicleID
                select v.PlateNumber
            ).Distinct().ToListAsync();

            cbRent.Items.Clear();
            if (duePlateNumbers.Any())
            {
                cbRent.Items.AddRange(duePlateNumbers.ToArray());
                cbRent.SelectedIndex = 0;
            }
            // No else: if none, ComboBox remains empty

            // Load enterLogs for currently parked vehicles (EndDateTime == DateTime.MinValue or some other logic)
            enterLogs = await (
                from vs in db.VehicleSessions
                where vs.EndDateTime == DateTime.MinValue // or your logic for "currently parked"
                join v in db.Vehicles on vs.VehicleID equals v.VehicleID
                join c in db.Clients on v.ClientID equals c.ClientID
                select new EnterLogRent
                {
                    LogID = vs.SessionID,
                    Name = c.Name,
                    PlateNumber = v.PlateNumber,
                    VehicleType = v.VehicleType
                }
            ).ToListAsync();

            dgvEnter.DataSource = null;
            dgvEnter.DataSource = enterLogs;
            dgvEnter.ReadOnly = true;

            // Initialize exitLogs and dgvExit
            exitLogs = new List<EnterLogRent>();
            dgvExit.DataSource = null;
            dgvExit.DataSource = exitLogs;
            dgvExit.ReadOnly = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (cbRent.SelectedItem == null || cbRent.SelectedItem.ToString() == "No rent is currently due")
                return;

            string selectedPlate = cbRent.SelectedItem.ToString();

            // Find the log in enterLogs
            var log = enterLogs.FirstOrDefault(l => l.PlateNumber == selectedPlate);
            if (log != null)
            {
                enterLogs.Remove(log);
                exitLogs.Add(log);

                // Refresh DataGridViews
                dgvEnter.DataSource = null;
                dgvEnter.DataSource = enterLogs;
                dgvExit.DataSource = null;
                dgvExit.DataSource = exitLogs;
            }
        }

        private void dgvEnter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
