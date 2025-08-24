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
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).BeginInit();
            SuspendLayout();
            // 
            // dgvRentRecord
            // 
            dgvRentRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRentRecord.Location = new Point(86, 105);
            dgvRentRecord.Name = "dgvRentRecord";
            dgvRentRecord.Size = new Size(805, 207);
            dgvRentRecord.TabIndex = 0;
            // 
            // dgvRegRecord
            // 
            dgvRegRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRegRecord.Location = new Point(86, 385);
            dgvRegRecord.Name = "dgvRegRecord";
            dgvRegRecord.Size = new Size(805, 207);
            dgvRegRecord.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(109, 58);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 3;
            label2.Text = "Rental Records";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(109, 342);
            label3.Name = "label3";
            label3.Size = new Size(118, 15);
            label3.TabIndex = 4;
            label3.Text = "Regular Park Records";
            // 
            // HistoricalRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 749);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dgvRegRecord);
            Controls.Add(dgvRentRecord);
            Name = "HistoricalRecords";
            Text = "HistoricalRecords";
            Load += HistoricalRecords_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRentRecord;
        private DataGridView dgvRegRecord;
        private Label label2;
        private Label label3;
    }
}