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
            rtbAvailSlot = new RichTextBox();
            rtbTotalRevenue = new RichTextBox();
            panelChart = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            rtbParkUse = new RichTextBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // rtbAvailSlot
            // 
            rtbAvailSlot.Location = new Point(629, 56);
            rtbAvailSlot.Name = "rtbAvailSlot";
            rtbAvailSlot.Size = new Size(226, 99);
            rtbAvailSlot.TabIndex = 1;
            rtbAvailSlot.Text = "";
            rtbAvailSlot.TextChanged += rtbAvailSlot_TextChanged;
            // 
            // rtbTotalRevenue
            // 
            rtbTotalRevenue.Location = new Point(340, 206);
            rtbTotalRevenue.Name = "rtbTotalRevenue";
            rtbTotalRevenue.Size = new Size(270, 74);
            rtbTotalRevenue.TabIndex = 2;
            rtbTotalRevenue.Text = "";
            rtbTotalRevenue.TextChanged += richTextBox5_TextChanged;
            // 
            // panelChart
            // 
            panelChart.BackColor = SystemColors.ActiveCaption;
            panelChart.Location = new Point(29, 39);
            panelChart.Name = "panelChart";
            panelChart.Size = new Size(1006, 272);
            panelChart.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(163, 25);
            label1.Name = "label1";
            label1.Size = new Size(124, 15);
            label1.TabIndex = 4;
            label1.Text = "Today's Parking Usage";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(689, 28);
            label2.Name = "label2";
            label2.Size = new Size(120, 15);
            label2.TabIndex = 5;
            label2.Text = "Today's Available Slot";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(452, 188);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 6;
            label3.Text = "Revenue";
            // 
            // rtbParkUse
            // 
            rtbParkUse.Location = new Point(118, 53);
            rtbParkUse.Name = "rtbParkUse";
            rtbParkUse.Size = new Size(207, 102);
            rtbParkUse.TabIndex = 0;
            rtbParkUse.Text = "";
            rtbParkUse.TextChanged += rtbParkUse_TextChanged;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(rtbAvailSlot);
            panel1.Controls.Add(rtbParkUse);
            panel1.Controls.Add(rtbTotalRevenue);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(49, 355);
            panel1.Name = "panel1";
            panel1.Size = new Size(966, 310);
            panel1.TabIndex = 0;
            // 
            // ReportManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1090, 702);
            Controls.Add(panelChart);
            Controls.Add(panel1);
            Name = "ReportManagement";
            Text = "ReportManagement";
            Load += ReportManagement_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private RichTextBox rtbAvailSlot;
        private RichTextBox rtbTotalRevenue;
        private Panel panelChart;
        private Label label1;
        private Label label2;
        private Label label3;
        private RichTextBox rtbParkUse;
        private Panel panel1;
    }
}