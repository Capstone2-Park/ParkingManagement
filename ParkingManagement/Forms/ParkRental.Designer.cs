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
            lblClientName = new Label();
            dtpStart = new DateTimePicker();
            label1 = new Label();
            dgvList = new DataGridView();
            label4 = new Label();
            dtpEnd = new DateTimePicker();
            label5 = new Label();
            btnPrev = new Button();
            btnNext = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVehicles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvList).BeginInit();
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
            btnSetSched.Location = new Point(435, 310);
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
            // dtpStart
            // 
            dtpStart.Format = DateTimePickerFormat.Time;
            dtpStart.Location = new Point(293, 267);
            dtpStart.Name = "dtpStart";
            dtpStart.ShowUpDown = true;
            dtpStart.Size = new Size(101, 23);
            dtpStart.TabIndex = 13;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(198, 275);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 14;
            label1.Text = "Time Start";
            // 
            // dgvList
            // 
            dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvList.Location = new Point(756, 183);
            dgvList.Name = "dgvList";
            dgvList.Size = new Size(252, 150);
            dgvList.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(415, 273);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 16;
            label4.Text = "Time Ends";
            // 
            // dtpEnd
            // 
            dtpEnd.Format = DateTimePickerFormat.Time;
            dtpEnd.Location = new Point(482, 267);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.ShowUpDown = true;
            dtpEnd.Size = new Size(101, 23);
            dtpEnd.TabIndex = 17;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(756, 155);
            label5.Name = "label5";
            label5.Size = new Size(166, 25);
            label5.TabIndex = 18;
            label5.Text = "Rented Client List";
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(1071, 659);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(96, 37);
            btnPrev.TabIndex = 19;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(98, 659);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(96, 37);
            btnNext.TabIndex = 20;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // ParkRental
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 749);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(label5);
            Controls.Add(dtpEnd);
            Controls.Add(label4);
            Controls.Add(dgvList);
            Controls.Add(label1);
            Controls.Add(dtpStart);
            Controls.Add(lblClientName);
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
            ((System.ComponentModel.ISupportInitialize)dgvList).EndInit();
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
        private Label lblClientName;
        private DateTimePicker dtpStart;
        private Label label1;
        private DataGridView dgvList;
        private Label label4;
        private DateTimePicker dtpEnd;
        private Label label5;
        private Button btnPrev;
        private Button btnNext;
    }
}