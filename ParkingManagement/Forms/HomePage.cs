using ParkingManagement.Forms;
using ParkingManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement
{

    public partial class HomePage : Form
    {
        private System.Windows.Forms.Timer notifTimer;
        private HashSet<int> acknowledgedDueSessions = new HashSet<int>();

        private System.Windows.Forms.Timer slotHistoryTimer;
        private bool slotHistoryCreatedToday = false;

        public HomePage()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.MinimumSize = new Size(800, 600);

            notifTimer = new System.Windows.Forms.Timer();
            notifTimer.Interval = 10000; // 10 seconds, adjust as needed
            notifTimer.Tick += NotifTimer_Tick;
            notifTimer.Start();

            // Set up the timer to check every minute
            slotHistoryTimer = new System.Windows.Forms.Timer();
            slotHistoryTimer.Interval = 60000; // 1 minute
            slotHistoryTimer.Tick += SlotHistoryTimer_Tick;
            slotHistoryTimer.Start();
        }

        public void ShowFormInPanel(Form childForm)
        {
            panelHome.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelHome.Controls.Add(childForm);
            childForm.Show();
        }

        private void btnRental_Click(object sender, EventArgs e)
        {
            btnClientM.Visible = true;
        }

        private void btnClientM_Click_1(object sender, EventArgs e)
        {
            ShowFormInPanel(new ClientManagement());
        }

        private void btnFeeM_Click(object sender, EventArgs e)
        {
            FeeManagement FeeList = new FeeManagement();
            FeeList.Show();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var today = DateTime.Today;
                if (!db.Set<SlotHistory>().Any(h => h.Date == today))
                {
                    int availableSlotM = db.RegularSlot.Sum(rs => rs.AvailableSlotM);
                    int availableSlotV = db.RegularSlot.Sum(rs => rs.AvailableSlotV);
                    int availableParkingSlots = db.Parkingslot.Count(ps => ps.SlotStatus == "Available");

                    var history = new SlotHistory
                    {
                        Date = today,
                        AvailableSlotM = availableSlotM,
                        AvailableSlotV = availableSlotV,
                        AvailableParkingSlots = availableParkingSlots
                    };

                    db.Set<SlotHistory>().Add(history);
                    db.SaveChanges();
                }
            }
        }

        private void btnRegularParking_Click(object sender, EventArgs e)
        {
            btnRegParkSess.Visible = true;
        }

        private void btnParkSlot_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new TotalPayment());
        }

        private void btnRegParkSess_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Regular_Parking());
        }


        private void button1_Click_2(object sender, EventArgs e)
        {
            ShowFormInPanel(new RegularParkingTotal());
        }

        private void btnSlotNav_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SlotEdit());
        }



        public void UpdateNotificationCount(int count)
        {
            lblNotif.Text = count.ToString();
        }

        private void NotifTimer_Tick(object? sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var tomorrow = DateTime.Today.AddDays(1);

                // Get all due sessions (today or earlier, and tomorrow)
                var dueSessions = db.VehicleSessions
                    .Where(vs => vs.EndDateTime.Date <= tomorrow)
                    .Select(vs => vs.SessionID) // Use your actual PK property
                    .ToList();

                // Only count those not yet acknowledged
                int count = dueSessions.Count(id => !acknowledgedDueSessions.Contains(id));

                UpdateNotificationCount(count);
            }
        }

        private void pbNotif_Click(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                var tomorrow = DateTime.Today.AddDays(1);

                // Get all due sessions (today or earlier, and tomorrow)
                var dueSessions = db.VehicleSessions
                    .Where(vs => vs.EndDateTime.Date <= tomorrow)
                    .Select(vs => vs.SessionID) // Use your actual PK property
                    .ToList();

                foreach (var id in dueSessions)
                    acknowledgedDueSessions.Add(id);
            }

            lblNotif.Text = ""; // Hide the notification count immediately
            ShowFormInPanel(new Notification());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

            btnReports.Visible = true;
            btnFinance.Visible = true;
            btnRecords.Visible = true;
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ReportManagement());
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new FinanceSum());
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new HistoricalRecords());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SlotHistoryTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            // Check if it's 11:59 PM and we haven't created the record yet today
            if (now.Hour == 23 && now.Minute == 59 && !slotHistoryCreatedToday)
            {
                CreateSlotHistoryForToday();
                slotHistoryCreatedToday = true;
            }
            // Reset the flag at midnight
            if (now.Hour == 0 && now.Minute == 0)
            {
                slotHistoryCreatedToday = false;
            }
        }

        private void CreateSlotHistoryForToday()
        {
            using (var db = new ParkingDbContext())
            {
                var today = DateTime.Today;
                if (!db.Set<SlotHistory>().Any(h => h.Date == today))
                {
                    int availableSlotM = db.RegularSlot.Sum(rs => rs.AvailableSlotM);
                    int availableSlotV = db.RegularSlot.Sum(rs => rs.AvailableSlotV);
                    int availableParkingSlots = db.Parkingslot.Count(ps => ps.SlotStatus == "Available");

                    var history = new SlotHistory
                    {
                        Date = today,
                        AvailableSlotM = availableSlotM,
                        AvailableSlotV = availableSlotV,
                        AvailableParkingSlots = availableParkingSlots
                    };

                    db.Set<SlotHistory>().Add(history);
                    db.SaveChanges();
                }
            }
        }
    }
}
