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
            dtpFrom.ValueChanged += DateRangeChanged;
            dtpTo.ValueChanged += DateRangeChanged;
        }

        private void HistoricalRecords_Load(object sender, EventArgs e)
        {
            LoadRentRecords();
        }

        private void DateRangeChanged(object sender, EventArgs e)
        {
            LoadRentRecords();
        }

        private void LoadRentRecords()
        {
            using var db = new ParkingDbContext();

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            // Get all records in the range
            var records = (from tr in db.TransactionsRent
                           join vs in db.VehicleSessions on tr.SessionID equals vs.SessionID
                           join v in db.Vehicles on vs.VehicleID equals v.VehicleID
                           join c in db.Clients on v.ClientID equals c.ClientID
                           where tr.TransactionDate >= fromDate && tr.TransactionDate <= toDate.AddDays(1).AddTicks(-1)
                           select new
                           {
                               tr.TransactionDate,
                               tr.TransactionID,
                               c.Name,
                               v.PlateNumber,
                               v.VehicleType,
                               vs.DurationType,
                               vs.TotalAmount,
                               tr.CashInHand,
                               tr.Change
                           }).ToList();

            var dt = new DataTable();
            dt.Columns.Add("TransactionDate", typeof(DateTime));
            dt.Columns.Add("TransactionID", typeof(int));
            dt.Columns.Add("Name");
            dt.Columns.Add("PlateNumber");
            dt.Columns.Add("VehicleType");
            dt.Columns.Add("DurationType");
            dt.Columns.Add("TotalAmount", typeof(decimal));
            dt.Columns.Add("CashInHand", typeof(int));
            dt.Columns.Add("Change", typeof(int));

            // Build a set of all dates in the range
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var dayRecords = records.Where(r => r.TransactionDate.Date == date).ToList();
                if (dayRecords.Count > 0)
                {
                    foreach (var r in dayRecords)
                    {
                        dt.Rows.Add(
                            r.TransactionDate,
                            r.TransactionID,
                            r.Name,
                            r.PlateNumber,
                            r.VehicleType,
                            r.DurationType,
                            r.TotalAmount,
                            r.CashInHand,
                            r.Change
                        );
                    }
                }
                else
                {
                    // Add a default row for this date
                    dt.Rows.Add(
                        date,
                        DBNull.Value, // TransactionID
                        "",           // Name
                        "",           // PlateNumber
                        "",           // VehicleType
                        "",           // DurationType
                        0m,           // TotalAmount
                        0,            // CashInHand
                        0             // Change
                    );
                }
            }

            dgvRentRecord.DataSource = dt;
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var historicalRecord = new HistoricalRecordReg();
                homePage.ShowFormInPanel(historicalRecord);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
