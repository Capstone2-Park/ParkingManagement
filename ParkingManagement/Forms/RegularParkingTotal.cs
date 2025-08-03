using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;
using ZXing;
using ZXing.Common; // Add this namespace for DecodingOptions
using AForge.Video;
using AForge.Video.DirectShow;
using System.Text;
using System.Drawing.Imaging; // Add this namespace for Bitmap to byte[] conversion

namespace ParkingManagement.Forms
{
    public partial class RegularParkingTotal : Form
    {
        private VideoCaptureDevice _videoDevice;
        private FilterInfoCollection _videoDevices;
        private BarcodeReader<Bitmap> _qrReader;
        private bool _isScanning;

        public RegularParkingTotal()
        {
            InitializeComponent();
            _qrReader = new BarcodeReader<Bitmap>(
                bitmap =>
                {
                    // Convert Bitmap to byte[] for RGBLuminanceSource
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        bitmap.Save(memoryStream, ImageFormat.Bmp);
                        var bitmapBytes = memoryStream.ToArray();
                        return new RGBLuminanceSource(bitmapBytes, bitmap.Width, bitmap.Height, RGBLuminanceSource.BitmapFormat.BGR32);
                    }
                })
            {
                AutoRotate = true,
                Options = new DecodingOptions { TryHarder = true }
            };
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0)
            {
                MessageBox.Show("No camera found.");
                return;
            }
            _videoDevice = new VideoCaptureDevice(_videoDevices[0].MonikerString);
            _videoDevice.NewFrame += VideoDevice_NewFrame;
            _videoDevice.Start();
            _isScanning = true;
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            if (_videoDevice != null && _videoDevice.IsRunning)
            {
                _videoDevice.SignalToStop();
                _videoDevice.WaitForStop();
                _videoDevice.NewFrame -= VideoDevice_NewFrame;
                _videoDevice = null;
            }
            ptbCheckOut.Image = null;
            _isScanning = false;
        }

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!_isScanning) return;

            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            ptbCheckOut.Invoke(new Action(() =>
            {
                ptbCheckOut.Image?.Dispose();
                ptbCheckOut.Image = (Bitmap)frame.Clone();
            }));

            var result = _qrReader.Decode(frame);
            frame.Dispose();

            if (result != null && !string.IsNullOrWhiteSpace(result.Text))
            {
                _isScanning = false;
                Invoke(new Action(() => ProcessQRCode(result.Text)));
            }
        }

        private void ProcessQRCode(string qrData)
        {
            // Parse SessionID from qrData (assuming format: "SessionID: 123\n...")
            var lines = qrData.Split('\n');
            var sessionIdLine = lines.FirstOrDefault(l => l.StartsWith("SessionID:"));
            if (sessionIdLine == null)
            {
                MessageBox.Show("Invalid QR code.");
                _isScanning = true;
                return;
            }
            int sessionId = int.Parse(sessionIdLine.Split(':')[1].Trim());

            using (var db = new ParkingDbContext())
            {
                var session = db.RegularParkingSessions.FirstOrDefault(s => s.SessionID == sessionId);
                if (session == null)
                {
                    MessageBox.Show("Session not found.");
                    _isScanning = true;
                    return;
                }

                // Fix for CS0019: Ensure both operands are of the same type by converting session.SessionID to string
                var slot = db.Parkingslot.FirstOrDefault(s => s.SessionID.ToString() == session.SessionID.ToString());

                var confirm = MessageBox.Show($"Checkout {session.PlateNumber}?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    // Compose receipt
                    var receipt = new StringBuilder();
                    receipt.AppendLine("=== Parking Receipt ===");
                    receipt.AppendLine($"Vehicle ID: {session.RegularVehicleID}");
                    receipt.AppendLine($"Plate Number: {session.PlateNumber}");
                    receipt.AppendLine($"Vehicle Type: {session.VehicleType}");
                    receipt.AppendLine($"Time In: {session.TimeIn}");
                    receipt.AppendLine($"Time Out: {DateTime.Now}");
                    receipt.AppendLine($"Slot: {slot?.SlotNumber ?? "N/A"}");
                    receipt.AppendLine($"Total Amount: {session.TotalAmount?.ToString("N2") ?? "Pending"}");
                    rtbReceipt.Text = receipt.ToString();

                    // Optionally update session/slot status here
                }
                else
                {
                    _isScanning = true;
                }
            }
            StopCamera();
        }
    }
}
