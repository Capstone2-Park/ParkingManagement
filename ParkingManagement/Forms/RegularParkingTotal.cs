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
        private Bitmap _lastFrame; // Store the last frame for capture

        public RegularParkingTotal()
        {
            InitializeComponent();
            _qrReader = new BarcodeReader<Bitmap>(
                bitmap =>
                {
                    // Lock the bitmap's bits to access raw pixel data
                    var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    try
                    {
                        int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
                        byte[] rgbValues = new byte[bytes];
                        System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                        return new RGBLuminanceSource(rgbValues, bitmap.Width, bitmap.Height, RGBLuminanceSource.BitmapFormat.BGR32);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bmpData);
                    }
                })
            {
                AutoRotate = true,
                Options = new DecodingOptions { TryHarder = true }
            };
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            lblScanStatus.Text = "Camera On. Ready to capture.";
            lblScanStatus.Visible = true;

            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0)
            {
                MessageBox.Show("No camera found.");
                return;
            }
            _videoDevice = new VideoCaptureDevice(_videoDevices[0].MonikerString);
            _videoDevice.NewFrame += VideoDevice_NewFrame;
            _videoDevice.Start();
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            // Capture the current frame and scan for QR code
            if (_lastFrame != null)
            {
                Bitmap captured = (Bitmap)_lastFrame.Clone();
                var result = _qrReader.Decode(captured);
                captured.Dispose();

                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    ProcessQRCode(result.Text);
                }
                else
                {
                    MessageBox.Show("No QR code detected in the captured image.");
                }
            }
            else
            {
                MessageBox.Show("No image captured from camera.");
            }
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
            _lastFrame?.Dispose();
            _lastFrame = null;
        }

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            // Store the latest frame for capture
            _lastFrame?.Dispose();
            _lastFrame = (Bitmap)frame.Clone();

            // Resize for display
            Bitmap resizedFrame = new Bitmap(ptbCheckOut.Width, ptbCheckOut.Height);
            using (Graphics g = Graphics.FromImage(resizedFrame))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(frame, 0, 0, ptbCheckOut.Width, ptbCheckOut.Height);
            }

            ptbCheckOut.Invoke(new Action(() =>
            {
                ptbCheckOut.Image?.Dispose();
                ptbCheckOut.Image = (Bitmap)resizedFrame.Clone();
          
            }));

            frame.Dispose();
            resizedFrame.Dispose();
        }

        private void ProcessQRCode(string qrData)
        {
            // Example: qrData contains "SessionID: 123\nPlate: ABC123\n..."
            var lines = qrData.Split('\n');
            var sessionIdLine = lines.FirstOrDefault(l => l.StartsWith("SessionID:"));
            if (sessionIdLine == null)
            {
                MessageBox.Show("Invalid QR code.");
                return;
            }
            var parts = sessionIdLine.Split(':'); 
            if (parts.Length < 2 || !int.TryParse(parts[1].Trim(), out int sessionId))
            {
                MessageBox.Show("Invalid SessionID format in QR code.");
                return;
            }

            using (var db = new ParkingDbContext())
            {
                // Find the RegularParkingTotals record by SessionID
                var total = db.RegularParkingTotal.FirstOrDefault(t => t.SessionID == sessionId);
                if (total == null)
                {
                    MessageBox.Show("Parking record not found.");
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
            lblScanStatus.Visible = false;
        }

        private void RegularParkingTotal_Load(object sender, EventArgs e)
        {
           
        }
    }
}
