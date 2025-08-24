using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ParkingManagement.Forms
{
    public partial class HistoricalRecords : Form
    {
        public HistoricalRecords()
        {
            InitializeComponent();
        }

        private void HistoricalRecords_Load(object sender, EventArgs e)
        {
            using var db = new ParkingDbContext();

            // Join Client, Vehicle, VehicleSession
            var records = (from vs in db.VehicleSessions
                           join v in db.Vehicles on vs.VehicleID equals v.VehicleID
                           join c in db.Clients on v.ClientID equals c.ClientID
                           select new
                           {
                               c.ClientID,
                               v.VehicleID,
                               c.Name,
                               c.Address,
                               c.CpNumber,
                               c.Discount,
                               v.Brand,
                               v.Color,
                               v.PlateNumber,
                               v.VehicleType,
                               vs.DurationType,
                               vs.StartDate,
                               vs.EndDateTime,
                               vs.TotalAmount,
                               c.IDPicture
                           }).ToList();

            // Prepare DataTable for DataGridView with image column
            var dt = new DataTable();
            dt.Columns.Add("ClientID");
            dt.Columns.Add("VehicleID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("CpNumber");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Brand");
            dt.Columns.Add("Color");
            dt.Columns.Add("PlateNumber");
            dt.Columns.Add("VehicleType");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("StartDate", typeof(DateTime));
            dt.Columns.Add("EndDateTime", typeof(DateTime));
            dt.Columns.Add("TotalAmount", typeof(decimal));
            dt.Columns.Add("IDPicture", typeof(Image));

            foreach (var r in records)
            {
                Image img = null;
                if (!string.IsNullOrEmpty(r.IDPicture) && System.IO.File.Exists(r.IDPicture))
                {
                    using (var fs = new System.IO.FileStream(r.IDPicture, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        img = Image.FromStream(fs);
                    }
                }
                dt.Rows.Add(
                    r.ClientID,
                    r.VehicleID,
                    r.Name,
                    r.Address,
                    r.CpNumber,
                    r.Discount,
                    r.Brand,
                    r.Color,
                    r.PlateNumber,
                    r.VehicleType,
                    r.DurationType,
                    r.StartDate,
                    r.EndDateTime,
                    r.TotalAmount,
                    img
                );
            }

            dgvRentRecord.DataSource = dt;
            if (dgvRentRecord.Columns["IDPicture"] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            // --- New code for dgvRegRecord ---
            var regRecords = db.RegularParkingTotal
                .Select(r => new
                {
                    r.SessionID,
                    r.TotalID,
                    r.VehicleType,
                    r.TimeIn,
                    r.TimeOut,
                    r.Discount,
                    r.TotalAmount,
                    r.QRCodeImage // This is assumed to be a Base64 string
                })
                .ToList();

            var regDt = new DataTable();
            regDt.Columns.Add("SessionID");
            regDt.Columns.Add("TotalID");
            regDt.Columns.Add("VehicleType");
            regDt.Columns.Add("TimeIn", typeof(DateTime));
            regDt.Columns.Add("TimeOut", typeof(DateTime));
            regDt.Columns.Add("Discount");
            regDt.Columns.Add("TotalAmount", typeof(decimal));
            regDt.Columns.Add("QRCodeImage", typeof(Image));

            foreach (var r in regRecords)
            {
                Image qrImg = null;
                if (!string.IsNullOrEmpty(r.QRCodeImage))
                {
                    try
                    {
                        byte[] qrBytes = Convert.FromBase64String(r.QRCodeImage);
                        using (var ms = new MemoryStream(qrBytes))
                        {
                            qrImg = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        qrImg = null;
                    }
                }

                regDt.Rows.Add(
                    r.SessionID,
                    r.TotalID,
                    r.VehicleType,
                    r.TimeIn,
                    r.TimeOut,
                    r.Discount,
                    r.TotalAmount,
                    qrImg
                );
            }

            dgvRegRecord.DataSource = regDt;
            if (dgvRegRecord.Columns["QRCodeImage"] is DataGridViewImageColumn qrCol)
            {
                qrCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }
    }
}
