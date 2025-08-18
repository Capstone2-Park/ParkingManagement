namespace ParkingManagement.Forms
{
    partial class Regular_Parking
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
            txtVehicleId = new TextBox();
            cmbTypeOfVehicle = new ComboBox();
            dgvTimeInTimeOutData = new DataGridView();
            btnTimeIn = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtAvailableSlotM = new TextBox();
            txtAvailableSlotV = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtOccupiedSlotV = new TextBox();
            label8 = new Label();
            txtOccupiedSlotM = new TextBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTimeInTimeOutData).BeginInit();
            SuspendLayout();
            // 
            // txtVehicleId
            // 
            txtVehicleId.Enabled = false;
            txtVehicleId.Location = new Point(271, 186);
            txtVehicleId.Name = "txtVehicleId";
            txtVehicleId.Size = new Size(203, 23);
            txtVehicleId.TabIndex = 0;
            // 
            // cmbTypeOfVehicle
            // 
            cmbTypeOfVehicle.FormattingEnabled = true;
            cmbTypeOfVehicle.Location = new Point(271, 256);
            cmbTypeOfVehicle.Name = "cmbTypeOfVehicle";
            cmbTypeOfVehicle.Size = new Size(203, 23);
            cmbTypeOfVehicle.TabIndex = 2;
            // 
            // dgvTimeInTimeOutData
            // 
            dgvTimeInTimeOutData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTimeInTimeOutData.Location = new Point(127, 388);
            dgvTimeInTimeOutData.Name = "dgvTimeInTimeOutData";
            dgvTimeInTimeOutData.Size = new Size(865, 150);
            dgvTimeInTimeOutData.TabIndex = 3;
            // 
            // btnTimeIn
            // 
            btnTimeIn.Location = new Point(897, 564);
            btnTimeIn.Name = "btnTimeIn";
            btnTimeIn.Size = new Size(95, 39);
            btnTimeIn.TabIndex = 5;
            btnTimeIn.Text = "Time in";
            btnTimeIn.UseVisualStyleBackColor = true;
            btnTimeIn.Click += btnTimeIn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(127, 197);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 7;
            label1.Text = "Vehicle ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(681, 142);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 8;
            label2.Text = "Available Slots";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(127, 264);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 9;
            label3.Text = "Type of Vehicle";
            // 
            // txtAvailableSlotM
            // 
            txtAvailableSlotM.Enabled = false;
            txtAvailableSlotM.Location = new Point(631, 194);
            txtAvailableSlotM.Name = "txtAvailableSlotM";
            txtAvailableSlotM.Size = new Size(57, 23);
            txtAvailableSlotM.TabIndex = 1;
            // 
            // txtAvailableSlotV
            // 
            txtAvailableSlotV.Enabled = false;
            txtAvailableSlotV.Location = new Point(754, 194);
            txtAvailableSlotV.Name = "txtAvailableSlotV";
            txtAvailableSlotV.Size = new Size(57, 23);
            txtAvailableSlotV.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(631, 176);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 11;
            label4.Text = "2-wheels";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(754, 176);
            label5.Name = "label5";
            label5.Size = new Size(54, 15);
            label5.TabIndex = 12;
            label5.Text = "4-wheels";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(751, 294);
            label6.Name = "label6";
            label6.Size = new Size(54, 15);
            label6.TabIndex = 17;
            label6.Text = "4-wheels";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(628, 294);
            label7.Name = "label7";
            label7.Size = new Size(54, 15);
            label7.TabIndex = 16;
            label7.Text = "2-wheels";
            // 
            // txtOccupiedSlotV
            // 
            txtOccupiedSlotV.Enabled = false;
            txtOccupiedSlotV.Location = new Point(751, 312);
            txtOccupiedSlotV.Name = "txtOccupiedSlotV";
            txtOccupiedSlotV.Size = new Size(57, 23);
            txtOccupiedSlotV.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(678, 260);
            label8.Name = "label8";
            label8.Size = new Size(111, 20);
            label8.TabIndex = 14;
            label8.Text = "Occupied Slots";
            // 
            // txtOccupiedSlotM
            // 
            txtOccupiedSlotM.Enabled = false;
            txtOccupiedSlotM.Location = new Point(628, 312);
            txtOccupiedSlotM.Name = "txtOccupiedSlotM";
            txtOccupiedSlotM.Size = new Size(57, 23);
            txtOccupiedSlotM.TabIndex = 13;
            // 
            // button1
            // 
            button1.Location = new Point(883, 247);
            button1.Name = "button1";
            button1.Size = new Size(95, 39);
            button1.TabIndex = 18;
            button1.Text = "Time in";
            button1.UseVisualStyleBackColor = true;
            // 
            // Regular_Parking
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1138, 734);
            Controls.Add(button1);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(txtOccupiedSlotV);
            Controls.Add(label8);
            Controls.Add(txtOccupiedSlotM);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txtAvailableSlotV);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnTimeIn);
            Controls.Add(dgvTimeInTimeOutData);
            Controls.Add(cmbTypeOfVehicle);
            Controls.Add(txtAvailableSlotM);
            Controls.Add(txtVehicleId);
            Name = "Regular_Parking";
            Text = "Regular_Parking";
            FormClosing += Regular_Parking_FormClosing;
            Load += Regular_Parking_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTimeInTimeOutData).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtVehicleId;
        private ComboBox cmbTypeOfVehicle;
        private DataGridView dgvTimeInTimeOutData;
        private Button btnTimeIn;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtAvailableSlotM;
        private TextBox txtAvailableSlotV;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox txtOccupiedSlotV;
        private Label label8;
        private TextBox txtOccupiedSlotM;
        private Button button1;
    }
}