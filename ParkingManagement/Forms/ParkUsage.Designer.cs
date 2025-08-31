namespace ParkingManagement.Forms
{
    partial class ParkUsage
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
            pnlParkUsage = new Panel();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnAvailSlot = new Button();
            btnPeakHours = new Button();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pnlParkUsage
            // 
            pnlParkUsage.AutoScroll = true;
            pnlParkUsage.BackColor = SystemColors.ActiveCaption;
            pnlParkUsage.Location = new Point(95, 139);
            pnlParkUsage.Name = "pnlParkUsage";
            pnlParkUsage.Size = new Size(810, 446);
            pnlParkUsage.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(465, 67);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 7;
            label6.Text = "Date Range";
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(dtpFrom);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(dtpTo);
            panel2.Location = new Point(56, 85);
            panel2.Name = "panel2";
            panel2.Size = new Size(883, 48);
            panel2.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(479, 17);
            label5.Name = "label5";
            label5.Size = new Size(19, 15);
            label5.TabIndex = 6;
            label5.Text = "To";
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(200, 11);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 23);
            dtpFrom.TabIndex = 0;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(122, 17);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 5;
            label4.Text = "Date from";
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(516, 11);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(200, 23);
            dtpTo.TabIndex = 4;
            dtpTo.ValueChanged += dtpTo_ValueChanged_1;
            // 
            // btnAvailSlot
            // 
            btnAvailSlot.Location = new Point(974, 511);
            btnAvailSlot.Name = "btnAvailSlot";
            btnAvailSlot.Size = new Size(131, 33);
            btnAvailSlot.TabIndex = 13;
            btnAvailSlot.Text = "AvailableSlot";
            btnAvailSlot.UseVisualStyleBackColor = true;
            btnAvailSlot.Click += btnAvailSlot_Click;
            // 
            // btnPeakHours
            // 
            btnPeakHours.Location = new Point(974, 460);
            btnPeakHours.Name = "btnPeakHours";
            btnPeakHours.Size = new Size(131, 33);
            btnPeakHours.TabIndex = 12;
            btnPeakHours.Text = "Peak Hours";
            btnPeakHours.UseVisualStyleBackColor = true;
            btnPeakHours.Click += btnPeakHours_Click;
            // 
            // ParkUsage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1166, 588);
            Controls.Add(btnAvailSlot);
            Controls.Add(label6);
            Controls.Add(btnPeakHours);
            Controls.Add(panel2);
            Controls.Add(pnlParkUsage);
            Name = "ParkUsage";
            Text = "ParkUsage";
            Load += ParkUsage_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlParkUsage;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnAvailSlot;
        private Button btnPeakHours;
    }
}