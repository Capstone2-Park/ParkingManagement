namespace ParkingManagement
{
    partial class ClientManagement
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtClientID = new TextBox();
            txtName = new TextBox();
            txtAddress = new TextBox();
            txtContactNo = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pcbIDPic = new PictureBox();
            btnStartCamera = new Button();
            btnCaptureImage = new Button();
            btnRetakeImage = new Button();
            panel1 = new Panel();
            txtVehicleIDNo = new TextBox();
            txtBrand = new TextBox();
            txtColor = new TextBox();
            txtPlateNo = new TextBox();
            cmbVehicleType = new ComboBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            btnAddVehicle = new Button();
            btnRemoveVehicle = new Button();
            dgvInformation = new DataGridView();
            btnSave = new Button();
            btnCancel = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pcbIDPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvInformation).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtClientID
            // 
            txtClientID.Location = new Point(176, 74);
            txtClientID.Name = "txtClientID";
            txtClientID.Size = new Size(191, 23);
            txtClientID.TabIndex = 0;
            // 
            // txtName
            // 
            txtName.Location = new Point(176, 141);
            txtName.Name = "txtName";
            txtName.Size = new Size(191, 23);
            txtName.TabIndex = 1;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(176, 209);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(191, 23);
            txtAddress.TabIndex = 2;
            // 
            // txtContactNo
            // 
            txtContactNo.Location = new Point(176, 279);
            txtContactNo.Name = "txtContactNo";
            txtContactNo.Size = new Size(191, 23);
            txtContactNo.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(78, 82);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 4;
            label1.Text = "Client ID No.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(102, 149);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 5;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(92, 217);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 6;
            label3.Text = "Address";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(70, 287);
            label4.Name = "label4";
            label4.Size = new Size(71, 15);
            label4.TabIndex = 7;
            label4.Text = "Contact No.";
            // 
            // pcbIDPic
            // 
            pcbIDPic.BackColor = SystemColors.ActiveCaption;
            pcbIDPic.Location = new Point(701, 82);
            pcbIDPic.Name = "pcbIDPic";
            pcbIDPic.Size = new Size(327, 193);
            pcbIDPic.TabIndex = 8;
            pcbIDPic.TabStop = false;
            // 
            // btnStartCamera
            // 
            btnStartCamera.Location = new Point(1072, 95);
            btnStartCamera.Name = "btnStartCamera";
            btnStartCamera.Size = new Size(100, 23);
            btnStartCamera.TabIndex = 9;
            btnStartCamera.Text = "Start Camera";
            btnStartCamera.UseVisualStyleBackColor = true;
            btnStartCamera.Click += btnStartCamera_Click;
            // 
            // btnCaptureImage
            // 
            btnCaptureImage.Location = new Point(1072, 149);
            btnCaptureImage.Name = "btnCaptureImage";
            btnCaptureImage.Size = new Size(100, 23);
            btnCaptureImage.TabIndex = 10;
            btnCaptureImage.Text = "Capture Image";
            btnCaptureImage.UseVisualStyleBackColor = true;
            btnCaptureImage.Click += btnCaptureImage_Click;
            // 
            // btnRetakeImage
            // 
            btnRetakeImage.Location = new Point(1072, 188);
            btnRetakeImage.Name = "btnRetakeImage";
            btnRetakeImage.Size = new Size(100, 23);
            btnRetakeImage.TabIndex = 11;
            btnRetakeImage.Text = "Retake";
            btnRetakeImage.UseVisualStyleBackColor = true;
            btnRetakeImage.Click += btnRetakeImage_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(128, 64, 64);
            panel1.Location = new Point(0, 342);
            panel1.Name = "panel1";
            panel1.Size = new Size(1266, 19);
            panel1.TabIndex = 12;
            // 
            // txtVehicleIDNo
            // 
            txtVehicleIDNo.Location = new Point(248, 403);
            txtVehicleIDNo.Name = "txtVehicleIDNo";
            txtVehicleIDNo.Size = new Size(191, 23);
            txtVehicleIDNo.TabIndex = 13;
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(248, 460);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(191, 23);
            txtBrand.TabIndex = 14;
            // 
            // txtColor
            // 
            txtColor.Location = new Point(248, 527);
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(191, 23);
            txtColor.TabIndex = 15;
            // 
            // txtPlateNo
            // 
            txtPlateNo.Location = new Point(769, 403);
            txtPlateNo.Name = "txtPlateNo";
            txtPlateNo.Size = new Size(191, 23);
            txtPlateNo.TabIndex = 16;
            // 
            // cmbVehicleType
            // 
            cmbVehicleType.FormattingEnabled = true;
            cmbVehicleType.Location = new Point(769, 460);
            cmbVehicleType.Name = "cmbVehicleType";
            cmbVehicleType.Size = new Size(191, 23);
            cmbVehicleType.TabIndex = 17;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(142, 411);
            label5.Name = "label5";
            label5.Size = new Size(80, 15);
            label5.TabIndex = 18;
            label5.Text = "Vehicle ID No.";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(183, 468);
            label6.Name = "label6";
            label6.Size = new Size(38, 15);
            label6.TabIndex = 19;
            label6.Text = "Brand";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(183, 535);
            label7.Name = "label7";
            label7.Size = new Size(36, 15);
            label7.TabIndex = 20;
            label7.Text = "Color";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(684, 411);
            label8.Name = "label8";
            label8.Size = new Size(55, 15);
            label8.TabIndex = 21;
            label8.Text = "Plate No.";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(667, 468);
            label9.Name = "label9";
            label9.Size = new Size(71, 15);
            label9.TabIndex = 22;
            label9.Text = "Vehicle Type";
            // 
            // btnAddVehicle
            // 
            btnAddVehicle.Location = new Point(1034, 407);
            btnAddVehicle.Name = "btnAddVehicle";
            btnAddVehicle.Size = new Size(89, 23);
            btnAddVehicle.TabIndex = 23;
            btnAddVehicle.Text = "Add Vehicle";
            btnAddVehicle.UseVisualStyleBackColor = true;
            btnAddVehicle.Click += btnAddVehicle_Click;
            // 
            // btnRemoveVehicle
            // 
            btnRemoveVehicle.Location = new Point(1034, 461);
            btnRemoveVehicle.Name = "btnRemoveVehicle";
            btnRemoveVehicle.Size = new Size(89, 23);
            btnRemoveVehicle.TabIndex = 24;
            btnRemoveVehicle.Text = "Remove Vehicle";
            btnRemoveVehicle.UseVisualStyleBackColor = true;
            btnRemoveVehicle.Click += btnRemoveVehicle_Click;
            // 
            // dgvInformation
            // 
            dgvInformation.Anchor = AnchorStyles.None;
            dgvInformation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInformation.Location = new Point(3, 3);
            dgvInformation.Name = "dgvInformation";
            dgvInformation.Size = new Size(770, 184);
            dgvInformation.TabIndex = 25;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.Location = new Point(3, 193);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 36);
            btnSave.TabIndex = 26;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.Location = new Point(84, 193);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 36);
            btnCancel.TabIndex = 27;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(dgvInformation);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Controls.Add(btnCancel);
            flowLayoutPanel1.Location = new Point(324, 594);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(773, 291);
            flowLayoutPanel1.TabIndex = 28;
            // 
            // ClientManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1266, 749);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btnRemoveVehicle);
            Controls.Add(btnAddVehicle);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(cmbVehicleType);
            Controls.Add(txtPlateNo);
            Controls.Add(txtColor);
            Controls.Add(txtBrand);
            Controls.Add(txtVehicleIDNo);
            Controls.Add(panel1);
            Controls.Add(btnRetakeImage);
            Controls.Add(btnCaptureImage);
            Controls.Add(btnStartCamera);
            Controls.Add(pcbIDPic);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtContactNo);
            Controls.Add(txtAddress);
            Controls.Add(txtName);
            Controls.Add(txtClientID);
            Name = "ClientManagement";
            Text = "Rent";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pcbIDPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvInformation).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtClientID;
        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtContactNo;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox pcbIDPic;
        private Button btnStartCamera;
        private Button btnCaptureImage;
        private Button btnRetakeImage;
        private Panel panel1;
        private TextBox txtVehicleIDNo;
        private TextBox txtBrand;
        private TextBox txtColor;
        private TextBox txtPlateNo;
        private ComboBox cmbVehicleType;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button btnAddVehicle;
        private Button btnRemoveVehicle;
        private DataGridView dgvInformation;
        private Button btnSave;
        private Button btnCancel;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
