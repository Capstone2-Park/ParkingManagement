namespace ParkingManagement.Forms
{
    partial class MonthlySum
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
            label3 = new Label();
            dgvMonthlyRep = new DataGridView();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnWeeklySums = new Button();
            btnDailysums = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMonthlyRep).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(362, 138);
            label3.Name = "label3";
            label3.Size = new Size(279, 37);
            label3.TabIndex = 7;
            label3.Text = "Monthly Financial Summaries";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvMonthlyRep
            // 
            dgvMonthlyRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMonthlyRep.Location = new Point(65, 221);
            dgvMonthlyRep.Name = "dgvMonthlyRep";
            dgvMonthlyRep.Size = new Size(818, 402);
            dgvMonthlyRep.TabIndex = 6;
            // 
            // label6
            // 
            label6.AccessibleRole = AccessibleRole.None;
            label6.AutoSize = true;
            label6.Location = new Point(474, 69);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 11;
            label6.Text = "Date Range";
            // 
            // panel2
            // 
            panel2.AccessibleRole = AccessibleRole.None;
            panel2.Controls.Add(label5);
            panel2.Controls.Add(dtpFrom);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(dtpTo);
            panel2.Location = new Point(65, 87);
            panel2.Name = "panel2";
            panel2.Size = new Size(883, 48);
            panel2.TabIndex = 10;
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
            // btnWeeklySums
            // 
            btnWeeklySums.Location = new Point(265, 643);
            btnWeeklySums.Name = "btnWeeklySums";
            btnWeeklySums.Size = new Size(131, 33);
            btnWeeklySums.TabIndex = 17;
            btnWeeklySums.Text = "Weekly Summaries";
            btnWeeklySums.UseVisualStyleBackColor = true;
            btnWeeklySums.Click += btnWeeklySums_Click;
            // 
            // btnDailysums
            // 
            btnDailysums.Location = new Point(104, 643);
            btnDailysums.Name = "btnDailysums";
            btnDailysums.Size = new Size(131, 33);
            btnDailysums.TabIndex = 16;
            btnDailysums.Text = "Daily Summaries";
            btnDailysums.UseVisualStyleBackColor = true;
            btnDailysums.Click += btnDailysums_Click;
            // 
            // MonthlySum
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 749);
            Controls.Add(btnWeeklySums);
            Controls.Add(btnDailysums);
            Controls.Add(label6);
            Controls.Add(panel2);
            Controls.Add(label3);
            Controls.Add(dgvMonthlyRep);
            Name = "MonthlySum";
            Text = "MonthlySum";
            Load += MonthlySum_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMonthlyRep).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label3;
        private DataGridView dgvMonthlyRep;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnWeeklySums;
        private Button btnDailysums;
    }
}