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
            txtPlateNumber = new TextBox();
            cmbTypeOfVehicle = new ComboBox();
            dgvTimeInTimeOutData = new DataGridView();
            pcbQRcode = new PictureBox();
            btnTimeIn = new Button();
            btnTimeOut = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvTimeInTimeOutData).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pcbQRcode).BeginInit();
            SuspendLayout();
            // 
            // txtVehicleId
            // 
            txtVehicleId.Enabled = false;
            txtVehicleId.Location = new Point(271, 154);
            txtVehicleId.Name = "txtVehicleId";
            txtVehicleId.Size = new Size(203, 23);
            txtVehicleId.TabIndex = 0;
            // 
            // txtPlateNumber
            // 
            txtPlateNumber.Location = new Point(271, 209);
            txtPlateNumber.Name = "txtPlateNumber";
            txtPlateNumber.Size = new Size(203, 23);
            txtPlateNumber.TabIndex = 1;
            // 
            // cmbTypeOfVehicle
            // 
            cmbTypeOfVehicle.FormattingEnabled = true;
            cmbTypeOfVehicle.Location = new Point(271, 270);
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
            // pcbQRcode
            // 
            pcbQRcode.BackColor = SystemColors.ControlLight;
            pcbQRcode.Location = new Point(674, 104);
            pcbQRcode.Name = "pcbQRcode";
            pcbQRcode.Size = new Size(233, 233);
            pcbQRcode.TabIndex = 4;
            pcbQRcode.TabStop = false;
            // 
            // btnTimeIn
            // 
            btnTimeIn.Location = new Point(811, 560);
            btnTimeIn.Name = "btnTimeIn";
            btnTimeIn.Size = new Size(75, 23);
            btnTimeIn.TabIndex = 5;
            btnTimeIn.Text = "Time in";
            btnTimeIn.UseVisualStyleBackColor = true;
            btnTimeIn.Click += btnTimeIn_Click;
            // 
            // btnTimeOut
            // 
            btnTimeOut.Location = new Point(917, 560);
            btnTimeOut.Name = "btnTimeOut";
            btnTimeOut.Size = new Size(75, 23);
            btnTimeOut.TabIndex = 6;
            btnTimeOut.Text = "Time out";
            btnTimeOut.UseVisualStyleBackColor = true;
            btnTimeOut.Click += btnTimeOut_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(122, 162);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 7;
            label1.Text = "Vehicle ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(127, 217);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 8;
            label2.Text = "Plate No.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(127, 278);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 9;
            label3.Text = "Type of Vehicle";
            // 
            // Regular_Parking
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1138, 734);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnTimeOut);
            Controls.Add(btnTimeIn);
            Controls.Add(pcbQRcode);
            Controls.Add(dgvTimeInTimeOutData);
            Controls.Add(cmbTypeOfVehicle);
            Controls.Add(txtPlateNumber);
            Controls.Add(txtVehicleId);
            Name = "Regular_Parking";
            Text = "Regular_Parking";
            FormClosing += Regular_Parking_FormClosing;
            Load += Regular_Parking_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTimeInTimeOutData).EndInit();
            ((System.ComponentModel.ISupportInitialize)pcbQRcode).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtVehicleId;
        private TextBox txtPlateNumber;
        private ComboBox cmbTypeOfVehicle;
        private DataGridView dgvTimeInTimeOutData;
        private PictureBox pcbQRcode;
        private Button btnTimeIn;
        private Button btnTimeOut;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}