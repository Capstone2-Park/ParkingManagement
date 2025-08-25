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
            dgvEnter = new DataGridView();
            dgvExit = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvExit).BeginInit();
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
            // dgvEnter
            // 
            dgvEnter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEnter.Location = new Point(44, 33);
            dgvEnter.Name = "dgvEnter";
            dgvEnter.Size = new Size(640, 243);
            dgvEnter.TabIndex = 0;
            // 
            // dgvExit
            // 
            dgvExit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExit.Location = new Point(44, 337);
            dgvExit.Name = "dgvExit";
            dgvExit.Size = new Size(640, 243);
            dgvExit.TabIndex = 1;
            // 
            // SecurityControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 749);
            Controls.Add(panel1);
            Name = "SecurityControl";
            Text = "SecurityControl";
            Load += SecurityControl_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEnter).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvExit).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dgvExit;
        private DataGridView dgvEnter;
    }
}