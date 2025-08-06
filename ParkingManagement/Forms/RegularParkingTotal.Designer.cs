namespace ParkingManagement.Forms
{
    partial class RegularParkingTotal
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
            ptbCheckOut = new PictureBox();
            rtbReceipt = new RichTextBox();
            btnOn = new Button();
            lblScanStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)ptbCheckOut).BeginInit();
            SuspendLayout();
            // 
            // ptbCheckOut
            // 
            ptbCheckOut.BackColor = SystemColors.ActiveCaption;
            ptbCheckOut.Location = new Point(49, 121);
            ptbCheckOut.Name = "ptbCheckOut";
            ptbCheckOut.Size = new Size(452, 311);
            ptbCheckOut.SizeMode = PictureBoxSizeMode.StretchImage;
            ptbCheckOut.TabIndex = 0;
            ptbCheckOut.TabStop = false;
            // 
            // rtbReceipt
            // 
            rtbReceipt.Location = new Point(565, 121);
            rtbReceipt.Name = "rtbReceipt";
            rtbReceipt.Size = new Size(351, 367);
            rtbReceipt.TabIndex = 1;
            rtbReceipt.Text = "";
            // 
            // btnOn
            // 
            btnOn.Location = new Point(141, 516);
            btnOn.Name = "btnOn";
            btnOn.Size = new Size(106, 30);
            btnOn.TabIndex = 2;
            btnOn.Text = "Camera On";
            btnOn.UseVisualStyleBackColor = true;
            btnOn.Click += btnOn_Click;
            // 
            // lblScanStatus
            // 
            lblScanStatus.AutoSize = true;
            lblScanStatus.Location = new Point(264, 90);
            lblScanStatus.Name = "lblScanStatus";
            lblScanStatus.Size = new Size(38, 15);
            lblScanStatus.TabIndex = 4;
            lblScanStatus.Text = "label1";
            // 
            // RegularParkingTotal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 720);
            Controls.Add(lblScanStatus);
            Controls.Add(btnOn);
            Controls.Add(rtbReceipt);
            Controls.Add(ptbCheckOut);
            Name = "RegularParkingTotal";
            Text = "RegularParkingTotal";
            Load += RegularParkingTotal_Load;
            ((System.ComponentModel.ISupportInitialize)ptbCheckOut).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox ptbCheckOut;
        private RichTextBox rtbReceipt;
        private Button btnOn;
        private Label lblScanStatus;
    }
}