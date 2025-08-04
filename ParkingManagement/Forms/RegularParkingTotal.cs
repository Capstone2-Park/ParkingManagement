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
            // Example: qrData contains "SessionID: 123\nPlate: ABC123\n..."
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
                // Find the RegularParkingTotals record by SessionID
                var total = db.RegularParkingTotal.FirstOrDefault(t => t.SessionID == sessionId);
                if (total == null)
                {
                    MessageBox.Show("Parking record not found.");
                    _isScanning = true;
                    return;
                }

                // Set TimeOut to now if not already set
                if (total.TimeOut == null)
                    total.TimeOut = DateTime.Now;

                // Calculate total hours
                var timeIn = total.TimeIn;
                var timeOut = total.TimeOut.Value;
                var totalHours = (timeOut - timeIn).TotalHours;

                // Get the correct daily fee from Fee table
                string vehicleType = total.VehicleType?.Trim();
                string durationType = "Daily";
                var fee = db.Fees.FirstOrDefault(f => f.VehicleType == vehicleType && f.DurationType == durationType);
                decimal dailyRate = fee?.FixedPrice ?? 0;

                // Calculate total amount: 1 daily fee for up to 2 hours, add another for each additional 2 hours
                int days = (int)Math.Ceiling(totalHours / 2.0);
                total.TotalAmount = days * dailyRate;

                // Save changes
                db.SaveChanges();

                // Compose and display receipt
                var receipt = new StringBuilder();
                receipt.AppendLine("=== Parking Receipt ===");
                receipt.AppendLine($"Plate Number: {total.PlateNumber}");
                receipt.AppendLine($"Vehicle Type: {total.VehicleType}");
                receipt.AppendLine($"Time In: {total.TimeIn}");
                receipt.AppendLine($"Time Out: {total.TimeOut}");
                receipt.AppendLine($"Slot: {total.SlotNumber}");
                receipt.AppendLine($"Total Hours: {totalHours:F2}");
                receipt.AppendLine($"Total Amount: {total.TotalAmount:N2}");
                rtbReceipt.Text = receipt.ToString();
            }
            StopCamera();
        }

        private void RegularParkingTotal_Load(object sender, EventArgs e)
        {
           
        }
    }
}
