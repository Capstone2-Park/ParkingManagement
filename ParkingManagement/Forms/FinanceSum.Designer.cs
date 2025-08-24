namespace ParkingManagement.Forms
{
    partial class FinanceSum
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
            dgvDailyRep = new DataGridView();
            dgvWeeklyRep = new DataGridView();
            dgvMonthlyRep = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvDailyRep).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvWeeklyRep).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMonthlyRep).BeginInit();
            SuspendLayout();
            // 
            // dgvDailyRep
            // 
            dgvDailyRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDailyRep.Location = new Point(172, 37);
            dgvDailyRep.Name = "dgvDailyRep";
            dgvDailyRep.Size = new Size(636, 196);
            dgvDailyRep.TabIndex = 0;
            // 
            // dgvWeeklyRep
            // 
            dgvWeeklyRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWeeklyRep.Location = new Point(172, 283);
            dgvWeeklyRep.Name = "dgvWeeklyRep";
            dgvWeeklyRep.Size = new Size(636, 196);
            dgvWeeklyRep.TabIndex = 1;
            // 
            // dgvMonthlyRep
            // 
            dgvMonthlyRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMonthlyRep.Location = new Point(172, 520);
            dgvMonthlyRep.Name = "dgvMonthlyRep";
            dgvMonthlyRep.Size = new Size(636, 196);
            dgvMonthlyRep.TabIndex = 2;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(47, 106);
            label1.Name = "label1";
            label1.Size = new Size(119, 85);
            label1.TabIndex = 3;
            label1.Text = "Daily Financial Summaries";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(47, 345);
            label2.Name = "label2";
            label2.Size = new Size(119, 85);
            label2.TabIndex = 4;
            label2.Text = "Weekly Financial Summaries";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(47, 582);
            label3.Name = "label3";
            label3.Size = new Size(119, 85);
            label3.TabIndex = 5;
            label3.Text = "Monthly Financial Summaries";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FinanceSum
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 749);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvMonthlyRep);
            Controls.Add(dgvWeeklyRep);
            Controls.Add(dgvDailyRep);
            Name = "FinanceSum";
            Text = "FinanceSum";
            Load += FinanceSum_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDailyRep).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvWeeklyRep).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMonthlyRep).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvDailyRep;
        private DataGridView dgvWeeklyRep;
        private DataGridView dgvMonthlyRep;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}