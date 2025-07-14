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

        private DateTime _calculatedEndDateTime;
        private decimal _calculatedTotalAmount;

        // ADD THIS LINE:
        private CancellationTokenSource _searchCancellationTokenSource;

        private List<(Client client, Vehicle vehicle, string durationType, DateTime startDate, DateTime endDateTime, decimal totalAmount)> _scheduledVehicles = new();

        public ParkRental()
        {
            InitializeComponent();
            InitializeDbContext();
            InitializeDataGridView();
            SetupDurationTypeComboBox();

            // Initialize the CancellationTokenSource here
            _searchCancellationTokenSource = new CancellationTokenSource();
        }

        private void InitializeDbContext()
        {
            _context = new ParkingDbContext();
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

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _searchCancellationTokenSource.Token;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                dgvVehicles.DataSource = null;
                _selectedClient = null;
                _selectedVehicle = null;
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return;
            }

            try
            {
                await Task.Delay(300, cancellationToken);

                // Get all vehicle IDs that have an active session
                var activeVehicleIds = await _context.VehicleSessions
                    .Where(s => !string.IsNullOrEmpty(s.DurationType) && s.TotalAmount > 0)
                    .Select(s => s.VehicleID)
                    .ToListAsync(cancellationToken);

                var results = await _context.Clients
                    .Include(c => c.VehicleList)
                    .Where(c => c.Name.Contains(searchTerm) || c.ClientID.Contains(searchTerm))
                    .SelectMany(c => c.VehicleList
                        .Where(v => !activeVehicleIds.Contains(v.VehicleID)) // Exclude vehicles with active session
                        .Select(v => new
                        {
                            c.ClientID,
                            ClientName = c.Name,
                            ClientContact = c.CpNumber,
                            v.VehicleID,
                            v.PlateNumber,
                            v.Brand,
                            v.Color,
                            v.VehicleType,
                            ClientObject = c,
                            VehicleObject = v
                        }))
                    .ToListAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                DataTable dt = new DataTable();
                dt.Columns.Add("ClientID");
                dt.Columns.Add("ClientName");
                dt.Columns.Add("ClientContact");
                dt.Columns.Add("VehicleID");
                dt.Columns.Add("PlateNumber");
                dt.Columns.Add("Brand");
                dt.Columns.Add("Color");
                dt.Columns.Add("VehicleType");
                dt.Columns.Add("ClientObject", typeof(Client));
                dt.Columns.Add("VehicleObject", typeof(Vehicle));

                foreach (var item in results)
                {
                    dt.Rows.Add(item.ClientID, item.ClientName, item.ClientContact,
                                item.VehicleID, item.PlateNumber, item.Brand,
                                item.Color, item.VehicleType, item.ClientObject, item.VehicleObject);
                }

                dgvVehicles.DataSource = dt;
            }
            catch (OperationCanceledException)
            {
                // Do nothing: this is expected when cancelling previous searches
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching clients: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                // Clear the list and UI
                _scheduledVehicles.Clear();
                RefreshScheduledListView();
                ClearFormForNewEntry();

                // Navigate to ParkingSlot
                var homePage = this.ParentForm as HomePage;
                if (homePage != null)
                {
                    var ParkingSlotForm = new ParkingSlot();
                    homePage.ShowFormInPanel(ParkingSlotForm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rental sessions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Renamed and modified to store values internally, not display on labels
        private async Task<bool> CalculateAndSetRentalDetailsForSave()
        {
            if (_selectedVehicle == null || cmbDurationType.SelectedItem == null)
            {
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return false;
            }

            DateTime startDate = dtpDateStart.Value.Date;
            string durationType = cmbDurationType.SelectedItem.ToString();
            string vehicleType = _selectedVehicle.VehicleType?.Trim();

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

            // Calculate EndDateTime based on duration type
            switch (durationType)
            {
                case "Daily":
                    _calculatedEndDateTime = startDate.AddDays(1);
                    break;
                case "Weekly":
                    _calculatedEndDateTime = startDate.AddDays(7);
                    break;
                case "Monthly":
                    _calculatedEndDateTime = startDate.AddMonths(1);
                    break;
                case "Yearly":
                    _calculatedEndDateTime = startDate.AddYears(1);
                    break;
                default:
                    MessageBox.Show("Invalid duration type selected.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _calculatedEndDateTime = DateTime.MinValue;
                    _calculatedTotalAmount = 0;
                    return false;
            }

            _calculatedTotalAmount = fee.FixedPrice;
            return true;
        }

        private void ClearFormForNewEntry()
        {
            txtSearch.Clear();
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

        private async Task<List<Vehicle>> GetAvailableVehiclesForClientAsync(string clientId)
        {
            // Get all vehicles for the client
            var vehicles = await _context.Vehicles
                .Where(v => v.ClientID == clientId)
                .ToListAsync();

            // Get all vehicle IDs that have an active session
            var activeVehicleIds = await _context.VehicleSessions
                .Where(s => !string.IsNullOrEmpty(s.DurationType) && s.TotalAmount > 0)
                .Select(s => s.VehicleID)
                .ToListAsync();

            // Filter out vehicles with active sessions
            return vehicles.Where(v => !activeVehicleIds.Contains(v.VehicleID)).ToList();
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

            // Calculate rental details for this vehicle
            bool calculationSuccess = await CalculateAndSetRentalDetailsForSave();
            if (!calculationSuccess)
                return;

            // Prevent duplicate scheduling
            if (_scheduledVehicles.Any(x => x.vehicle.VehicleID == _selectedVehicle.VehicleID))
            {
                MessageBox.Show("This vehicle is already scheduled.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add to the scheduled list
            _scheduledVehicles.Add((
                _selectedClient,
                _selectedVehicle,
                cmbDurationType.SelectedItem.ToString(),
                dtpDateStart.Value.Date,
                _calculatedEndDateTime,
                _calculatedTotalAmount
            ));

            // Optionally, show the scheduled list in a DataGridView or ListBox for user feedback
            RefreshScheduledListView();

            // Optionally, clear selection for next entry
            dgvVehicles.ClearSelection();
            _selectedClient = null;
            _selectedVehicle = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _scheduledVehicles.Clear();
            RefreshScheduledListView();
            ClearFormForNewEntry();
        }

        private void RefreshScheduledListView()
        {
            // Example for a ListBox named lstScheduled
            lstScheduled.Items.Clear();
            foreach (var sched in _scheduledVehicles)
            {
                lstScheduled.Items.Add($"{sched.vehicle.PlateNumber} - {sched.durationType} - ₱{sched.totalAmount:N2}");
            }
        }
    }
}
