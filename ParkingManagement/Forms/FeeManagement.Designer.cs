namespace ParkingManagement.Forms
{
    partial class FeeManagement
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
            dgvFeeList = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            btnSaveFee = new Button();
            cmbTypeOfVehicle = new ComboBox();
            txtInputFee = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvFeeList).BeginInit();
            SuspendLayout();
            // 
            // dgvFeeList
            // 
            dgvFeeList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFeeList.Location = new Point(131, 195);
            dgvFeeList.Name = "dgvFeeList";
            dgvFeeList.Size = new Size(471, 150);
            dgvFeeList.TabIndex = 0;
            dgvFeeList.SelectionChanged += dgvFeeList_SelectionChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(74, 61);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 1;
            label1.Text = "Type Of Vehicle";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(106, 115);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 2;
            label2.Text = "Input Fee";
            // 
            // btnSaveFee
            // 
            btnSaveFee.Location = new Point(520, 381);
            btnSaveFee.Name = "btnSaveFee";
            btnSaveFee.Size = new Size(75, 23);
            btnSaveFee.TabIndex = 3;
            btnSaveFee.Text = "Save";
            btnSaveFee.UseVisualStyleBackColor = true;
            btnSaveFee.Click += btnSave_Click;
            // 
            // cmbTypeOfVehicle
            // 
            cmbTypeOfVehicle.FormattingEnabled = true;
            cmbTypeOfVehicle.Location = new Point(188, 58);
            cmbTypeOfVehicle.Name = "cmbTypeOfVehicle";
            cmbTypeOfVehicle.Size = new Size(121, 23);
            cmbTypeOfVehicle.TabIndex = 4;
            // 
            // txtInputFee
            // 
            txtInputFee.Location = new Point(188, 107);
            txtInputFee.Name = "txtInputFee";
            txtInputFee.Size = new Size(121, 23);
            txtInputFee.TabIndex = 5;
            // 
            // FeeManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(737, 450);
            Controls.Add(txtInputFee);
            Controls.Add(cmbTypeOfVehicle);
            Controls.Add(btnSaveFee);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvFeeList);
            Name = "FeeManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FeeManagement";
            Load += FeeManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvFeeList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvFeeList;
        private Label label1;
        private Label label2;
        private Button btnSaveFee;
        private ComboBox cmbTypeOfVehicle;
        private TextBox txtInputFee;
    }
}