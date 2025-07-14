using AForge.Video; 
using AForge.Video.DirectShow;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
using ParkingManagement.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; 
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement
{
    public partial class ClientManagement : Form
    {
        private ParkingDbContext _context;

        // --- Camera Variables ---
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        // --- Data Models for OOP ---
        private Client currentClient; // Represents the client data being worked on
        private DataTable vehicleDataTable; // Used to bind to DataGridView for current session vehicles
        private List<Vehicle> currentClientVehicles; // List to hold vehicle objects for the current client

        public ClientManagement()



        {
            InitializeComponent();
            InitializeCamera();
            InitializeVehicleDataTable(); // Sets up the DataTable for current session vehicles
            InitializeNewClient(); // This will initialize currentClient and call SetCurrentSessionVehiclesToDGV()

            // Set PictureBox SizeMode programmatically for camera feed
            pcbIDPic.SizeMode = PictureBoxSizeMode.Zoom;

            // Ensure dgvInformation starts by showing current session vehicles (empty initially)
            // This is handled by InitializeNewClient, but explicitly calling it here ensures initial state
            SetCurrentSessionVehiclesToDGV();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await GenerateNewClientID(); // Generate the first ClientID
            await PopulateVehicleTypesComboBox(); // Load vehicle types
            await SetNextAvailableVehicleIDDisplay(); // Set the initial Vehicle ID on form load

            // --- Initial UI State ---
            txtClientID.Enabled = false; // Client ID is always read-only
            txtVehicleIDNo.Enabled = false; // Vehicle ID is always read-only

            // Camera buttons
            btnCaptureImage.Enabled = false;
            btnRetakeImage.Enabled = false;

            // Vehicle related buttons (initially enabled to add to in-memory list)
            btnAddVehicle.Enabled = true; // Enabled from the start to allow adding vehicles to the list
            btnRemoveVehicle.Enabled = true; // Enabled from the start
            dgvInformation.Enabled = true; // Enabled from the start
        }

        // --- Camera Initialization ---
        private void InitializeCamera()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("No video input devices found.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnStartCamera.Enabled = false;
                }
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Error initializing camera devices: " + ex.Message, "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartCamera.Enabled = false;
            }
        }

        // --- DataGridView DataTable Initialization for current session vehicles ---
        private void InitializeVehicleDataTable()
        {
            vehicleDataTable = new DataTable();
            vehicleDataTable.Columns.Add("VehicleID", typeof(string));
            vehicleDataTable.Columns.Add("Brand", typeof(string));
            vehicleDataTable.Columns.Add("Color", typeof(string));
            vehicleDataTable.Columns.Add("PlateNumber", typeof(string));
            vehicleDataTable.Columns.Add("VehicleType", typeof(string));

        }

        // --- Helper: Initialize New Client Object and DbContext ---
        private void InitializeNewClient()
        {
            // Only dispose and create new context if it exists and needs reset
            if (_context != null)
            {
                _context.Dispose(); // Dispose previous context if it exists
            }
            _context = new ParkingDbContext(); // Create a new DbContext instance
            currentClient = new Client();
            currentClientVehicles = new List<Vehicle>(); // Reset vehicles list for new client session

            // Reset dgvInformation to show current session vehicles (which is now empty)
            SetCurrentSessionVehiclesToDGV();
        }

        // --- Helper: Generate New Client ID ---
        private async Task GenerateNewClientID()
        {
            string newClientID = "C0000001";
            try
            {
                var lastClient = await _context.Clients
                                               .Where(c => c.ClientID != null && c.ClientID.StartsWith("C") && c.ClientID.Length == 8)
                                               .OrderByDescending(c => c.ClientID)
                                               .FirstOrDefaultAsync();

                if (lastClient != null && lastClient.ClientID.StartsWith("C") && lastClient.ClientID.Length == 8)
                {
                    if (int.TryParse(lastClient.ClientID.Substring(1), out int lastIdNum))
                    {
                        lastIdNum++;
                        newClientID = "C" + lastIdNum.ToString("D7");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating new Client ID: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            currentClient.ClientID = newClientID;
            txtClientID.Text = newClientID;
        }



        // --- Helper: Generate Next Vehicle ID ---
        private async Task<string> GenerateNextVehicleID()
        {
            string newVehicleID = "V0000001";
            int maxIdNum = 0;

            try
            {
                var lastVehicleInDb = await _context.Vehicles
                                                     .Where(v => v.VehicleID != null && v.VehicleID.StartsWith("V") && v.VehicleID.Length == 8)
                                                     .OrderByDescending(v => v.VehicleID)
                                                     .FirstOrDefaultAsync();

                if (lastVehicleInDb != null && lastVehicleInDb.VehicleID.StartsWith("V") && lastVehicleInDb.VehicleID.Length == 8)
                {
                    if (int.TryParse(lastVehicleInDb.VehicleID.Substring(1), out int dbIdNum))
                    {
                        maxIdNum = dbIdNum;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting last vehicle ID from DB: " + ex.Message);
            }

            if (currentClientVehicles.Any())
            {
                var lastVehicleInMemory = currentClientVehicles
                                            .Where(v => v.VehicleID != null && v.VehicleID.StartsWith("V") && v.VehicleID.Length == 8)
                                            .OrderByDescending(v => v.VehicleID)
                                            .FirstOrDefault();

                if (lastVehicleInMemory != null && int.TryParse(lastVehicleInMemory.VehicleID.Substring(1), out int inMemoryIdNum))
                {
                    if (inMemoryIdNum > maxIdNum)
                    {
                        maxIdNum = inMemoryIdNum;
                    }
                }
            }

            maxIdNum++;
            newVehicleID = "V" + maxIdNum.ToString("D7");
            return newVehicleID;
        }


        // --- Helper Method: Set Next Available Vehicle ID in Textbox ---
        private async Task SetNextAvailableVehicleIDDisplay()
        {
            txtVehicleIDNo.Text = await GenerateNextVehicleID();
        }

        // Fix for CS0103: The name 'vehicles' does not exist in the current context
        private async Task PopulateVehicleTypesComboBox()
        {
            try
            {
                // Assuming 'vehicles' is intended to refer to a collection of Vehicle objects in the database
                var vehicleTypes = await _context.Vehicles
                                                 .Select(v => v.VehicleType)
                                                 .Distinct()
                                                 .ToListAsync();

                cmbVehicleType.DataSource = vehicleTypes;

                if (cmbVehicleType.Items.Count > 0)
                {
                    cmbVehicleType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicle types: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            if (videoDevices == null || videoDevices.Count == 0)
            {
                MessageBox.Show("No video input devices found.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                pcbIDPic.Image = null; // Clear image
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSource.Start();

            btnStartCamera.Text = "Stop Camera";
            btnStartCamera.Click -= btnStartCamera_Click;
            btnStartCamera.Click += btnStopCamera_Click;
            btnCaptureImage.Enabled = true;
            btnRetakeImage.Enabled = false;
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= videoSource_NewFrame; // Detach event handler
            }
            pcbIDPic.Image = null; // Clear the camera feed from picture box

            btnStartCamera.Text = "Start Camera";
            btnStartCamera.Click -= btnStopCamera_Click;
            btnStartCamera.Click += btnStartCamera_Click;
            btnCaptureImage.Enabled = false;
            btnRetakeImage.Enabled = false;
        }

        // --- New Frame Event Handler for Camera ---
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap currentFrame = (Bitmap)eventArgs.Frame.Clone();
            if (pcbIDPic.Image != null)
            {
                pcbIDPic.Image.Dispose();
            }
            pcbIDPic.Image = currentFrame;
        }


        // --- HELPER: Save Image to Disk ---
        private string SaveImageToDisk(Image img)
        {
            if (img == null)
            {
                return string.Empty; // Return empty string if no image to save
            }

            string dir = Path.Combine(Application.StartupPath, "ClientImages");
            Directory.CreateDirectory(dir);
            string fn = $"{Guid.NewGuid()}.jpg";
            string fullPath = Path.Combine(dir, fn);

            try
            {
                img.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                return fullPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image to disk: {ex.Message}", "Image Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }


        private async void btnAddVehicle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBrand.Text) ||
                string.IsNullOrWhiteSpace(txtColor.Text) ||
                string.IsNullOrWhiteSpace(txtPlateNo.Text) ||
                cmbVehicleType.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all vehicle details.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Vehicle newVehicle = new Vehicle
            {
                VehicleID = txtVehicleIDNo.Text,
                Brand = txtBrand.Text,
                Color = txtColor.Text,
                PlateNumber = txtPlateNo.Text,
                VehicleType = cmbVehicleType.SelectedItem.ToString(),
                ClientID = currentClient.ClientID // Still link to current client in memory
            };

            currentClientVehicles.Add(newVehicle);

            // Clear and re-populate the DataTable to reflect changes for the current session
            vehicleDataTable.Rows.Clear();
            foreach (var vehicle in currentClientVehicles)
            {
                DataRow newRow = vehicleDataTable.NewRow();
                newRow["VehicleID"] = vehicle.VehicleID;
                newRow["Brand"] = vehicle.Brand;
                newRow["Color"] = vehicle.Color;
                newRow["PlateNumber"] = vehicle.PlateNumber;
                newRow["VehicleType"] = vehicle.VehicleType;

                vehicleDataTable.Rows.Add(newRow);
            }
            // The dgvInformation is already bound to vehicleDataTable, so it updates automatically.

            MessageBox.Show($"Vehicle {newVehicle.VehicleID} added to list. Click Save All to commit.", "Vehicle Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearVehicleInputFields();
            await SetNextAvailableVehicleIDDisplay();
        }

        // --- Helper to clear vehicle input fields ---
        private void ClearVehicleInputFields()
        {
            txtVehicleIDNo.Clear();
            txtBrand.Clear();
            txtColor.Clear();
            txtPlateNo.Clear();
            cmbVehicleType.SelectedIndex = (cmbVehicleType.Items.Count > 0) ? 0 : -1;
        }

        private async void btnRemoveVehicle_Click(object sender, EventArgs e)
        {
            if (dgvInformation.SelectedRows.Count > 0)
            {
                // Ensure dgvInformation is showing current session vehicles
                if (dgvInformation.DataSource != vehicleDataTable)
                {
                    MessageBox.Show("Please switch to current session view (e.g., after clicking 'Cancel' for a new client entry) to remove vehicles from the list.", "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string vehicleIdToRemove = dgvInformation.SelectedRows[0].Cells["VehicleID"].Value.ToString();

                currentClientVehicles.RemoveAll(v => v.VehicleID == vehicleIdToRemove);
                dgvInformation.Rows.RemoveAt(dgvInformation.SelectedRows[0].Index); // Remove from DataGridView's visual representation

                MessageBox.Show($"Vehicle {vehicleIdToRemove} removed from list.", "Vehicle Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await SetNextAvailableVehicleIDDisplay(); // Update ID display after removal
            }
            else
            {
                MessageBox.Show("Please select a vehicle to remove.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            currentClient.Name = txtName.Text;
            currentClient.Address = txtAddress.Text;
            currentClient.CpNumber = txtContactNo.Text;

            if (string.IsNullOrWhiteSpace(currentClient.Name) ||
                string.IsNullOrWhiteSpace(currentClient.Address) ||
                string.IsNullOrWhiteSpace(currentClient.CpNumber) ||
                !Regex.IsMatch(currentClient.CpNumber, @"^\d{11}$"))
            {
                MessageBox.Show("Please fill in all client details correctly (contact number must be 11 digits).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pcbIDPic.Image == null)
            {
                MessageBox.Show("Please capture an ID picture.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string imagePathToSave = string.Empty;

            try
            {
                var existingClient = await _context.Clients.FindAsync(currentClient.ClientID);

                if (existingClient == null)
                {
                    imagePathToSave = SaveImageToDisk(pcbIDPic.Image);
                    currentClient.IDPicture = imagePathToSave;
                    await _context.Clients.AddAsync(currentClient);
                }
                else
                {
                    existingClient.Name = currentClient.Name;
                    existingClient.Address = currentClient.Address;
                    existingClient.CpNumber = currentClient.CpNumber;

                    if (pcbIDPic.Image != null)
                    {
                        if (!string.IsNullOrEmpty(existingClient.IDPicture) && File.Exists(existingClient.IDPicture))
                        {
                            try { File.Delete(existingClient.IDPicture); } catch (Exception ex) { Console.WriteLine($"Warning: Could not delete old image file {existingClient.IDPicture}. Error: {ex.Message}"); }
                        }
                        imagePathToSave = SaveImageToDisk(pcbIDPic.Image);
                        existingClient.IDPicture = imagePathToSave;
                    }
                    _context.Clients.Update(existingClient);
                }

                foreach (var vehicle in currentClientVehicles)
                {
                    vehicle.ClientID = currentClient.ClientID;
                    var existingVehicle = await _context.Vehicles.FindAsync(vehicle.VehicleID);
                    if (existingVehicle == null)
                    {
                        await _context.Vehicles.AddAsync(vehicle);
                    }
                }

                await _context.SaveChangesAsync();
                MessageBox.Show("Client and all associated vehicle(s) saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- CHANGE HERE: Call ClearForm(true) to clear all inputs AND reset DGV to empty for new entry ---
                await ClearForm(true); // Clear input fields AND reset dgvInformation to empty for new client

                // After successful save:
                var homePage = this.ParentForm as HomePage;
                if (homePage != null)
                {
                    var parkRentalForm = new ParkRental();
                    homePage.ShowFormInPanel(parkRentalForm);
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Error saving data to database: " + ex.InnerException?.Message ?? ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(imagePathToSave))
                {
                    try { File.Delete(imagePathToSave); } catch { /* ignore */ }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(imagePathToSave))
                {
                    try { File.Delete(imagePathToSave); } catch { /* ignore */ }
                }
            }
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            await ClearForm(true); // Clear everything, including dgvInformation's content for new entry
        }

        // Added a parameter 'resetDGV' to control if dgvInformation should reset to empty or stay in "all data" view
        private async Task ClearForm(bool resetDGV)
        {
            // Client fields
            txtName.Clear();
            txtAddress.Clear();
            txtContactNo.Clear();
            pcbIDPic.Image = null;
            btnCaptureImage.Enabled = false;
            btnRetakeImage.Enabled = false;

            // Stop camera if running
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= videoSource_NewFrame;
            }
            btnStartCamera.Text = "Start Camera";
            btnStartCamera.Click -= btnStopCamera_Click;
            btnStartCamera.Click += btnStartCamera_Click;

            // Vehicle input fields
            ClearVehicleInputFields();

            // Re-initialize client & vehicle lists for a fresh start
            // This entire block (InitializeNewClient() and subsequent ID generation)
            // should ONLY run if we intend to clear the DGV for a new input session (resetDGV is true)
            if (resetDGV)
            {
                InitializeNewClient(); // This will also call SetCurrentSessionVehiclesToDGV()
                await GenerateNewClientID();
                await SetNextAvailableVehicleIDDisplay();
            }
            // else, if resetDGV is false, we don't do anything to the client or vehicle lists
            // This path (resetDGV == false) will now effectively not affect the DGV's content,
            // as it won't be called after save anymore for DGV clearing.

            // Reset UI for new client entry
            btnAddVehicle.Enabled = true;
            btnRemoveVehicle.Enabled = true;
            dgvInformation.Enabled = true; // Still enabled even if showing all data
        }



        private void btnCaptureImage_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= videoSource_NewFrame;

                btnStartCamera.Text = "Start Camera";
                btnStartCamera.Click -= btnStopCamera_Click;
                btnStartCamera.Click += btnStartCamera_Click;

                btnCaptureImage.Enabled = false;
                btnRetakeImage.Enabled = true;
                MessageBox.Show("Image captured.", "Capture Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Camera is not running.", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRetakeImage_Click(object sender, EventArgs e)
        {
            pcbIDPic.Image = null;
            btnCaptureImage.Enabled = true;
            btnRetakeImage.Enabled = false;
            btnStartCamera_Click(sender, e);
        }

        // --- NEW: Method to set dgvInformation to show only current session vehicles ---
        private void SetCurrentSessionVehiclesToDGV()
        {
            dgvInformation.DataSource = null; // Unbind first
            dgvInformation.DataSource = vehicleDataTable; // Bind to the DataTable for current session

            // Set column headers for the "Current Session Vehicles" view
            dgvInformation.Columns["VehicleID"].HeaderText = "Vehicle ID (Current)"; // Vehicle ID (Current) in Filipino
            dgvInformation.Columns["Brand"].HeaderText = "Brand";
            dgvInformation.Columns["Color"].HeaderText = "Color";
            dgvInformation.Columns["PlateNumber"].HeaderText = "Plate No.";
            dgvInformation.Columns["VehicleType"].HeaderText = "Type of Vehicle"; // Vehicle Type in Filipino


            // Clear any image column formatting from the "All Data" view
            foreach (DataGridViewColumn col in dgvInformation.Columns)
            {
                if (col is DataGridViewImageColumn)
                {
                    col.Visible = false; // Hide image column if it's not relevant for this view
                }
            }
            dgvInformation.RowTemplate.Height = 22; // Reset row height
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= videoSource_NewFrame;
            }

            if (_context != null)
            {
                _context.Dispose();
            }
        }

        private void cmbVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
