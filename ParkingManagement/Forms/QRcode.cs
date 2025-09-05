using System;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;
using ParkingManagement.Models;
using System.Linq;
using System.IO;
using System.Drawing.Printing; // Add this at the top

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

                // Build QR data string including Discount
                string qrData = $"SessionID: {session.SessionID}\n" +
                                $"VehicleID: {session.RegularVehicleID}\n" +
                                $"Type: {session.VehicleType}\n" +
                                $"Time In: {session.TimeIn}\n" +
                                $"Time Out: {session.TimeOut}\n" +
                                $"Total: {session.TotalAmount}\n" +
                                $"Discount: {session.Discount}";

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

                        // Check if a record already exists for this session
                        var existingTotal = db.RegularParkingTotal.FirstOrDefault(t => t.SessionID == session.SessionID);
                        if (existingTotal == null)
                        {
                            var slot = db.RegularSlot
                                .Where(s => (session.VehicleType == "2-Wheels" && s.AvailableSlotM > 0)
                                         || (session.VehicleType == "4-Wheels" && s.AvailableSlotV > 0))
                                .FirstOrDefault();

                            if (slot != null)
                            {
                                if (session.VehicleType == "2-Wheels")
                                    slot.AvailableSlotM = Math.Max(0, slot.AvailableSlotM - 1);
                                else if (session.VehicleType == "4-Wheels")
                                    slot.AvailableSlotV = Math.Max(0, slot.AvailableSlotV - 1);

                                db.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("No available RegularSlot found for this vehicle type.", "Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            var total = new RegularParkingTotals
                            {
                                VehicleType = session.VehicleType,
                                TimeIn = session.TimeIn,
                                TimeOut = session.TimeOut,
                                TotalAmount = session.TotalAmount,
                                SessionID = session.SessionID,
                                QRCodeImage = Convert.ToBase64String(qrBytes),
                                Discount = session.Discount,
                                RegSlotID = slot.RegSlotID // <-- Set the foreign key here
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (pbQRCode.Image == null)
            {
                MessageBox.Show("No QR code to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) =>
            {
                // Center the image on the page
                int x = (ev.PageBounds.Width - pbQRCode.Image.Width) / 2;
                int y = (ev.PageBounds.Height - pbQRCode.Image.Height) / 2;
                ev.Graphics.DrawImage(pbQRCode.Image, x, y, pbQRCode.Image.Width, pbQRCode.Image.Height);
            };

            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.Document = printDoc;
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
        }
    }
}
