using Microsoft.EntityFrameworkCore;
using ParkingManagement.Models;
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
    public partial class Regular_Parking : Form
    {

        private ParkingDbContext _context;
        private CancellationTokenSource _searchCancellationTokenSource;
        private CancellationTokenSource _qrScanCancellationTokenSource;
        private DateTime _currentTimeIn;

        public Regular_Parking()
        {
            InitializeComponent();
            InitializeDbContext();
            InitializeDataGridView();
            SetupVehicleTypeComboBox();
            SetupDiscountComboBox(); // <-- Add this line
            _searchCancellationTokenSource = new CancellationTokenSource();
            _qrScanCancellationTokenSource = new CancellationTokenSource();
        }

        private async void Regular_Parking_Load(object sender, EventArgs e)
        {
            await GenerateNewRegularVehicleID();
            await LoadActiveParkingSessions();
            ClearInputFields();
            await LoadAvailableSlots(); // Add this line
            
        }

        private void InitializeDbContext()
        {
            _context = new ParkingDbContext();
        }

        private void InitializeDataGridView()
        {
            dgvTimeInTimeOutData.AutoGenerateColumns = false;

            dgvTimeInTimeOutData.Columns.Clear(); // Ensure no duplicate columns

            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "SessionID", HeaderText = "Session ID", DataPropertyName = "SessionID", ReadOnly = true, Visible = false });
            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "RegularVehicleID", HeaderText = "Vehicle ID", DataPropertyName = "RegularVehicleID", ReadOnly = true });
            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNumber", HeaderText = "Plate Number", DataPropertyName = "PlateNumber", ReadOnly = true });
            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "VehicleType", HeaderText = "Type", DataPropertyName = "VehicleType", ReadOnly = true });
            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "TimeIn", HeaderText = "Time In", DataPropertyName = "TimeIn", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "MM/dd/yyyy hh:mm:ss tt" } });
            // dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "QRCodeData", HeaderText = "QR Data", DataPropertyName = "QRCodeData", ReadOnly = true, Visible = false }); // REMOVE THIS LINE

            dgvTimeInTimeOutData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTimeInTimeOutData.MultiSelect = false;
        }

        private void SetupVehicleTypeComboBox()
        {
            cmbTypeOfVehicle.Items.Clear();
            cmbTypeOfVehicle.Items.Add("2-Wheels");
            cmbTypeOfVehicle.Items.Add("4-Wheels");
            cmbTypeOfVehicle.SelectedIndex = 0;
        }

        private void SetupDiscountComboBox()
        {
            cbDiscount.Items.Clear();
            cbDiscount.Items.Add("PWD/Senior Citizen");
            cbDiscount.Items.Add("No Discount");
            cbDiscount.SelectedIndex = 1; // Set "No Discount" as default
        }

        private async Task GenerateNewRegularVehicleID()
        {
            string newId = "RP000001";
            try
            {
                using var context = new ParkingDbContext();
                var lastSession = await context.RegularParkingSessions
                    .Where(s => s.RegularVehicleID != null && s.RegularVehicleID.StartsWith("RP") && s.RegularVehicleID.Length == 8)
                    .OrderByDescending(s => s.RegularVehicleID)
                    .FirstOrDefaultAsync();

                if (lastSession != null && int.TryParse(lastSession.RegularVehicleID.Substring(2), out int lastNum))
                {
                    lastNum++;
                    newId = "RP" + lastNum.ToString("D6");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new Vehicle ID: {ex.Message}", "ID Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtVehicleId.Text = newId;
        }

        private async Task LoadActiveParkingSessions()
        {
            try
            {
                var activeSessions = await _context.RegularParkingSessions
                                                   .Where(s => s.TimeOut == null)
                                                   .OrderBy(s => s.TimeIn)
                                                   .ToListAsync();
                dgvTimeInTimeOutData.DataSource = activeSessions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading active parking sessions: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAvailableSlots()
        {
            using var context = new ParkingDbContext();
            var regSlot = await context.Set<RegularSlot>().FirstOrDefaultAsync();
            txtAvailableSlotM.Text = regSlot?.AvailableSlotM.ToString() ?? "0";
            txtAvailableSlotV.Text = regSlot?.AvailableSlotV.ToString() ?? "0";
            txtOccupiedSlotM.Text = regSlot?.OccupiedSlotM.ToString() ?? "0";
            txtOccupiedSlotV.Text = regSlot?.OccupiedSlotV.ToString() ?? "0";
        }

        private void ClearInputFields()
        {
            txtAvailableSlotM.Clear();

            // Keep txtVehicleId as it will be auto-generated on load or after save
            // cmbTypeOfVehicle.SelectedIndex is already set on load, can reset if needed: cmbTypeOfVehicle.SelectedIndex = 0;
        }

        private async void btnTimeIn_Click(object sender, EventArgs e)
        {
            if (cmbTypeOfVehicle.SelectedItem == null)
            {
                MessageBox.Show("Please select the Vehicle Type.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the RegularSlot record that holds the slot counts
            var regSlot = await _context.Set<RegularSlot>().FirstOrDefaultAsync();
            if (regSlot == null)
            {
                MessageBox.Show("Regular slot data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedType = cmbTypeOfVehicle.SelectedItem.ToString();
            string selectedDiscount = cbDiscount.SelectedItem?.ToString() ?? "No Discount"; // Updated fallback

            // Decrement the appropriate slot count and increment occupied slot count
            if (selectedType == "2-Wheels")
            {
                if (regSlot.AvailableSlotM <= 0)
                {
                    MessageBox.Show("No available slots for 2-Wheels.", "Slot Full", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                regSlot.AvailableSlotM--;
                regSlot.OccupiedSlotM++;
            }
            else if (selectedType == "4-Wheels")
            {
                if (regSlot.AvailableSlotV <= 0)
                {
                    MessageBox.Show("No available slots for 4-Wheels.", "Slot Full", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                regSlot.AvailableSlotV--;
                regSlot.OccupiedSlotV++;
            }

            _currentTimeIn = DateTime.Now;
            string regularVehicleId = txtVehicleId.Text;

            RegularParkingSession newSession = new RegularParkingSession
            {
                RegularVehicleID = regularVehicleId,
                VehicleType = selectedType,
                TimeIn = _currentTimeIn,
                TimeOut = null,
                TotalAmount = null,
                Discount = selectedDiscount // <-- Save the discount
            };

            try
            {
                _context.RegularParkingSessions.Add(newSession);
                await _context.SaveChangesAsync();

                // Save the updated slot counts
                await _context.SaveChangesAsync();

                MessageBox.Show("Vehicle checked in successfully!", "Check In Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadActiveParkingSessions();
                ClearInputFields();
                await GenerateNewRegularVehicleID();
                await LoadAvailableSlots();
                MessageBox.Show($"New session created with SessionID: {newSession.SessionID}");
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Error checking in vehicle: {dbEx.InnerException?.Message ?? dbEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred during check-in: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var parkSlotForm = new QRcode();
                homePage.ShowFormInPanel(parkSlotForm);
            }
        }

        private void Regular_Parking_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource?.Dispose();
            _qrScanCancellationTokenSource?.Cancel();
            _qrScanCancellationTokenSource?.Dispose();
            _context?.Dispose();
        }

      
    }
}
