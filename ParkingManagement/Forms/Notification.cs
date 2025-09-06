using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class Notification : Form
    {
        private ParkingDbContext _db = new ParkingDbContext();

        public Notification()
        {
            InitializeComponent();
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var dueSessions = _db.VehicleSessions
                .Where(vs => vs.EndDateTime.Date == tomorrow)
                .ToList();

            MessageBox.Show($"Due sessions: {dueSessions.Count}");

            flpNotif.Controls.Clear();

            foreach (var session in dueSessions)
            {
                var vehicle = _db.Vehicles.FirstOrDefault(v => v.VehicleID == session.VehicleID);
                var client = vehicle != null ? _db.Clients.FirstOrDefault(c => c.ClientID == vehicle.ClientID) : null;

                if (client == null || vehicle == null)
                {
                    MessageBox.Show("Missing vehicle or client for session: " + session.SessionID);
                    continue;
                }

                var btn = new Button
                {
                    Text = $"{client.Name}'s rent is almost due",
                    AutoSize = false,
                    Height = 40, // Set your preferred height
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Tag = new NotificationTag { Session = session, Client = client, Vehicle = vehicle }
                };

                // Set width after creation, so you can access Margin
                btn.Width = flpNotif.ClientSize.Width - flpNotif.Padding.Horizontal - btn.Margin.Horizontal;

                btn.Click += NotificationButton_Click;
                flpNotif.Controls.Add(btn);
            }

          
        }

        private void NotificationButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is NotificationTag tag)
            {
                var session = tag.Session;
                var client = tag.Client;
                var vehicle = tag.Vehicle;

                var message = $"Plate Number: {vehicle.PlateNumber}\n" +
                              $"Client Name: {client.Name}\n" +
                              $"Notify the client: {client.CpNumber}";

                var result = MessageBox.Show(message, "Notification", MessageBoxButtons.OK);

                if (result == DialogResult.OK)
                {
                    var extend = MessageBox.Show("Do you want to extend the rent?", "Extend Rent", MessageBoxButtons.YesNo);
                    if (extend == DialogResult.Yes)
                    {
                        int daysToAdd = session.DurationType switch
                        {
                            "Daily" => 1,
                            "Weekly" => 7,
                            "Monthly" => 30,
                            "Yearly" => 365,
                            _ => 0
                        };

                        session.EndDateTime = session.EndDateTime.AddDays(daysToAdd);
                        _db.SaveChanges();
                        MessageBox.Show("Rent extended successfully.");
                        LoadNotifications(); // Refresh notifications
                    }
                }
            }
        }

        private void Notification_Load(object sender, EventArgs e)
        {

          
        }

    
    }
}
