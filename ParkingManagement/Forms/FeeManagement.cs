using Microsoft.EntityFrameworkCore;
using ParkingManagement.Model;
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
    public partial class FeeManagement : Form
    {
        private ParkingDbContext _context;
        private Fee _selectedFee;

        public object MessageBoxInformation { get; private set; }

        public FeeManagement()
        {
            InitializeComponent();
            InitializeDbContext();
            InitializeDataGridView();
            SetupVehicleTypeComboBox();
            SetupDurationComboBox();
        }

        private async void FeeManagement_Load(object sender, EventArgs e)
        {
            await LoadFeesToDataGridView();
            ClearFeeInputFields();
        }

        private void InitializeDbContext()
        {
            _context = new ParkingDbContext();
        }

        private void InitializeDataGridView()
        {
            dgvFeeList.AutoGenerateColumns = false;

            dgvFeeList.Columns.Clear();
            dgvFeeList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FeeID",
                HeaderText = "Fee ID",
                DataPropertyName = "FeeID",
                ReadOnly = true,
                Visible = false
            });
            dgvFeeList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleType",
                HeaderText = "Vehicle Type",
                DataPropertyName = "VehicleType",
                ReadOnly = true
            });
            dgvFeeList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DurationType",
                HeaderText = "Duration Type",
                DataPropertyName = "DurationType",
                ReadOnly = true
            });
            dgvFeeList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FixedPrice",
                HeaderText = "Fixed Price (₱)",
                DataPropertyName = "FeePerHour", // Still mapped to FeePerHour in model
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            dgvFeeList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFeeList.MultiSelect = false; // Allow only single row selection
        }

        private void SetupVehicleTypeComboBox()
        {
            cmbTypeOfVehicle.Items.Clear();
            cmbTypeOfVehicle.Items.Add("2-Wheels");
            cmbTypeOfVehicle.Items.Add("4-Wheels");
            // Add any other types if they are consistently used
            // cmbVehicleType.Items.Add("6-Wheels"); // Example
            cmbTypeOfVehicle.SelectedIndex = -1; // No default selection on start
        }

        private void SetupDurationComboBox()
        {
            cbDuration.Items.Clear();
            cbDuration.Items.Add("Daily");
            cbDuration.Items.Add("Weekly");
            cbDuration.Items.Add("Monthly");
            cbDuration.Items.Add("Yearly");
            cbDuration.SelectedIndex = -1; // No default selection
        }

        private async Task LoadFeesToDataGridView()
        {
            try
            {
                var fees = await _context.Fees.ToListAsync();
                dgvFeeList.DataSource = fees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading fees: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFeeInputFields()
        {
            cmbTypeOfVehicle.SelectedIndex = -1; // No item selected
            txtInputFee.Clear();
            _selectedFee = null; // Clear selected fee object
            btnSaveFee.Text = "Save"; // <--- Ensure button text is always "Save"
            cmbTypeOfVehicle.Enabled = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbTypeOfVehicle.SelectedItem == null)
            {
                MessageBox.Show("Please select a Vehicle Type.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtInputFee.Text, out decimal feeValue) || feeValue < 0)
            {
                MessageBox.Show("Please enter a valid positive number for Fee Per Hour.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedType = cmbTypeOfVehicle.SelectedItem.ToString();

            try
            {
                // First, try to find an existing fee based on the selected VehicleType
                Fee feeToSave = await _context.Fees.FirstOrDefaultAsync(f => f.VehicleType == selectedType);

                if (feeToSave != null)
                {
                    // If an existing fee for this VehicleType is found, UPDATE it
                    feeToSave.FixedPrice = feeValue;
                    // EF Core automatically tracks changes to 'feeToSave' because it was fetched from the context
                    MessageBox.Show($"Fee for '{selectedType}' has been updated successfully!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // If no existing fee for this VehicleType is found, ADD a new one
                    feeToSave = new Fee
                    {
                        VehicleType = selectedType,
                        FixedPrice = feeValue
                    };
                    _context.Fees.Add(feeToSave);
                    MessageBox.Show($"New fee for '{selectedType}' saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                await _context.SaveChangesAsync(); // Save changes to the database

                await LoadFeesToDataGridView(); // Reload the DGV to show updated data
                ClearFeeInputFields();         // Clear inputs for the next operation
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Error saving/updating fee: {dbEx.InnerException?.Message ?? dbEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFeeList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFeeList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFeeList.SelectedRows[0];
                if (selectedRow.DataBoundItem is Fee fee)
                {
                    _selectedFee = fee; // Store the selected Fee object
                    cmbTypeOfVehicle.SelectedItem = _selectedFee.VehicleType;
                    txtInputFee.Text = _selectedFee.FixedPrice.ToString("N2");

                    
                    cmbTypeOfVehicle.Enabled = false;
                }
            }
            else
            {
                // If no row is selected (e.g., after clearing), reset the form
                ClearFeeInputFields();
            }
        }
    }
}
