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

        public HomePage()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.MinimumSize = new Size(800, 600);

            notifTimer = new System.Windows.Forms.Timer();
            notifTimer.Interval = 10000; // 10 seconds, adjust as needed
            notifTimer.Tick += NotifTimer_Tick;
            notifTimer.Start();
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
            btnClientM.Visible = false;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            btnClientM.Visible = false;
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
    }
}
