namespace ParkingManagement.Forms
{
    partial class WeeklySum
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
            label2 = new Label();
            dgvWeeklyRep = new DataGridView();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnMonthlySums = new Button();
            btnDailysums = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvWeeklyRep).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(366, 124);
            label2.Name = "label2";
            label2.Size = new Size(272, 44);
            label2.TabIndex = 8;
            label2.Text = "Weekly Financial Summaries";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvWeeklyRep
            // 
            dgvWeeklyRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWeeklyRep.Location = new Point(65, 181);
            dgvWeeklyRep.Name = "dgvWeeklyRep";
            dgvWeeklyRep.Size = new Size(840, 435);
            dgvWeeklyRep.TabIndex = 7;
            dgvWeeklyRep.CellContentClick += dgvWeeklyRep_CellContentClick;
            // 
            // label6
            // 
            label6.AccessibleRole = AccessibleRole.None;
            label6.AutoSize = true;
            label6.Location = new Point(474, 55);
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
            panel2.Location = new Point(65, 73);
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
            // btnMonthlySums
            // 
            btnMonthlySums.Location = new Point(244, 633);
            btnMonthlySums.Name = "btnMonthlySums";
            btnMonthlySums.Size = new Size(131, 33);
            btnMonthlySums.TabIndex = 15;
            btnMonthlySums.Text = "Monthly Summaries";
            btnMonthlySums.UseVisualStyleBackColor = true;
            btnMonthlySums.Click += btnMonthlySums_Click;
            // 
            // btnDailysums
            // 
            btnDailysums.Location = new Point(83, 633);
            btnDailysums.Name = "btnDailysums";
            btnDailysums.Size = new Size(131, 33);
            btnDailysums.TabIndex = 14;
            btnDailysums.Text = "Daily Summaries";
            btnDailysums.UseVisualStyleBackColor = true;
            btnDailysums.Click += btnDailysums_Click;
            // 
            // WeeklySum
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 749);
            Controls.Add(btnMonthlySums);
            Controls.Add(btnDailysums);
            Controls.Add(label6);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(dgvWeeklyRep);
            Name = "WeeklySum";
            Text = "WeeklySum";
            Load += WeeklySum_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWeeklyRep).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private DataGridView dgvWeeklyRep;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnMonthlySums;
        private Button btnDailysums;
    }
}