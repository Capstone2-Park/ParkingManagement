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
            label2 = new Label();
            panel1 = new Panel();
            btnReg = new Button();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvRentRecord
            // 
            dgvRentRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRentRecord.Location = new Point(48, 160);
            dgvRentRecord.Name = "dgvRentRecord";
            dgvRentRecord.ReadOnly = true;
            dgvRentRecord.Size = new Size(805, 396);
            dgvRentRecord.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(81, 120);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 3;
            label2.Text = "Rental Records";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(btnReg);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(dgvRentRecord);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(38, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(896, 673);
            panel1.TabIndex = 5;
            // 
            // btnReg
            // 
            btnReg.Location = new Point(70, 576);
            btnReg.Name = "btnReg";
            btnReg.Size = new Size(131, 33);
            btnReg.TabIndex = 17;
            btnReg.Text = "Regular Records";
            btnReg.UseVisualStyleBackColor = true;
            btnReg.Click += btnReg_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(422, 51);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 9;
            label6.Text = "Date Range";
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(dtpFrom);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(dtpTo);
            panel2.Location = new Point(33, 69);
            panel2.Name = "panel2";
            panel2.Size = new Size(860, 48);
            panel2.TabIndex = 8;
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
            dtpFrom.Location = new Point(186, 11);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 23);
            dtpFrom.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(108, 17);
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
            // 
            // HistoricalRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 749);
            Controls.Add(panel1);
            Name = "HistoricalRecords";
            Text = "HistoricalRecords";
            Load += HistoricalRecords_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRentRecord).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvRentRecord;
        private Label label2;
        private Panel panel1;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnReg;
    }
}