using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class LogIn : Form
    {
        private int failedAttempts = 0;
        private System.Windows.Forms.Timer cooldownTimer;
        private int cooldownSeconds = 3;
        private bool isPasswordVisible = false;

        public LogIn()
        {
            InitializeComponent();
            txtPass.PasswordChar = '*';
            lblCoolDown.Visible = false;
        }

        private void pbShow_Click(object sender, EventArgs e)
        {

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (lblCoolDown.Visible) return; // Prevent login during cooldown

            string username = txtUname.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string passwordHash = ComputeSha256Hash(password);

            using (var db = new ParkingDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
                if (user != null)
                {
                    // Fill CurrentUser table
                    var currentUser = new CurrentUser
                    {
                        FullName = user.FullName,
                        Username = user.Username,
                        Role = user.UserRole,
                        TimeLoggedin = DateTime.Now
                    };
                    db.Add(currentUser);
                    db.SaveChanges();

                    // Navigate to HomePage
                    HomePage homePage = new HomePage();
                    homePage.Show();
                    this.Hide();
                    return;
                }
            }

            failedAttempts++;
            if (failedAttempts >= 5)
            {
                StartCooldown();
            }
            else
            {
                MessageBox.Show("Wrong credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void StartCooldown()
        {
            lblCoolDown.Visible = true;
            cooldownSeconds = 3;
            lblCoolDown.Text = $"Cooldown: {cooldownSeconds}s";
            cooldownTimer = new System.Windows.Forms.Timer();
            cooldownTimer.Interval = 1000;
            cooldownTimer.Tick += CooldownTimer_Tick;
            cooldownTimer.Start();
        }

        private void CooldownTimer_Tick(object sender, EventArgs e)
        {
            cooldownSeconds--;
            lblCoolDown.Text = $"Cooldown: {cooldownSeconds}s";
            if (cooldownSeconds <= 0)
            {
                cooldownTimer.Stop();
                lblCoolDown.Visible = false;
                failedAttempts = 0;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUname.Clear();
            txtPass.Clear();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '*';
            lblCoolDown.Visible = false;

            // Insert default Admin user if not exists
            using (var db = new ParkingDbContext())
            {
                bool adminExists = db.Users.Any(u => u.UserRole == "Admin" && u.Username == "Admin");
                if (!adminExists)
                {
                    var adminUser = new Users
                    {
                        UserRole = "Admin",
                        FullName = "Admin",
                        Username = "Admin",
                        PasswordHash = ComputeSha256Hash("Admin"), // Default password is "Admin"
                        DateCreated = DateTime.Now
                    };
                    db.Users.Add(adminUser);
                    db.SaveChanges();
                }
            }
        }

        private void pbShow_Click_1(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtPass.PasswordChar = isPasswordVisible ? '\0' : '*';
        }
    }
}
