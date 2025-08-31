namespace ParkingManagement.Forms
{
    partial class ReportManagement
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
            panelChart = new Panel();
            btnSave = new Button();
            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();
            label4 = new Label();
            label5 = new Label();
            panel2 = new Panel();
            label6 = new Label();
            btnParkUse = new Button();
            btnAvailSlot = new Button();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panelChart
            // 
            panelChart.BackColor = SystemColors.ActiveCaption;
            panelChart.Location = new Point(65, 139);
            panelChart.Name = "panelChart";
            panelChart.Size = new Size(883, 483);
            panelChart.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1000, 543);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(131, 33);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save Report";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(200, 11);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 23);
            dtpFrom.TabIndex = 0;
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(516, 11);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(200, 23);
            dtpTo.TabIndex = 4;
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
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(479, 17);
            label5.Name = "label5";
            label5.Size = new Size(19, 15);
            label5.TabIndex = 6;
            label5.Text = "To";
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(dtpFrom);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(dtpTo);
            panel2.Location = new Point(65, 70);
            panel2.Name = "panel2";
            panel2.Size = new Size(883, 48);
            panel2.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(474, 52);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 5;
            label6.Text = "Date Range";
            // 
            // btnParkUse
            // 
            btnParkUse.Location = new Point(1000, 436);
            btnParkUse.Name = "btnParkUse";
            btnParkUse.Size = new Size(131, 33);
            btnParkUse.TabIndex = 10;
            btnParkUse.Text = "Parking Usage";
            btnParkUse.UseVisualStyleBackColor = true;
            btnParkUse.Click += btnParkUse_Click;
            // 
            // btnAvailSlot
            // 
            btnAvailSlot.Location = new Point(1000, 487);
            btnAvailSlot.Name = "btnAvailSlot";
            btnAvailSlot.Size = new Size(131, 33);
            btnAvailSlot.TabIndex = 11;
            btnAvailSlot.Text = "AvailableSlot";
            btnAvailSlot.UseVisualStyleBackColor = true;
            btnAvailSlot.Click += btnAvailSlot_Click;
            // 
            // ReportManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1166, 588);
            Controls.Add(btnSave);
            Controls.Add(btnAvailSlot);
            Controls.Add(btnParkUse);
            Controls.Add(label6);
            Controls.Add(panel2);
            Controls.Add(panelChart);
            Name = "ReportManagement";
            Text = "ReportManagement";
            Load += ReportManagement_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelChart;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Label label4;
        private Label label5;
        private Panel panel2;
        private Label label6;
        private Button btnSave;
        private Button btnParkUse;
        private Button btnAvailSlot;
    }
}