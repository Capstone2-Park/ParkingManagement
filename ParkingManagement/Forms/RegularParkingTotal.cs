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
        private volatile bool _isProcessingQRCode = false; // Prevent multiple scans in quick succession

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
            lblScanStatus.Text = "Camera On. Ready to scan.";
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
            StopCamera();
        }

        private void StopCamera()
        {
            var videoDevice = _videoDevice;
            if (videoDevice == null)
                return;

            if (videoDevice.IsRunning)
            {
                // Unsubscribe before stopping to avoid deadlocks
                videoDevice.NewFrame -= VideoDevice_NewFrame;

                // Stop the camera on a background thread to avoid UI freeze
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        videoDevice.SignalToStop();
                        videoDevice.WaitForStop();
                    }
                    catch { /* Handle/log errors if needed */ }
                });
            }
            _videoDevice = null;

            if (ptbCheckOut.InvokeRequired)
            {
                ptbCheckOut.BeginInvoke(new Action(() => ptbCheckOut.Image = null));
            }
            else
            {
                ptbCheckOut.Image = null;
            }
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

            // Automatic QR code scanning
            if (!_isProcessingQRCode)
            {
                _isProcessingQRCode = true;
                Bitmap scanFrame = (Bitmap)frame.Clone();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        var result = _qrReader.Decode(scanFrame);
                        if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                        {
                            // Process QR code on UI thread
                            this.Invoke(new Action(() =>
                            {
                                ProcessQRCode(result.Text);
                            }));
                        }
                        else
                        {
                            // No QR code found, allow next scan
                            _isProcessingQRCode = false;
                        }
                    }
                    finally
                    {
                        scanFrame.Dispose();
                    }
                });
            }

            frame.Dispose();
            resizedFrame.Dispose();
        }

        private void ProcessQRCode(string qrData)
        {
            var lines = qrData.Split('\n');
            var sessionIdLine = lines.FirstOrDefault(l => l.StartsWith("SessionID:"));
            if (sessionIdLine == null)
            {
                MessageBox.Show("Invalid QR code.");
                _isProcessingQRCode = false;
                return;
            }
            var parts = sessionIdLine.Split(':');
            if (parts.Length < 2 || !int.TryParse(parts[1].Trim(), out int sessionId))
            {
                MessageBox.Show("Invalid SessionID format in QR code.");
                _isProcessingQRCode = false;
                return;
            }

            using (var db = new ParkingDbContext())
            {
                var total = db.RegularParkingTotal.FirstOrDefault(t => t.SessionID == sessionId);
                if (total == null)
                {
                    MessageBox.Show("QR code not found in the system.");
                    _isProcessingQRCode = false;
                    return;
                }

                bool alreadyCheckedOut = total.TimeOut != null;

                if (!alreadyCheckedOut)
                {
                    total.TimeOut = DateTime.Now;
                    var timeIn = total.TimeIn;
                    var timeOut = total.TimeOut.Value;
                    var totalHours = (timeOut - timeIn).TotalHours;
                    if (totalHours > 0 && totalHours < 2)
                        totalHours = 2;
                    string vehicleType = total.VehicleType?.Trim() ?? "";
                    string durationType = "Daily";
                    var fee = db.Fees.FirstOrDefault(f => f.VehicleType.Trim().ToLower() == vehicleType.ToLower() && f.DurationType == durationType);
                    decimal dailyRate = fee?.FixedPrice ?? 0;
                    int days = (int)Math.Ceiling(Math.Max(totalHours, 0.01) / 2.0);
                    if (days < 1) days = 1;
                    total.TotalAmount = days * dailyRate;
                    db.SaveChanges();
                }

                var receipt = new StringBuilder();
                receipt.AppendLine("=== Parking Receipt ===");
                receipt.AppendLine($"Plate Number: {total.PlateNumber}");
                receipt.AppendLine($"Vehicle Type: {total.VehicleType}");
                receipt.AppendLine($"Time In: {total.TimeIn}");
                receipt.AppendLine($"Time Out: {total.TimeOut}");
                receipt.AppendLine($"Slot: {total.SlotNumber}");
                var totalHoursDisplay = (total.TimeOut.Value - total.TimeIn).TotalHours;
                receipt.AppendLine($"Total Hours: {totalHoursDisplay:F2}");
                receipt.AppendLine($"Total Amount: {total.TotalAmount:N2}");
                rtbReceipt.Text = receipt.ToString();
            }

            lblScanStatus.Visible = false;
            StopCamera();
            _isProcessingQRCode = false;
        }

        private void RegularParkingTotal_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }
    }
}
