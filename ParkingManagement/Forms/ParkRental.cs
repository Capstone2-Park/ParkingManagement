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

            SetupDurationTypeComboBox();
            PopulateTimeComboBox();
            // Wire up the selection changed event
            cbVehicle.SelectedIndexChanged += cbVehicle_SelectedIndexChanged;


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

        }

        private void LoadClientVehicles()
        {
            if (_currentClient == null)
            {
                cbVehicle.DataSource = null;
                return;
            }

            // Get all vehicles for the current client
            var vehicles = _context.Vehicles
                .Where(v => v.ClientID == _currentClient.ClientID)
                .ToList();

            // Get IDs of vehicles already scheduled in the database
            var scheduledVehicleIdsDb = _context.VehicleSessions
                .Select(vs => vs.VehicleID)
                .ToList();

            // Get IDs of vehicles already scheduled in the current session
            var scheduledVehicleIdsSession = _scheduledVehicles
                .Select(s => s.vehicle.VehicleID)
                .ToList();

            // Filter out vehicles that are already scheduled
            var availableVehicles = vehicles
                .Where(v => !scheduledVehicleIdsDb.Contains(v.VehicleID) && !scheduledVehicleIdsSession.Contains(v.VehicleID))
                .ToList();

            cbVehicle.DataSource = availableVehicles;
            cbVehicle.DisplayMember = "PlateNumber";
            cbVehicle.ValueMember = "VehicleID";
            cbVehicle.SelectedIndex = -1;
        }

        // Update the selection changed event handler


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
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Error saving rental sessions: " + (ex.InnerException?.Message ?? ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rental sessions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await RefreshScheduledListView();

            _selectedClient = null;
            _selectedVehicle = null;
            // After successful save:
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var TotalForm = new TotalPayment();
                homePage.ShowFormInPanel(TotalForm);
            }
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

            // Combine date and time
            DateTime startDate = dtpDateStart.Value.Date + dtpTime.Value.TimeOfDay;
            int hoursPerDay = cbTime.SelectedIndex + 1;
            DateTime endDate = startDate.AddHours(hoursPerDay);

            string durationType = cmbDurationType.SelectedItem.ToString();
            string vehicleType = _selectedVehicle.VehicleType?.Trim();

            if (endDate <= startDate)
            {
                MessageBox.Show("End time must be after start time.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var fee = await _context.Fees
                .FirstOrDefaultAsync(f => f.VehicleType == vehicleType && f.DurationType == durationType);

            if (fee == null)
            {
                MessageBox.Show($"No fixed price found for {vehicleType} - {durationType}.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return false;
            }

            double selectedHours = hoursPerDay;
            if (selectedHours <= 0) selectedHours = 1;

            double percentage = selectedHours / 24.0;
            if (percentage > 1) percentage = 1;

            _calculatedEndDateTime = endDate;
            _calculatedTotalAmount = fee.FixedPrice * (decimal)percentage;

            return true;
        }

        private void ClearFormForNewEntry()
        {
            cbVehicle.SelectedIndex = -1;
            _selectedVehicle = null;
            dtpDateStart.Value = DateTime.Now;
            dtpTime.Value = DateTime.Now;
            cmbDurationType.SelectedIndex = 0;
            _calculatedEndDateTime = DateTime.MinValue;
            _calculatedTotalAmount = 0;
            lblVSelect.Text = "Vehicle Selection";
        }

        private void ParkRental_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchCancellationTokenSource?.Cancel(); // Cancel any pending search operation
            _searchCancellationTokenSource?.Dispose(); // Dispose the token source
            _context?.Dispose(); // Dispose the DbContext
        }



        private async void btnSetSched_Click(object sender, EventArgs e)
        {
            if (_currentClient == null || _selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle first from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // Combine date and time for scheduling
            DateTime startDate = dtpDateStart.Value.Date + dtpTime.Value.TimeOfDay;
            DateTime endDate = startDate.AddHours(cbTime.SelectedIndex + 1);

            _scheduledVehicles.Add((
                _currentClient,
                _selectedVehicle,
                cmbDurationType.SelectedItem.ToString(),
                startDate,
                endDate,
                _calculatedTotalAmount
));

            await RefreshScheduledListView();
            cbVehicle.SelectedIndex = -1;
            _selectedVehicle = null;
            lblVSelect.Text = "Vehicle Selection";
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Do you want to cancel the progress?",
                "Cancel Progress",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK)
                return;

            using (var db = new ParkingDbContext())
            {
                // Get the latest client (by highest ClientID)
                var latestClient = await db.Clients
                    .OrderByDescending(c => c.ClientID)
                    .FirstOrDefaultAsync();

                if (latestClient != null)
                {
                    // Get all vehicles for this client
                    var vehicles = await db.Vehicles
                        .Where(v => v.ClientID == latestClient.ClientID)
                        .ToListAsync();

                    var vehicleIds = vehicles.Select(v => v.VehicleID).ToList();

                    // For each slot where one of the client's vehicles is parked, reset statuses
                    var slotsToUpdate = await db.Parkingslot
                        .Where(s => vehicleIds.Contains(s.VehicleID))
                        .ToListAsync();

                    foreach (var slot in slotsToUpdate)
                    {
                        slot.VehicleStatus = "Not Parked";
                        slot.SlotStatus = "Available";
                        // Do NOT delete SlotNumber
                        slot.VehicleID = null;
                        slot.ClientID = null;
                        db.Parkingslot.Update(slot);
                    }

                    // Remove vehicles
                    if (vehicles.Any())
                    {
                        db.Vehicles.RemoveRange(vehicles);
                    }

                    // Remove client
                    db.Clients.Remove(latestClient);

                    await db.SaveChangesAsync();
                }
            }

            // Navigate to ClientManagement
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var clientManagementForm = new ClientManagement();
                homePage.ShowFormInPanel(clientManagementForm);
            }
        }

        private async Task RefreshScheduledListView()
        {
            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Vehicle");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("HoursPerDay");
            dt.Columns.Add("TimeStart");
            dt.Columns.Add("TimeEnd");
            dt.Columns.Add("TotalAmount", typeof(decimal));

            // 1. Add already saved schedules from the database for the current client only
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
                .Where(s => s.ClientID == _currentClient.ClientID)
                .ToListAsync();

            foreach (var sched in saved)
            {
                int hoursPerDay = (int)(sched.TimeEnd - sched.TimeStart).TotalHours;

                dt.Rows.Add(
                    sched.ClientID,
                    sched.ClientName,
                    sched.Vehicle,
                    sched.DurationType,
                    hoursPerDay,
                    sched.TimeStart.ToString("yyyy-MM-dd HH:mm"),
                    sched.TimeEnd.ToString("yyyy-MM-dd HH:mm"),
                    sched.TotalAmount
                );
            }

            // 2. Add unsaved scheduled vehicles for the current client only (avoid duplicates)
            foreach (var sched in _scheduledVehicles.Where(s => s.client.ClientID == _currentClient.ClientID))
            {
                bool alreadySaved = saved.Any(s =>
                    s.Vehicle == $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}" &&
                    s.TimeStart == sched.startDate &&
                    s.TimeEnd == sched.endDateTime
                );
                if (!alreadySaved)
                {
                    int hoursPerDay = (int)(sched.endDateTime - sched.startDate).TotalHours;

                    dt.Rows.Add(
                        sched.client.ClientID,
                        sched.client.Name,
                        $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}",
                        sched.durationType,
                        hoursPerDay,
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
            dt.Columns.Add("HoursPerDay"); // <-- New column
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


            await LoadScheduledVehiclesToListViewAsync();
            await RefreshScheduledListView();

        }

        private void PopulateTimeComboBox()
        {
            cbTime.Items.Clear();
            for (int hour = 1; hour <= 23; hour++)
            {
                string timeLabel = DateTime.Today.AddHours(hour).ToString("HH");
                cbTime.Items.Add(timeLabel + "-hour/s");
            }
            cbTime.Items.Add("24-hour/s"); // Explicitly add "24hours" as the last item
            cbTime.SelectedIndex = 0; // Default to first time slot
        }
        private async Task RefreshScheduledListViewAsync()
        {
            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Vehicle");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("HoursPerDay"); // <-- New column
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
                // For saved schedules, you may not have HoursPerDay, so you can calculate it:
                int hoursPerDay = (int)(sched.TimeEnd - sched.TimeStart).TotalHours;

                dt.Rows.Add(
                    sched.ClientID,
                    sched.ClientName,
                    sched.Vehicle,
                    sched.DurationType,
                    hoursPerDay,
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
                    // For unsaved scheduled vehicles:
                    int hoursPerDay = (int)(sched.endDateTime - sched.startDate).TotalHours;

                    dt.Rows.Add(
                        sched.client.ClientID,
                        sched.client.Name,
                        $"{sched.vehicle.Brand} {sched.vehicle.PlateNumber}",
                        sched.durationType,
                        hoursPerDay,
                        sched.startDate.ToString("yyyy-MM-dd HH:mm"),
                        sched.endDateTime.ToString("yyyy-MM-dd HH:mm"),
                        sched.totalAmount
                    );
                }
            }

            dgvList.DataSource = dt;
        }

        private void cbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedVehicle = cbVehicle.SelectedItem as Vehicle;
            if (_selectedVehicle != null)
                lblVSelect.Text = _selectedVehicle.PlateNumber;
            else
                lblVSelect.Text = "Vehicle Selection";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
