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
            dtpDateStart = new DateTimePicker();
            label2 = new Label();
            cmbDurationType = new ComboBox();
            label3 = new Label();
            btnSave = new Button();
            btnSetSched = new Button();
            btnCancel = new Button();
            lstScheduled = new ListBox();
            lblClientName = new Label();
            dateTimePicker1 = new DateTimePicker();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvVehicles).BeginInit();
            SuspendLayout();
            // 
            // dgvVehicles
            // 
            dgvVehicles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVehicles.Location = new Point(198, 402);
            dgvVehicles.Name = "dgvVehicles";
            dgvVehicles.Size = new Size(810, 150);
            dgvVehicles.TabIndex = 0;
            dgvVehicles.SelectionChanged += dgvVehicles_SelectionChanged;
            // 
            // dtpDateStart
            // 
            dtpDateStart.Location = new Point(293, 225);
            dtpDateStart.Name = "dtpDateStart";
            dtpDateStart.Size = new Size(200, 23);
            dtpDateStart.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(198, 231);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 5;
            label2.Text = "Date Start";
            // 
            // cmbDurationType
            // 
            cmbDurationType.FormattingEnabled = true;
            cmbDurationType.Location = new Point(293, 311);
            cmbDurationType.Name = "cmbDurationType";
            cmbDurationType.Size = new Size(121, 23);
            cmbDurationType.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(195, 319);
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
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnSetSched
            // 
            btnSetSched.Location = new Point(817, 162);
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
            // lstScheduled
            // 
            lstScheduled.FormattingEnabled = true;
            lstScheduled.ItemHeight = 15;
            lstScheduled.Location = new Point(583, 162);
            lstScheduled.Name = "lstScheduled";
            lstScheduled.Size = new Size(221, 199);
            lstScheduled.TabIndex = 11;
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
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.Location = new Point(293, 267);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(101, 23);
            dateTimePicker1.TabIndex = 13;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(198, 275);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 14;
            label1.Text = "Set Time";
            // 
            // ParkRental
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 749);
            Controls.Add(label1);
            Controls.Add(dateTimePicker1);
            Controls.Add(lblClientName);
            Controls.Add(lstScheduled);
            Controls.Add(btnCancel);
            Controls.Add(btnSetSched);
            Controls.Add(btnSave);
            Controls.Add(label3);
            Controls.Add(cmbDurationType);
            Controls.Add(label2);
            Controls.Add(dtpDateStart);
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
        private DateTimePicker dtpDateStart;
        private Label label2;
        private ComboBox cmbDurationType;
        private Label label3;
        private Button btnSave;
        private Button btnSetSched;
        private Button btnCancel;
        private ListBox lstScheduled;
        private Label lblClientName;
        private DateTimePicker dateTimePicker1;
        private Label label1;
    }
}