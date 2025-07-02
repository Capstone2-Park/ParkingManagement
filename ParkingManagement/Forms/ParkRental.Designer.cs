namespace ParkingManagement.Forms
{
    partial class ParkRental
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvVehicles = new DataGridView();
            txtSearch = new TextBox();
            dtpDateStart = new DateTimePicker();
            label2 = new Label();
            cmbDurationType = new ComboBox();
            label3 = new Label();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVehicles).BeginInit();
            SuspendLayout();
            // 
            // dgvVehicles
            // 
            dgvVehicles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVehicles.Location = new Point(200, 466);
            dgvVehicles.Name = "dgvVehicles";
            dgvVehicles.Size = new Size(810, 150);
            dgvVehicles.TabIndex = 0;
            dgvVehicles.SelectionChanged += dgvVehicles_SelectionChanged;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(200, 425);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(302, 23);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dtpDateStart
            // 
            dtpDateStart.Location = new Point(295, 178);
            dtpDateStart.Name = "dtpDateStart";
            dtpDateStart.Size = new Size(200, 23);
            dtpDateStart.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(200, 184);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 5;
            label2.Text = "Date Start";
            // 
            // cmbDurationType
            // 
            cmbDurationType.FormattingEnabled = true;
            cmbDurationType.Location = new Point(295, 249);
            cmbDurationType.Name = "cmbDurationType";
            cmbDurationType.Size = new Size(121, 23);
            cmbDurationType.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(197, 257);
            label3.Name = "label3";
            label3.Size = new Size(81, 15);
            label3.TabIndex = 7;
            label3.Text = "Duration Type";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(935, 640);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // ParkRental
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 890);
            Controls.Add(btnSave);
            Controls.Add(label3);
            Controls.Add(cmbDurationType);
            Controls.Add(label2);
            Controls.Add(dtpDateStart);
            Controls.Add(txtSearch);
            Controls.Add(dgvVehicles);
            Name = "ParkRental";
            Text = "Fee";
            FormClosing += ParkRental_FormClosing;
            ((System.ComponentModel.ISupportInitialize)dgvVehicles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvVehicles;
        private TextBox txtSearch;
        private DateTimePicker dtpDateStart;
        private Label label2;
        private ComboBox cmbDurationType;
        private Label label3;
        private Button btnSave;
    }
}