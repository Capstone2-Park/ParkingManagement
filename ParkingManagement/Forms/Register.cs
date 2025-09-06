using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Add UserRole values to cbRole if not already present
            if (!cbRole.Items.Contains("Admin")) cbRole.Items.Add("Admin");
            if (!cbRole.Items.Contains("Staff")) cbRole.Items.Add("Staff");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string role = cbRole.SelectedItem?.ToString();
            string fullName = txtFname.Text.Trim();
            string username = txtUname.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash password using SHA256
            string passwordHash = ComputeSha256Hash(password);

            using (var db = new ParkingDbContext())
            {
                // Check if username already exists
                bool usernameExists = db.Users.Any(u => u.Username == username);
                if (usernameExists)
                {
                    MessageBox.Show("Username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = new Users
                {
                    UserRole = role,
                    FullName = fullName,
                    Username = username,
                    PasswordHash = passwordHash,
                    DateCreated = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtUname.Clear();
            txtPass.Clear();
            cbRole.SelectedIndex = -1; // Clears selection but not items
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            LogIn loginForm = new LogIn();
            loginForm.Show();
            this.Hide();
        }
    }
}
