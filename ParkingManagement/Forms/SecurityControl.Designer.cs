namespace ParkingManagement.Forms
{
    partial class SecurityControl
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
            dgvExit = new DataGridView();
            dgvEnter = new DataGridView();
            panel2 = new Panel();
            label1 = new Label();
            btnConfirm = new Button();
            cbRent = new ComboBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEnter).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(dgvExit);
            panel1.Controls.Add(dgvEnter);
            panel1.Location = new Point(13, 68);
            panel1.Name = "panel1";
            panel1.Size = new Size(719, 617);
            panel1.TabIndex = 0;
            // 
            // dgvExit
            // 
            dgvExit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExit.Location = new Point(44, 337);
            dgvExit.Name = "dgvExit";
            dgvExit.Size = new Size(640, 243);
            dgvExit.TabIndex = 1;
            // 
            // dgvEnter
            // 
            dgvEnter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEnter.Location = new Point(44, 33);
            dgvEnter.Name = "dgvEnter";
            dgvEnter.Size = new Size(640, 243);
            dgvEnter.TabIndex = 0;
            dgvEnter.CellContentClick += dgvEnter_CellContentClick;
            // 
            // panel2
            // 
            panel2.Controls.Add(cbRent);
            panel2.Controls.Add(btnConfirm);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(760, 389);
            panel2.Name = "panel2";
            panel2.Size = new Size(227, 296);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 59);
            label1.Name = "label1";
            label1.Size = new Size(180, 15);
            label1.TabIndex = 0;
            label1.Text = "Log Out confirm for Rental Parks";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(53, 188);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(121, 38);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // cbRent
            // 
            cbRent.FormattingEnabled = true;
            cbRent.Location = new Point(34, 112);
            cbRent.Name = "cbRent";
            cbRent.Size = new Size(163, 23);
            cbRent.TabIndex = 2;
            // 
            // SecurityControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 749);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "SecurityControl";
            Text = "SecurityControl";
            Load += SecurityControl_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvExit).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEnter).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dgvExit;
        private DataGridView dgvEnter;
        private Panel panel2;
        private Label label1;
        private Button btnConfirm;
        private ComboBox cbRent;
    }
}