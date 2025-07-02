using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using static QRCoder.PayloadGenerator.ShadowSocksConfig;

namespace ParkingManagement.Forms
{
    public partial class Regular_Parking : Form
    {

        private ParkingDbContext _context;
        private CancellationTokenSource _searchCancellationTokenSource;
        private CancellationTokenSource _qrScanCancellationTokenSource;
        private DateTime _currentTimeIn;

        private VideoCaptureDevice _videoCaptureDevice;
        private FilterInfoCollection _videoDevices;
        private BarcodeReader _qrCodeReader;
        private bool _isScanningQRCode;

        public Regular_Parking()
        {
            InitializeComponent();
            InitializeDbContext();
            InitializeDataGridView();
            SetupVehicleTypeComboBox();
            _searchCancellationTokenSource = new CancellationTokenSource();
            _qrCodeReader = new BarcodeReader
            {
                AutoRotate = true, // Attempt to auto-rotate image for better detection
                Options = new DecodingOptions
                {
                    TryHarder = true, // Spend more time to find a code
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.QR_CODE } // Only look for QR codes
                }
            };
            _isScanningQRCode = false;
        }

        private async void Regular_Parking_Load(object sender, EventArgs e)
        {

            await GenerateNewRegularVehicleID();
            await LoadActiveParkingSessions();
            pcbQRcode.SizeMode = PictureBoxSizeMode.Zoom;

            ClearInputFields();

            // Initialize video devices for QR scanning only once
            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (_videoDevices.Count == 0)
                {
                    // This message is fine here as it's typically on the UI thread during form load
                    MessageBox.Show("No video input devices found on this system.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnTimeOut.Enabled = false; // Disable timeout button if no camera is available
                }
            }
            catch (Exception ex)
            {
                // Catch any errors during device enumeration (e.g., driver issues)
                MessageBox.Show($"Error initializing video devices: {ex.Message}", "Camera Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnTimeOut.Enabled = false;
            }
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
            dgvTimeInTimeOutData.Columns.Add(new DataGridViewTextBoxColumn { Name = "QRCodeData", HeaderText = "QR Data", DataPropertyName = "QRCodeData", ReadOnly = true, Visible = false });

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

        private void GenerateQRCode(string qrData)
        {
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        // You can set this to match PictureBox dimensions
                        int pixelPerModule = Math.Min(pcbQRcode.Width, pcbQRcode.Height) / qrCodeData.ModuleMatrix.Count;
                        pixelPerModule = Math.Max(1, Math.Min(pixelPerModule, 20)); // Clamp to reasonable range

                        Bitmap qrCodeImage = qrCode.GetGraphic(pixelPerModule);

                        // Resize image to PictureBox size (optional for exact fit)
                        Bitmap resizedImage = new Bitmap(qrCodeImage, pcbQRcode.Size);
                        pcbQRcode.Image = resizedImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR Code: {ex.Message}", "QR Code Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pcbQRcode.Image = null;
            }
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
                // SessionID is not set here; it will be generated by the database
                RegularVehicleID = regularVehicleId,
                PlateNumber = txtPlateNumber.Text,
                VehicleType = cmbTypeOfVehicle.SelectedItem?.ToString() ?? string.Empty,
                TimeIn = _currentTimeIn,
                TimeOut = null,
                TotalAmount = null,
                QRCodeData = null // Will set after saving to get the SessionID
            };

            try
            {
                _context.RegularParkingSessions.Add(newSession);
                await _context.SaveChangesAsync();

                // Now newSession.SessionID contains the generated int value
                // Generate QR code data using the int SessionID
                string qrCodeContent = $"SessionID: {newSession.SessionID}\n" +
                                       $"VehicleID: {regularVehicleId}\n" +
                                       $"PlateNumber: {txtPlateNumber.Text}\n" +
                                       $"VehicleType: {cmbTypeOfVehicle.SelectedItem}\n" +
                                       $"TimeIn: {_currentTimeIn:MM/dd/yyyy hh:mm:ss tt}";

                newSession.QRCodeData = qrCodeContent;
                _context.RegularParkingSessions.Update(newSession);
                await _context.SaveChangesAsync();

                MessageBox.Show("Vehicle checked in successfully!", "Check In Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GenerateQRCode(newSession.QRCodeData);
                await LoadActiveParkingSessions();
                ClearInputFields();
                await GenerateNewRegularVehicleID();
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Error checking in vehicle: {dbEx.InnerException?.Message ?? dbEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred during check-in: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeCamera()
        {
            // Ensure any previous camera is stopped cleanly
            StopCamera();

            // Check if any video input devices were found during form load
            if (_videoDevices == null || _videoDevices.Count == 0)
            {
                // Ensure this message is displayed on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("No video input devices found. Cannot start camera.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show("No video input devices found. Cannot start camera.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // If for some reason a camera object exists and is already running, prevent restarting
            if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
            {
                return; // Camera is already active, do nothing
            }

            try
            {
                // Instantiate the video capture device using the moniker string of the first device
                _videoCaptureDevice = new VideoCaptureDevice(_videoDevices[0].MonikerString);

                // Optional: Attempt to set a specific video resolution for better compatibility/performance
                // This can sometimes help with cameras that are finicky with default settings.
                if (_videoCaptureDevice.VideoCapabilities.Length > 0)
                {
                    // Prioritize 640x480 if available, otherwise pick the first capability
                    var desiredCapability = _videoCaptureDevice.VideoCapabilities.FirstOrDefault(
                                            c => c.FrameSize.Width == 640 && c.FrameSize.Height == 480)
                                            ?? _videoCaptureDevice.VideoCapabilities.FirstOrDefault();

                    if (desiredCapability != null)
                    {
                        _videoCaptureDevice.VideoResolution = desiredCapability;
                    }
                }

                // Subscribe to the NewFrame event to receive video frames
                _videoCaptureDevice.NewFrame += videoSourcePlayer_NewFrame;

                // Start the video capture device
                _videoCaptureDevice.Start();

                // Confirm camera started on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("Camera started. Please hold the QR code in front of the camera.", "Scan QR Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                else
                {
                    MessageBox.Show("Camera started. Please hold the QR code in front of the camera.", "Scan QR Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during camera startup (e.g., camera already in use, driver error)
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show($"Failed to start camera: {ex.Message}\n\n" +
                                        "Possible reasons:\n" +
                                        "- Camera in use by another application\n" +
                                        "- Driver issues or incompatible camera\n" +
                                        "- No compatible video format found\n" +
                                        "Please close other camera applications and try again, or check camera settings.",
                                        "Camera Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Failed to start camera: {ex.Message}\n\n" +
                                    "Possible reasons:\n" +
                                    "- Camera in use by another application\n" +
                                    "- Driver issues or incompatible camera\n" +
                                    "- No compatible video format found\n" +
                                    "Please close other camera applications and try again, or check camera settings.",
                                    "Camera Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                StopCamera(); // Attempt to stop any partially started camera or release resources
            }
        }

        private async void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (_isScanningQRCode) return;

            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            // Display the frame in the PictureBox (on UI thread)
            if (pcbQRcode.InvokeRequired)
            {
                pcbQRcode.Invoke(new MethodInvoker(() =>
                {
                    if (pcbQRcode.Image != null)
                        pcbQRcode.Image.Dispose();
                    pcbQRcode.Image = (Bitmap)frame.Clone();
                }));
            }
            else
            {
                if (pcbQRcode.Image != null)
                    pcbQRcode.Image.Dispose();
                pcbQRcode.Image = (Bitmap)frame.Clone();
            }

            // Try to decode QR code
            var result = _qrCodeReader.Decode(frame);
            frame.Dispose();

            if (result != null && !string.IsNullOrWhiteSpace(result.Text))
            {
                _isScanningQRCode = true; // Prevent further scans
                // Call on UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(async () =>
                    {
                        await ProcessScannedQRCodeDataAsync(result.Text);
                        _isScanningQRCode = false; // Reset for next scan if needed
                    }));
                }
                else
                {
                    await ProcessScannedQRCodeDataAsync(result.Text);
                    _isScanningQRCode = false;
                }
            }
        }

        private void StopCamera()
        {
            if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
            {
                _videoCaptureDevice.SignalToStop();
                _videoCaptureDevice.WaitForStop(); // Wait for the camera thread to fully stop
                _videoCaptureDevice.NewFrame -= videoSourcePlayer_NewFrame; // Detach event handler
                _videoCaptureDevice = null; // Allow garbage collection

                // Clear the PictureBox on the UI thread
                if (pcbQRcode.InvokeRequired)
                {
                    pcbQRcode.Invoke(new MethodInvoker(delegate { pcbQRcode.Image = null; }));
                }
                else
                {
                    pcbQRcode.Image = null;
                }
            }
        }

        private void Regular_Parking_FormClosing(object sender, FormClosingEventArgs e)
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource?.Dispose();
            _qrScanCancellationTokenSource?.Cancel();
            _qrScanCancellationTokenSource?.Dispose();
            StopCamera(); // <--- This is crucial! Ensure it's called reliably.
            _context?.Dispose();
        }

        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            // Reset cancellation token for a new scan
            _qrScanCancellationTokenSource?.Dispose();
            _qrScanCancellationTokenSource = new CancellationTokenSource();

            // Start the camera and QR scanning process
            InitializeCamera();
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
                        decimal hourlyRate = fee?.FeePerHour ?? 0m;

                        session.TotalAmount = (decimal)Math.Ceiling(duration.TotalHours) * hourlyRate;

                        _context.RegularParkingSessions.Update(session);
                        await _context.SaveChangesAsync();

                        StopCamera();
                        await LoadActiveParkingSessions();

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
                StopCamera();
            }
        }
    }
}
