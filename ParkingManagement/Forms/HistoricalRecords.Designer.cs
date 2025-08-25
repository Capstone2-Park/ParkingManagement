namespace ParkingManagement.Forms
{
    partial class HistoricalRecords
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
            dgvRentRecord = new DataGridView();
            dgvRegRecord = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvRentRecord
            // 
            dgvRentRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRentRecord.Location = new Point(86, 105);
            dgvRentRecord.Name = "dgvRentRecord";
            dgvRentRecord.ReadOnly = true;
            dgvRentRecord.Size = new Size(805, 207);
            dgvRentRecord.TabIndex = 0;
            // 
            // dgvRegRecord
            // 
            dgvRegRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRegRecord.Location = new Point(86, 385);
            dgvRegRecord.Name = "dgvRegRecord";
            dgvRegRecord.ReadOnly = true;
            dgvRegRecord.Size = new Size(805, 207);
            dgvRegRecord.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 28);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 3;
            label2.Text = "Rental Records";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ActiveCaption;
            label3.Location = new Point(109, 342);
            label3.Name = "label3";
            label3.Size = new Size(118, 15);
            label3.TabIndex = 4;
            label3.Text = "Regular Park Records";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(label2);
            panel1.Location = new Point(38, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(896, 673);
            panel1.TabIndex = 5;
            // 
            // HistoricalRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 749);
            Controls.Add(label3);
            Controls.Add(dgvRegRecord);
            Controls.Add(dgvRentRecord);
            Controls.Add(panel1);
            Name = "HistoricalRecords";
            Text = "HistoricalRecords";
            Load += HistoricalRecords_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRentRecord;
        private DataGridView dgvRegRecord;
        private Label label2;
        private Label label3;
        private Panel panel1;
    }
}