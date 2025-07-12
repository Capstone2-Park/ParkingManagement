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
            cmbDurationType.Items.Add("Weekly");
            cmbDurationType.Items.Add("Monthly");
            cmbDurationType.Items.Add("Yearly");
            cmbDurationType.SelectedIndex = 0; // Default to Weekly
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

                var results = await _context.Clients
                    .Include(c => c.VehicleList)
                    .Where(c => c.Name.Contains(searchTerm) || c.ClientID.Contains(searchTerm))
                    .SelectMany(c => c.VehicleList.Select(v => new
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
            if (_selectedClient == null || _selectedVehicle == null)
            {
                MessageBox.Show("Please select a client and their vehicle first from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure we have valid inputs for calculation
            if (cmbDurationType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Duration Type.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Perform the calculation just before saving
            bool calculationSuccess = await CalculateAndSetRentalDetailsForSave();
            if (!calculationSuccess)
            {
                // Message box will be shown by CalculateAndSetRentalDetailsForSave if something went wrong
                return;
            }

            // Now, _calculatedEndDateTime and _calculatedTotalAmount are set
            DateTime startDate = dtpDateStart.Value.Date;
            string durationType = cmbDurationType.SelectedItem.ToString();

            // Final validation before saving
            if (_calculatedEndDateTime == DateTime.MinValue || _calculatedTotalAmount <= 0)
            {
                MessageBox.Show("Rental calculation failed. Please check inputs.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate that the end date is not in the past relative to the current time,
            // or if it makes sense for a pre-booked future rental, validate against start time.
            if (_calculatedEndDateTime < startDate)
            {
                MessageBox.Show("The calculated end date/time is in the past. Please adjust start date or duration.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VehicleSession newSession = new VehicleSession
            {
                VehicleID = _selectedVehicle.VehicleID,
                DurationType = durationType,
                StartDate = startDate,
                EndDateTime = _calculatedEndDateTime, // Use the calculated value
                TotalAmount = _calculatedTotalAmount  // Use the calculated value
            };

            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var ParkingSlotForm = new ParkingSlot();
                homePage.ShowFormInPanel(ParkingSlotForm);
            }

            try
            {
                _context.VehicleSessions.Add(newSession);
                await _context.SaveChangesAsync();
                MessageBox.Show("Rental session saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFormForNewEntry(); // Reset the form for the next entry
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Error saving rental session: {dbEx.InnerException?.Message ?? dbEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Renamed and modified to store values internally, not display on labels
        private async Task<bool> CalculateAndSetRentalDetailsForSave()
        {
            if (_selectedVehicle == null || cmbDurationType.SelectedItem == null)
            {
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                // No message box here, let btnSaveRental handle it if inputs are missing
                return false;
            }

            DateTime startDate = dtpDateStart.Value.Date;
            string durationType = cmbDurationType.SelectedItem.ToString();

            decimal feePerHour = 0;

            try
            {
                feePerHour = await _context.Fees
                                        .Where(f => f.VehicleType == _selectedVehicle.VehicleType)
                                        .Select(f => f.FeePerHour)
                                        .FirstOrDefaultAsync();

                if (feePerHour == 0)
                {
                    MessageBox.Show($"Fee not found for vehicle type: {_selectedVehicle.VehicleType}. Please configure fees.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _calculatedEndDateTime = DateTime.MinValue;
                    _calculatedTotalAmount = 0;
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving fee: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _calculatedEndDateTime = DateTime.MinValue;
                _calculatedTotalAmount = 0;
                return false;
            }

            // Calculate EndDateTime and TotalAmount based on duration type
            switch (durationType)
            {
                case "Weekly":
                    _calculatedEndDateTime = startDate.AddDays(7);
                    _calculatedTotalAmount = feePerHour * 24 * 7;
                    break;
                case "Monthly":
                    _calculatedEndDateTime = startDate.AddMonths(1);
                    // For monthly, you might have a fixed monthly fee or use an average (e.g., 30 days)
                    _calculatedTotalAmount = feePerHour * 24 * 30; // Using 30 days for calculation
                    break;
                case "Yearly":
                    _calculatedEndDateTime = startDate.AddYears(1);
                    // For yearly, use 365 days or 366 for leap years if precision matters
                    _calculatedTotalAmount = feePerHour * 24 * 365; // Using 365 days for calculation
                    break;
                default:
                    MessageBox.Show("Invalid duration type selected.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _calculatedEndDateTime = DateTime.MinValue;
                    _calculatedTotalAmount = 0;
                    return false;
            }
            return true; // Calculation was successful
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
    }
}
