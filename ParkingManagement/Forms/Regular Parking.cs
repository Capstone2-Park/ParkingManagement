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
            _searchCancellationTokenSource = new CancellationTokenSource();
            _qrScanCancellationTokenSource = new CancellationTokenSource();
        }

        private async void Regular_Parking_Load(object sender, EventArgs e)
        {

            await GenerateNewRegularVehicleID();
            await LoadActiveParkingSessions();

            ClearInputFields();
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

        private async Task GenerateNewRegularVehicleID()
        {
            string newId = "RP000001";
            try
            {
                var lastSession = await _context.RegularParkingSessions
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

        private void ClearInputFields()
        {
            txtPlateNumber.Clear();

            // Keep txtVehicleId as it will be auto-generated on load or after save
            // cmbTypeOfVehicle.SelectedIndex is already set on load, can reset if needed: cmbTypeOfVehicle.SelectedIndex = 0;
        }

        private async void btnTimeIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
            {
                MessageBox.Show("Please enter the Plate Number.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbTypeOfVehicle.SelectedItem == null)
            {
                MessageBox.Show("Please select the Vehicle Type.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingSession = await _context.RegularParkingSessions
                                                .FirstOrDefaultAsync(s => s.PlateNumber == txtPlateNumber.Text && s.TimeOut == null);
            if (existingSession != null)
            {
                MessageBox.Show($"Vehicle with plate number '{txtPlateNumber.Text}' is already checked in. Session ID: {existingSession.RegularVehicleID}", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _currentTimeIn = DateTime.Now;
            string regularVehicleId = txtVehicleId.Text;

            RegularParkingSession newSession = new RegularParkingSession
            {
                RegularVehicleID = regularVehicleId,
                PlateNumber = txtPlateNumber.Text,
                VehicleType = cmbTypeOfVehicle.SelectedItem?.ToString() ?? string.Empty,
                TimeIn = _currentTimeIn,
                TimeOut = null,
                TotalAmount = null,
            };

            try
            {
                _context.RegularParkingSessions.Add(newSession);
                await _context.SaveChangesAsync(); // newSession.SessionID is set here

                MessageBox.Show("Vehicle checked in successfully!", "Check In Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadActiveParkingSessions();
                ClearInputFields();
                await GenerateNewRegularVehicleID();

                // Pass the new session's SessionID to RegularParkingSlot
                await _context.SaveChangesAsync();
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
                var parkSlotForm = new RegularParkingSlot(newSession.SessionID);
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

        private async Task ProcessScannedQRCodeDataAsync(string qrCodeData)
        {
            try
            {
                var dataLines = qrCodeData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var dataDictionary = dataLines.Select(line => line.Split(new[] { ':' }, 2))
                                              .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

                if (dataDictionary.TryGetValue("SessionID", out string sessionIdStr) && int.TryParse(sessionIdStr, out int sessionId))
                {
                    var session = await _context.RegularParkingSessions.FirstOrDefaultAsync(s => s.SessionID == sessionId && s.TimeOut == null);

                    if (session != null)
                    {
                        session.TimeOut = DateTime.Now;
                        TimeSpan duration = session.TimeOut.Value - session.TimeIn;

                        // Get the fee from the Fee table based on VehicleType
                        var fee = await _context.Fees.FirstOrDefaultAsync(f => f.VehicleType == session.VehicleType);
                        decimal hourlyRate = fee?.FixedPrice ?? 0m;

                        session.TotalAmount = (decimal)Math.Ceiling(duration.TotalHours) * hourlyRate;

                        _context.RegularParkingSessions.Update(session);
                        await _context.SaveChangesAsync();

                        MessageBox.Show(
                            $"Vehicle with Plate Number '{session.PlateNumber}' checked out successfully!\n" +
                            $"Time In: {session.TimeIn:MM/dd/yyyy hh:mm:ss tt}\n" +
                            $"Time Out: {session.TimeOut:MM/dd/yyyy hh:mm:ss tt}\n" +
                            $"Duration: {duration.TotalHours:F2} hours\n" +
                            $"Total Amount: {session.TotalAmount:N2} PHP",
                            "Check Out Success", MessageBoxButtons.OK, MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show("No active session found for the scanned QR code.", "QR Code Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid QR code data format. 'SessionID' is missing or not an integer.", "QR Code Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the QR code data: {ex.Message}", "QR Code Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
