using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
using ParkingManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement.Forms
{
    public partial class ParkRental : Form
    {
        private ParkingDbContext _context;
        private Client _selectedClient;
        private Vehicle _selectedVehicle;
        private Client _currentClient;

        private DateTime _calculatedEndDateTime;
        private decimal _calculatedTotalAmount;

        private CancellationTokenSource _searchCancellationTokenSource;

        private List<(Client client, Vehicle vehicle, string durationType, DateTime startDate, DateTime endDateTime, decimal totalAmount)> _scheduledVehicles = new();

        public ParkRental()
        {
            InitializeComponent();
            _context = new ParkingDbContext();
            InitializeDataGridView();
            SetupDurationTypeComboBox();
            PopulateTimeComboBox();
            // Wire up the selection changed event
            dgvVehicles.SelectionChanged += dgvVehicles_SelectionChanged;

            // Get the latest client (assuming highest ClientID is the latest)
            _currentClient = _context.Clients
                .OrderByDescending(c => c.ClientID)
                .FirstOrDefault();

            if (_currentClient != null)
            {
                lblClientName.Text = _currentClient.Name;
                LoadClientVehicles();
            }
            else
            {
                lblClientName.Text = "No client found";
                dgvVehicles.DataSource = null;
            }
        }

        public ParkRental(Client currentClient)
        {
            InitializeComponent();
            _context = new ParkingDbContext();
            _currentClient = currentClient;

            // Set client name label
            lblClientName.Text = _currentClient.Name;

            // Load vehicles for this client
            LoadClientVehicles();
            SetupDurationTypeComboBox();
        }



        // Initialize the DataTable structure for fees display
        private void InitializeDataGridView()
        {
            dgvVehicles.AutoGenerateColumns = false;

            // Client Columns
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClientID",
                HeaderText = "Client ID",
                DataPropertyName = "ClientID",
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClientName",
                HeaderText = "Client Name",
                DataPropertyName = "ClientName", // This maps to the `ClientName` property in the anonymous object
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClientContact",
                HeaderText = "Contact No.",
                DataPropertyName = "ClientContact", // <--- CRITICAL FIX: Match the name in the anonymous object
                ReadOnly = true
            });

            // Vehicle Columns (flattened for DGV)
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleID",
                HeaderText = "Vehicle ID",
                DataPropertyName = "VehicleID",
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlateNumber",
                HeaderText = "Plate No.",
                DataPropertyName = "PlateNumber",
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Brand",
                HeaderText = "Brand",
                DataPropertyName = "Brand",
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Color",
                HeaderText = "Color",
                DataPropertyName = "Color",
                ReadOnly = true
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleType",
                HeaderText = "Type",
                DataPropertyName = "VehicleType",
                ReadOnly = true
            });

            // Hidden columns to store the actual objects
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ClientObject",
                HeaderText = "",
                DataPropertyName = "ClientObject",
                Visible = false
            });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleObject",
                HeaderText = "",
                DataPropertyName = "VehicleObject",
                Visible = false
            });

            dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehicles.MultiSelect = false;
        }

        private void SetupDurationTypeComboBox()
        {
            cmbDurationType.Items.Clear();
            cmbDurationType.Items.Add("Daily");
            cmbDurationType.Items.Add("Weekly");
            cmbDurationType.Items.Add("Monthly");
            cmbDurationType.Items.Add("Yearly");
            cmbDurationType.SelectedIndex = 0; // Default to Daily
        }

        // Replace the existing UpdateNextButtonState with this async version:
        private async Task UpdateNextButtonStateAsync()
        {
            // Check for scheduled vehicles in the current session
            bool hasScheduledInSession = _scheduledVehicles.Count > 0;

            // Check for scheduled vehicles in the database
            bool hasScheduledInDb = await _context.VehicleSessions.AnyAsync();

            btnNext.Enabled = hasScheduledInSession || hasScheduledInDb;
        }

        private void LoadClientVehicles()
        {
            if (_currentClient == null)
            {
                dgvVehicles.DataSource = null;
                return;
            }

            var vehicles = _context.Vehicles
                .Where(v => v.ClientID == _currentClient.ClientID)
                .ToList();

            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("ClientContact");
            dt.Columns.Add("VehicleID");
            dt.Columns.Add("PlateNumber");
            dt.Columns.Add("Brand");
            dt.Columns.Add("Color");
            dt.Columns.Add("VehicleType");
            dt.Columns.Add("ClientObject", typeof(Client));    // Add this
            dt.Columns.Add("VehicleObject", typeof(Vehicle));  // Add this

            foreach (var v in vehicles)
            {
                dt.Rows.Add(
                    _currentClient.ClientID,
                    _currentClient.Name,
                    _currentClient.CpNumber,
                    v.VehicleID,
                    v.PlateNumber,
                    v.Brand,
                    v.Color,
                    v.VehicleType,
                    _currentClient, // Set the actual Client object
                    v               // Set the actual Vehicle object
                );
            }

            dgvVehicles.DataSource = dt;
        }

        private void dgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvVehicles.SelectedRows[0];

                _selectedClient = selectedRow.Cells["ClientObject"].Value as Client;
                _selectedVehicle = selectedRow.Cells["VehicleObject"].Value as Vehicle;
            }
            else
            {
                _selectedClient = null;
                _selectedVehicle = null;

                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_scheduledVehicles.Count == 0)
            {
                MessageBox.Show("No vehicles scheduled. Please add at least one.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var sched in _scheduledVehicles)
            {
                var newSession = new VehicleSession
                {
                    VehicleID = sched.vehicle.VehicleID,
                    DurationType = sched.durationType,
                    StartDate = sched.startDate,
                    EndDateTime = sched.endDateTime,
                    TotalAmount = sched.totalAmount
                };
                _context.VehicleSessions.Add(newSession);
            }

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("All scheduled vehicles saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFormForNewEntry();
                btnNext.Enabled = true; // Enable btnNext only after successful save
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Error saving rental sessions: " + (ex.InnerException?.Message ?? ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNext.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rental sessions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNext.Enabled = false;
            }
            await RefreshScheduledListView();
            dgvVehicles.ClearSelection();
            _selectedClient = null;
            _selectedVehicle = null;
        }

        // Renamed and modified to store values internally, not display on labels
        private async Task<bool> CalculateAndSetRentalDetailsForSave()
        {
            if (_selectedVehicle == null || cmbDurationType.SelectedItem == null || cbTime.SelectedItem == null)
            {
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return false;
            }

            // Start at midnight, end at selected time
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today.AddHours(cbTime.SelectedIndex + 1);

            string durationType = cmbDurationType.SelectedItem.ToString();
            string vehicleType = _selectedVehicle.VehicleType?.Trim();

            if (endDate <= startDate)
            {
                MessageBox.Show("End time must be after start time.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Query the Fee table for the fixed price
            var fee = await _context.Fees
                .FirstOrDefaultAsync(f => f.VehicleType == vehicleType && f.DurationType == durationType);

            if (fee == null)
            {
                MessageBox.Show($"No fixed price found for {vehicleType} - {durationType}.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return false;
            }

            // Calculate the number of hours selected
            double selectedHours = (endDate - startDate).TotalHours;
            if (selectedHours <= 0) selectedHours = 1; // Minimum 1 hour

            // 24 hours is always 100% of the fee, regardless of duration type
            double percentage = selectedHours / 24.0;
            if (percentage > 1) percentage = 1; // Cap at 100%

            _calculatedEndDateTime = endDate;
            _calculatedTotalAmount = fee.FixedPrice * (decimal)percentage;

            return true;
        }

        private void ClearFormForNewEntry()
        {
            dgvVehicles.DataSource = null; // Clear DGV results
            _selectedClient = null;
            _selectedVehicle = null;
            dtpDateStart.Value = DateTime.Now; // Reset to current date
            cmbDurationType.SelectedIndex = 0; // Default to Weekly

            _calculatedEndDateTime = DateTime.MinValue;
            _calculatedTotalAmount = 0;
        }

        private void ParkRental_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchCancellationTokenSource?.Cancel(); // Cancel any pending search operation
            _searchCancellationTokenSource?.Dispose(); // Dispose the token source
            _context?.Dispose(); // Dispose the DbContext
        }



        private async void btnSetSched_Click(object sender, EventArgs e)
        {
            if (_selectedClient == null || _selectedVehicle == null)
            {
                MessageBox.Show("Please select a client and their vehicle first from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbDurationType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Duration Type.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            // Check if the vehicle already has a schedule in the database
            var alreadyScheduled = await _context.VehicleSessions
                .AnyAsync(vs => vs.VehicleID == _selectedVehicle.VehicleID);

            if (alreadyScheduled)
            {
                MessageBox.Show("This vehicle already has a schedule.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Prevent duplicate scheduling in the current session
            if (_scheduledVehicles.Any(x => x.vehicle.VehicleID == _selectedVehicle.VehicleID))
            {
                MessageBox.Show("This vehicle is already scheduled in this session.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Calculate rental details for this vehicle
            bool calculationSuccess = await CalculateAndSetRentalDetailsForSave();
            if (!calculationSuccess)
                return;

            // Add to the scheduled list
            _scheduledVehicles.Add((
                _selectedClient,
                _selectedVehicle,
                cmbDurationType.SelectedItem.ToString(),
                DateTime.Today,
                DateTime.Today.AddHours(cbTime.SelectedIndex + 1),
                _calculatedTotalAmount
            ));

            RefreshScheduledListView();
            dgvVehicles.ClearSelection();
            _selectedClient = null;
            _selectedVehicle = null;
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            _scheduledVehicles.Clear();
            RefreshScheduledListView();
            ClearFormForNewEntry();

            await UpdateNextButtonStateAsync();
        }

        private async Task RefreshScheduledListView()
        {
            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Vehicle");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("TimeStart");
            dt.Columns.Add("TimeEnd");
            dt.Columns.Add("TotalAmount", typeof(decimal));

            // 1. Add already saved schedules from the database
            var saved = await _context.VehicleSessions
                .Join(_context.Vehicles, vs => vs.VehicleID, v => v.VehicleID, (vs, v) => new { vs, v })
                .Join(_context.Clients, temp => temp.v.ClientID, c => c.ClientID, (temp, c) => new
                {
                    ClientID = c.ClientID,
                    ClientName = c.Name,
                    Vehicle = temp.v.Brand + " " + temp.v.PlateNumber,
                    DurationType = temp.vs.DurationType,
                    TimeStart = temp.vs.StartDate,
                    TimeEnd = temp.vs.EndDateTime,
                    TotalAmount = temp.vs.TotalAmount
                })
                .ToListAsync();

            foreach (var sched in saved)
            {
                dt.Rows.Add(
                    sched.ClientID,
                    sched.ClientName,
                    sched.Vehicle,
                    sched.DurationType,
                    sched.TimeStart.ToString("yyyy-MM-dd HH:mm"),
                    sched.TimeEnd.ToString("yyyy-MM-dd HH:mm"),
                    sched.TotalAmount
                );
            }

            // 2. Add unsaved scheduled vehicles (avoid duplicates)
            foreach (var sched in _scheduledVehicles)
            {
                // Check if this vehicle is already in the saved list (by VehicleID and time overlap)
                bool alreadySaved = saved.Any(s =>
                    s.Vehicle == $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}" &&
                    s.TimeStart == sched.startDate &&
                    s.TimeEnd == sched.endDateTime
                );
                if (!alreadySaved)
                {
                    dt.Rows.Add(
                        sched.client.ClientID,
                        sched.client.Name,
                        $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}",
                        sched.durationType,
                        sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                        sched.endDateTime.ToString("yyyy-MM-dd HH:mm"),
                        sched.totalAmount
                    );
                }
            }

            dgvList.DataSource = dt;
        }

        private async Task LoadScheduledVehiclesToListViewAsync()
        {
            var scheduled = await _context.VehicleSessions
                .Join(_context.Vehicles, vs => vs.VehicleID, v => v.VehicleID, (vs, v) => new { vs, v })
                .Join(_context.Clients, temp => temp.v.ClientID, c => c.ClientID, (temp, c) => new
                {
                    ClientID = c.ClientID,
                    ClientName = c.Name,
                    Vehicle = temp.v.Brand + " " + temp.v.PlateNumber,
                    DurationType = temp.vs.DurationType,
                    TimeStart = temp.vs.StartDate,
                    TimeEnd = temp.vs.EndDateTime,
                    TotalAmount = temp.vs.TotalAmount
                })
                .ToListAsync();

            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Vehicle");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("TimeStart");
            dt.Columns.Add("TimeEnd");
            dt.Columns.Add("TotalAmount", typeof(decimal));

            foreach (var sched in scheduled)
            {
                dt.Rows.Add(
                    sched.ClientID,
                    sched.ClientName,
                    sched.Vehicle,
                    sched.DurationType,
                    sched.TimeStart.ToString("yyyy-MM-dd HH:mm"),
                    sched.TimeEnd.ToString("yyyy-MM-dd HH:mm"),
                    sched.TotalAmount
                );
            }

            dgvList.DataSource = dt;
        }

        public static void ShowParkRentalForm(Client currentClient)
        {
            var parkRentalForm = new ParkRental(currentClient);
            parkRentalForm.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var TotalForm = new TotalPayment();
                homePage.ShowFormInPanel(TotalForm);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var SlotForm = new ParkingSlot();
                homePage.ShowFormInPanel(SlotForm);
            }
        }

     

        private async void ParkRental_Load_1(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today.AddHours(cbTime.SelectedIndex + 1);

            await UpdateNextButtonStateAsync();
            await LoadScheduledVehiclesToListViewAsync();
            await RefreshScheduledListView();
        }

        private void PopulateTimeComboBox()
        {
            cbTime.Items.Clear();
            for (int hour = 1; hour <= 23; hour++)
            {
                string timeLabel = DateTime.Today.AddHours(hour).ToString("HH:mm");
                cbTime.Items.Add(timeLabel);
            }
            cbTime.Items.Add("24:00"); // Explicitly add "24:00" as the last item
            cbTime.SelectedIndex = 0; // Default to first time slot
        }
        private async Task RefreshScheduledListViewAsync()
        {
            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Vehicle");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("TimeStart");
            dt.Columns.Add("TimeEnd");
            dt.Columns.Add("TotalAmount", typeof(decimal));

            // 1. Add already saved schedules from the database
            var saved = await _context.VehicleSessions
                .Join(_context.Vehicles, vs => vs.VehicleID, v => v.VehicleID, (vs, v) => new { vs, v })
                .Join(_context.Clients, temp => temp.v.ClientID, c => c.ClientID, (temp, c) => new
                {
                    ClientID = c.ClientID,
                    ClientName = c.Name,
                    Vehicle = temp.v.Brand + " " + temp.v.PlateNumber,
                    DurationType = temp.vs.DurationType,
                    TimeStart = temp.vs.StartDate,
                    TimeEnd = temp.vs.EndDateTime,
                    TotalAmount = temp.vs.TotalAmount
                })
                .ToListAsync();

            foreach (var sched in saved)
            {
                dt.Rows.Add(
                    sched.ClientID,
                    sched.ClientName,
                    sched.Vehicle,
                    sched.DurationType,
                    sched.TimeStart.ToString("yyyy-MM-dd HH:mm"),
                    sched.TimeEnd.ToString("yyyy-MM-dd HH:mm"),
                    sched.TotalAmount
                );
            }

            // 2. Add unsaved scheduled vehicles (avoid duplicates)
            foreach (var sched in _scheduledVehicles)
            {
                // Check if this vehicle is already in the saved list (by VehicleID and time overlap)
                bool alreadySaved = saved.Any(s =>
                    s.Vehicle == $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}" &&
                    s.TimeStart == sched.startDate &&
                    s.TimeEnd == sched.endDateTime
                );
                if (!alreadySaved)
                {
                    dt.Rows.Add(
                        sched.client.ClientID,
                        sched.client.Name,
                        $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}",
                        sched.durationType,
                        sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                        sched.endDateTime.ToString("yyyy-MM-dd HH:mm"),
                        sched.totalAmount
                    );
                }
            }

            dgvList.DataSource = dt;
        }
    }
}
