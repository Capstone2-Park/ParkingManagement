using System;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;
using ParkingManagement.Models;
using System.Linq;
using System.IO;

namespace ParkingManagement.Forms
{
    public partial class QRcode : Form


    {
        private int sessionId;

        public QRcode(int sessionId)
        {
            InitializeComponent();
            this.sessionId = sessionId;
            this.Load += QRcode_Load;
        }

        // Keep the parameterless constructor if needed elsewhere
        public QRcode()
        {
            InitializeComponent();
            this.Load += QRcode_Load;
        }

        private void QRcode_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var session = db.RegularParkingSessions
                    .FirstOrDefault(s => s.SessionID == sessionId);

                if (session == null)
                {
                    MessageBox.Show("No recent parking session found.");
                    return;
                }

                var slot = db.Parkingslot.FirstOrDefault(s => s.SessionID == sessionId);

                if (slot == null || string.IsNullOrEmpty(slot.SlotNumber))
                {
                    MessageBox.Show("No slot found for this session. Cannot generate QR code record.");
                    return;
                }

                string qrData = $"SessionID: {session.SessionID}\n" +
                                $"SlotID: {slot.SlotID}\n" +
                                $"Plate: {session.PlateNumber}\n" +
                                $"Type: {session.VehicleType}\n" +
                                $"Time In: {session.TimeIn}\n" +
                                $"Time Out: {session.TimeOut}\n" +
                                $"Total: {session.TotalAmount}\n" +
                                $"Slot: {slot.SlotNumber}";

                using (var qrGenerator = new QRCoder.QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.Q))
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                {
                    // Generate the QR code at a base size
                    Bitmap qrBitmap = qrCode.GetGraphic(20);

                    // Resize the QR code to fit the PictureBox
                    Bitmap resizedBitmap = new Bitmap(qrBitmap, pbQRCode.Width, pbQRCode.Height);

                    // Set PictureBox SizeMode to StretchImage (optional if set in designer)
                    pbQRCode.SizeMode = PictureBoxSizeMode.StretchImage;

                    pbQRCode.Image = resizedBitmap;

                    // Save the QR code as a PNG byte array
                    using (var ms = new MemoryStream())
                    {
                        resizedBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] qrBytes = ms.ToArray();

                        var total = new RegularParkingTotals
                        {
                            PlateNumber = session.PlateNumber,
                            VehicleType = session.VehicleType,
                            TimeIn = session.TimeIn,
                            TimeOut = session.TimeOut,
                            TotalAmount = session.TotalAmount,
                            SlotNumber = slot.SlotNumber,
                            SlotID = slot.SlotID,           // Set the foreign key
                            SessionID = session.SessionID,  // Set the foreign key
                            QRCodeImage = Convert.ToBase64String(qrBytes) // Save as Base64 string
                        };

                        try
                        {
                            db.RegularParkingTotal.Add(total);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Error saving RegularParkingTotal:\n{ex.Message}\n{ex.InnerException?.Message}",
                                "Database Save Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }

                var totalRecord = db.RegularParkingTotal
                    .FirstOrDefault(t => t.SessionID == sessionId);

                if (totalRecord != null && !string.IsNullOrEmpty(totalRecord.QRCodeImage))
                {
                    byte[] qrBytes = Convert.FromBase64String(totalRecord.QRCodeImage);
                    using (var ms = new MemoryStream(qrBytes))
                    {
                        pbQRCode.Image = Image.FromStream(ms);
                    }
                }
            }
        }
    }
}
