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
                // Get the latest session if sessionId is 0, otherwise use the provided sessionId
                RegularParkingSession session;
                if (sessionId == 0)
                {
                    session = db.RegularParkingSessions
                        .OrderByDescending(s => s.SessionID)
                        .FirstOrDefault();
                }
                else
                {
                    session = db.RegularParkingSessions
                        .FirstOrDefault(s => s.SessionID == sessionId);
                }

                if (session == null)
                {
                    MessageBox.Show("No recent parking session found.");
                    return;
                }

                // Build QR data string using only available fields
                string qrData = $"SessionID: {session.SessionID}\n" +
                                $"VehicleID: {session.RegularVehicleID}\n" +
                                $"Type: {session.VehicleType}\n" +
                                $"Time In: {session.TimeIn}\n" +
                                $"Time Out: {session.TimeOut}\n" +
                                $"Total: {session.TotalAmount}";

                using (var qrGenerator = new QRCoder.QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.Q))
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                {
                    Bitmap qrBitmap = qrCode.GetGraphic(20);
                    Bitmap resizedBitmap = new Bitmap(qrBitmap, pbQRCode.Width, pbQRCode.Height);
                    pbQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbQRCode.Image = resizedBitmap;

                    using (var ms = new MemoryStream())
                    {
                        resizedBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] qrBytes = ms.ToArray();

                        var total = new RegularParkingTotals
                        {
                            VehicleType = session.VehicleType,
                            TimeIn = session.TimeIn,
                            TimeOut = session.TimeOut, // This will be null for new sessions
                            TotalAmount = session.TotalAmount,
                            SessionID = session.SessionID,
                            QRCodeImage = Convert.ToBase64String(qrBytes)
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
                    .FirstOrDefault(t => t.SessionID == session.SessionID);

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
