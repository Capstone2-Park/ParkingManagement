namespace ParkingManagement.Forms
{
    partial class UserManagement
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
            dgvLogged = new DataGridView();
            dgvRegistered = new DataGridView();
            btnRegister = new Button();
            label1 = new Label();
            label2 = new Label();
            btnEdit = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogged).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegistered).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(dgvRegistered);
            panel1.Controls.Add(dgvLogged);
            panel1.Location = new Point(32, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(695, 544);
            panel1.TabIndex = 0;
            // 
            // dgvLogged
            // 
            dgvLogged.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogged.Location = new Point(20, 50);
            dgvLogged.Name = "dgvLogged";
            dgvLogged.Size = new Size(654, 245);
            dgvLogged.TabIndex = 0;
            // 
            // dgvRegistered
            // 
            dgvRegistered.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRegistered.Location = new Point(20, 345);
            dgvRegistered.Name = "dgvRegistered";
            dgvRegistered.Size = new Size(654, 159);
            dgvRegistered.TabIndex = 1;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(759, 543);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(126, 41);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 315);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 2;
            label1.Text = "Registered User";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 19);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 3;
            label2.Text = "User Logs";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(759, 487);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(126, 41);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "Edit User";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // UserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 618);
            Controls.Add(btnEdit);
            Controls.Add(btnRegister);
            Controls.Add(panel1);
            Name = "UserManagement";
            Text = "UserManagement";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogged).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRegistered).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dgvRegistered;
        private DataGridView dgvLogged;
        private Button btnRegister;
        private Label label2;
        private Label label1;
        private Button btnEdit;
    }
}