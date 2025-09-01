using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement.Forms
{
    public partial class HistoricalRecordReg : Form
    {
        public HistoricalRecordReg()
        {
            InitializeComponent();
            dtpFrom.ValueChanged += DateRangeChanged;
            dtpTo.ValueChanged += DateRangeChanged;
        }

        private void HistoricalRecordReg_Load(object sender, EventArgs e)
        {
            LoadRegRecords();
        }

        private void DateRangeChanged(object sender, EventArgs e)
        {
            LoadRegRecords();
        }

        private void LoadRegRecords()
        {
            using var db = new ParkingDbContext();

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            // Get all records in the range
            var records = (from tr in db.TransactionsReg
                           join rpt in db.RegularParkingTotal on tr.TotalID equals rpt.TotalID
                           join rps in db.RegularParkingSessions on rpt.SessionID equals rps.SessionID
                           where tr.TransactionDate >= fromDate && tr.TransactionDate <= toDate.AddDays(1).AddTicks(-1)
                           select new
                           {
                               tr.TransactionDate,
                               tr.TransactionID,
                               rps.SessionID,
                               rpt.VehicleType,
                               rpt.TotalAmount,
                               tr.CashInHand,
                               tr.Change
                           }).ToList();

            var dt = new DataTable();
            dt.Columns.Add("TransactionDate", typeof(DateTime));
            dt.Columns.Add("TransactionID", typeof(int));
            dt.Columns.Add("SessionID", typeof(int));
            dt.Columns.Add("VehicleType");
            dt.Columns.Add("TotalAmount", typeof(decimal));
            dt.Columns.Add("CashInHand", typeof(decimal));
            dt.Columns.Add("Change", typeof(decimal));

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
                            r.SessionID,
                            r.VehicleType,
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
                        DBNull.Value, // SessionID
                        "",           // VehicleType
                        0m,           // TotalAmount
                        0m,           // CashInHand
                        0m            // Change
                    );
                }
            }

            dgvRegRecord.DataSource = dt;
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            var homePage = this.ParentForm as HomePage;
            if (homePage != null)
            {
                var historicalRecords = new HistoricalRecords();
                homePage.ShowFormInPanel(historicalRecords);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
