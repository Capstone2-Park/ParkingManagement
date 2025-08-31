namespace ParkingManagement.Forms
{
    partial class HistoricalRecordReg
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
            panel1 = new Panel();
            btnReg = new Button();
            dgvRegRecord = new DataGridView();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label3 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(btnReg);
            panel1.Controls.Add(dgvRegRecord);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(35, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(896, 673);
            panel1.TabIndex = 6;
            // 
            // btnReg
            // 
            btnReg.Location = new Point(50, 574);
            btnReg.Name = "btnReg";
            btnReg.Size = new Size(131, 33);
            btnReg.TabIndex = 18;
            btnReg.Text = "Rental Records";
            btnReg.UseVisualStyleBackColor = true;
            btnReg.Click += btnReg_Click;
            // 
            // dgvRegRecord
            // 
            dgvRegRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRegRecord.Location = new Point(33, 194);
            dgvRegRecord.Name = "dgvRegRecord";
            dgvRegRecord.ReadOnly = true;
            dgvRegRecord.Size = new Size(805, 365);
            dgvRegRecord.TabIndex = 10;
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
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
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
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ActiveCaption;
            label3.Location = new Point(50, 143);
            label3.Name = "label3";
            label3.Size = new Size(118, 15);
            label3.TabIndex = 4;
            label3.Text = "Regular Park Records";
            // 
            // HistoricalRecordReg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 749);
            Controls.Add(panel1);
            Name = "HistoricalRecordReg";
            Text = "HistoricalRecordReg";
            Load += HistoricalRecordReg_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRegRecord).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Label label3;
        private DataGridView dgvRegRecord;
        private Button btnReg;
    }
}