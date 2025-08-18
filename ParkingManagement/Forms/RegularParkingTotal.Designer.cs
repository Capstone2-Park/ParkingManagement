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
            txtCash = new TextBox();
            label1 = new Label();
            rtbChange = new RichTextBox();
            label2 = new Label();
            btnConfirm = new Button();
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
            rtbReceipt.Size = new Size(351, 200);
            rtbReceipt.TabIndex = 1;
            rtbReceipt.Text = "";
            // 
            // btnOn
            // 
            btnOn.Location = new Point(223, 493);
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
            // txtCash
            // 
            txtCash.Location = new Point(741, 348);
            txtCash.Name = "txtCash";
            txtCash.Size = new Size(175, 23);
            txtCash.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(832, 330);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 6;
            label1.Text = "Caash on Hand";
            // 
            // rtbChange
            // 
            rtbChange.Location = new Point(626, 442);
            rtbChange.Name = "rtbChange";
            rtbChange.Size = new Size(290, 92);
            rtbChange.TabIndex = 7;
            rtbChange.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(868, 424);
            label2.Name = "label2";
            label2.Size = new Size(48, 15);
            label2.TabIndex = 8;
            label2.Text = "Change";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(827, 377);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(89, 25);
            btnConfirm.TabIndex = 9;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // RegularParkingTotal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 720);
            Controls.Add(btnConfirm);
            Controls.Add(label2);
            Controls.Add(rtbChange);
            Controls.Add(label1);
            Controls.Add(txtCash);
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
        private TextBox txtCash;
        private Label label1;
        private RichTextBox rtbChange;
        private Label label2;
        private Button btnConfirm;
    }
}