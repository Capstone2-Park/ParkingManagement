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
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1); // Include the whole end day

            var records = (from tr in db.TransactionsReg
                           join rpt in db.RegularParkingTotal on tr.TotalID equals rpt.TotalID
                           join rps in db.RegularParkingSessions on rpt.SessionID equals rps.SessionID
                           where tr.TransactionDate >= fromDate && tr.TransactionDate <= toDate
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

            foreach (var r in records)
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
