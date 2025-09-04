using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ParkingManagement.Models;

namespace ParkingManagement.Forms
{
    public partial class UserManagement : Form
    {
        private DataTable usersBackup;

        public UserManagement()
        {
            InitializeComponent();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            using (var db = new ParkingDbContext())
            {
                // Fill dgvLogged with CurrentUser table
                dgvLogged.DataSource = db.Set<CurrentUser>().ToList();
                dgvLogged.ReadOnly = true;

                // Fill dgvRegistered with Users table
                var users = db.Users.ToList();
                dgvRegistered.DataSource = users;
                dgvRegistered.ReadOnly = true;

                // Backup for revert
                usersBackup = ToDataTable(users);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvRegistered.ReadOnly = false;
            dgvRegistered.CellEndEdit += DgvRegistered_CellEndEdit;
            dgvRegistered.MouseHover += DgvRegistered_MouseHover;
        }

        private void DgvRegistered_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            PromptSaveOrRevert();
        }

        private void DgvRegistered_MouseHover(object sender, EventArgs e)
        {
            if (!dgvRegistered.ReadOnly)
                PromptSaveOrRevert();
        }

        private void PromptSaveOrRevert()
        {
            var result = MessageBox.Show("Save Edit?", "Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SaveUsersEdits();
                dgvRegistered.ReadOnly = true;
                dgvRegistered.CellEndEdit -= DgvRegistered_CellEndEdit;
                dgvRegistered.MouseHover -= DgvRegistered_MouseHover;
            }
            else
            {
                // Revert changes
                dgvRegistered.DataSource = usersBackup.Copy();
                dgvRegistered.ReadOnly = true;
                dgvRegistered.CellEndEdit -= DgvRegistered_CellEndEdit;
                dgvRegistered.MouseHover -= DgvRegistered_MouseHover;
            }
        }

        private void SaveUsersEdits()
        {
            using (var db = new ParkingDbContext())
            {
                foreach (DataGridViewRow row in dgvRegistered.Rows)
                {
                    if (row.IsNewRow) continue;
                    int userId = Convert.ToInt32(row.Cells["UserID"].Value);
                    var user = db.Users.FirstOrDefault(u => u.UserID == userId);
                    if (user != null)
                    {
                        user.UserRole = row.Cells["UserRole"].Value?.ToString();
                        user.FullName = row.Cells["FullName"].Value?.ToString();
                        user.Username = row.Cells["Username"].Value?.ToString();
                        user.PasswordHash = row.Cells["PasswordHash"].Value?.ToString();
                        user.DateCreated = Convert.ToDateTime(row.Cells["DateCreated"].Value);
                    }
                }
                db.SaveChanges();
            }
        }

        private DataTable ToDataTable<T>(System.Collections.Generic.List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Close UserManagement and HomePage, open Register form
            // To avoid modifying the collection while enumerating, copy the forms to a list first
            var formsToClose = Application.OpenForms
                .OfType<Form>()
                .Where(frm => frm is HomePage || frm is UserManagement)
                .ToList();

            foreach (Form frm in formsToClose)
            {
                frm.Close();
            }
            Register registerForm = new Register();
            registerForm.Show();
        }
    }
}
