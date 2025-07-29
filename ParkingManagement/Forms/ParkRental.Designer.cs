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
            dtpDateStart = new DateTimePicker();
            label2 = new Label();
            cmbDurationType = new ComboBox();
            label3 = new Label();
            btnSave = new Button();
            btnSetSched = new Button();
            btnCancel = new Button();
            lblClientName = new Label();
            dgvList = new DataGridView();
            label5 = new Label();
            cbTime = new ComboBox();
            lblVSelect = new Label();
            label6 = new Label();
            cbVehicle = new ComboBox();
            dtpTime = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvList).BeginInit();
            SuspendLayout();
            // 
            // dtpDateStart
            // 
            dtpDateStart.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpDateStart.Location = new Point(292, 273);
            dtpDateStart.Name = "dtpDateStart";
            dtpDateStart.Size = new Size(206, 23);
            dtpDateStart.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(220, 273);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 5;
            label2.Text = "Date Start";
            // 
            // cmbDurationType
            // 
            cmbDurationType.FormattingEnabled = true;
            cmbDurationType.Location = new Point(292, 203);
            cmbDurationType.Name = "cmbDurationType";
            cmbDurationType.Size = new Size(121, 23);
            cmbDurationType.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(198, 206);
            label3.Name = "label3";
            label3.Size = new Size(80, 15);
            label3.TabIndex = 7;
            label3.Text = "Duration Type";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(834, 580);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Next";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnSetSched
            // 
            btnSetSched.Location = new Point(194, 324);
            btnSetSched.Name = "btnSetSched";
            btnSetSched.Size = new Size(92, 23);
            btnSetSched.TabIndex = 9;
            btnSetSched.Text = "Set Schedule";
            btnSetSched.UseVisualStyleBackColor = true;
            btnSetSched.Click += btnSetSched_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(933, 580);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblClientName
            // 
            lblClientName.AutoSize = true;
            lblClientName.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblClientName.Location = new Point(198, 158);
            lblClientName.Name = "lblClientName";
            lblClientName.Size = new Size(144, 25);
            lblClientName.TabIndex = 12;
            lblClientName.Text = "Name of Client";
            // 
            // dgvList
            // 
            dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvList.Location = new Point(198, 400);
            dgvList.Name = "dgvList";
            dgvList.Size = new Size(831, 150);
            dgvList.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(195, 367);
            label5.Name = "label5";
            label5.Size = new Size(171, 25);
            label5.TabIndex = 18;
            label5.Text = "Scheduled Vehicle";
            label5.Click += label5_Click;
            // 
            // cbTime
            // 
            cbTime.FormattingEnabled = true;
            cbTime.Location = new Point(292, 235);
            cbTime.Name = "cbTime";
            cbTime.Size = new Size(121, 23);
            cbTime.TabIndex = 21;
            // 
            // lblVSelect
            // 
            lblVSelect.AutoSize = true;
            lblVSelect.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVSelect.Location = new Point(618, 158);
            lblVSelect.Name = "lblVSelect";
            lblVSelect.Size = new Size(160, 25);
            lblVSelect.TabIndex = 22;
            lblVSelect.Text = "Vehicle Selection";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(196, 235);
            label6.Name = "label6";
            label6.Size = new Size(82, 15);
            label6.TabIndex = 23;
            label6.Text = "Hours Per Day";
            // 
            // cbVehicle
            // 
            cbVehicle.FormattingEnabled = true;
            cbVehicle.Location = new Point(626, 198);
            cbVehicle.Name = "cbVehicle";
            cbVehicle.Size = new Size(140, 23);
            cbVehicle.TabIndex = 25;
            // 
            // dtpTime
            // 
            dtpTime.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpTime.Format = DateTimePickerFormat.Time;
            dtpTime.Location = new Point(504, 273);
            dtpTime.Name = "dtpTime";
            dtpTime.ShowUpDown = true;
            dtpTime.Size = new Size(93, 23);
            dtpTime.TabIndex = 26;
            // 
            // ParkRental
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 749);
            Controls.Add(dtpTime);
            Controls.Add(cbVehicle);
            Controls.Add(label6);
            Controls.Add(lblVSelect);
            Controls.Add(cbTime);
            Controls.Add(label5);
            Controls.Add(dgvList);
            Controls.Add(lblClientName);
            Controls.Add(btnCancel);
            Controls.Add(btnSetSched);
            Controls.Add(btnSave);
            Controls.Add(label3);
            Controls.Add(cmbDurationType);
            Controls.Add(label2);
            Controls.Add(dtpDateStart);
            Name = "ParkRental";
            FormClosing += ParkRental_FormClosing;
            Load += ParkRental_Load_1;
            ((System.ComponentModel.ISupportInitialize)dgvList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DateTimePicker dtpDateStart;
        private Label label2;
        private ComboBox cmbDurationType;
        private Label label3;
        private Button btnSave;
        private Button btnSetSched;
        private Button btnCancel;
        private Label lblClientName;
        private DataGridView dgvList;
        private Label label5;
        private ComboBox cbTime;
        private Label lblVSelect;
        private Label label6;
        private ComboBox cbVehicle;
        private DateTimePicker dtpTime;
    }
}