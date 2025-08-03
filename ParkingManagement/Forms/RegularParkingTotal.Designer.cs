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
            btnOff = new Button();
            ((System.ComponentModel.ISupportInitialize)ptbCheckOut).BeginInit();
            SuspendLayout();
            // 
            // ptbCheckOut
            // 
            ptbCheckOut.BackColor = SystemColors.ActiveCaption;
            ptbCheckOut.Location = new Point(120, 86);
            ptbCheckOut.Name = "ptbCheckOut";
            ptbCheckOut.Size = new Size(232, 194);
            ptbCheckOut.TabIndex = 0;
            ptbCheckOut.TabStop = false;
            // 
            // rtbReceipt
            // 
            rtbReceipt.Location = new Point(451, 86);
            rtbReceipt.Name = "rtbReceipt";
            rtbReceipt.Size = new Size(351, 492);
            rtbReceipt.TabIndex = 1;
            rtbReceipt.Text = "";
            // 
            // btnOn
            // 
            btnOn.Location = new Point(120, 298);
            btnOn.Name = "btnOn";
            btnOn.Size = new Size(106, 30);
            btnOn.TabIndex = 2;
            btnOn.Text = "Camera On";
            btnOn.UseVisualStyleBackColor = true;
            btnOn.Click += btnOn_Click;
            // 
            // btnOff
            // 
            btnOff.Location = new Point(246, 298);
            btnOff.Name = "btnOff";
            btnOff.Size = new Size(106, 30);
            btnOff.TabIndex = 3;
            btnOff.Text = "Camera Off";
            btnOff.UseVisualStyleBackColor = true;
            btnOff.Click += btnOff_Click;
            // 
            // RegularParkingTotal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(930, 590);
            Controls.Add(btnOff);
            Controls.Add(btnOn);
            Controls.Add(rtbReceipt);
            Controls.Add(ptbCheckOut);
            Name = "RegularParkingTotal";
            Text = "RegularParkingTotal";
            ((System.ComponentModel.ISupportInitialize)ptbCheckOut).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox ptbCheckOut;
        private RichTextBox rtbReceipt;
        private Button btnOn;
        private Button btnOff;
    }
}