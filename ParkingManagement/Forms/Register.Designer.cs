namespace ParkingManagement.Forms
{
    partial class Register
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            cbRole = new ComboBox();
            txtFname = new TextBox();
            txtUname = new TextBox();
            txtPass = new TextBox();
            panel1 = new Panel();
            label5 = new Label();
            btnRegister = new Button();
            btnClear = new Button();
            btnLogIn = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(149, 85);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 0;
            label1.Text = "Full Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(149, 40);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 1;
            label2.Text = "User Role";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(149, 195);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 2;
            label3.Text = "Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(149, 137);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 3;
            label4.Text = "Username";
            // 
            // cbRole
            // 
            cbRole.FormattingEnabled = true;
            cbRole.Location = new Point(294, 37);
            cbRole.Name = "cbRole";
            cbRole.Size = new Size(209, 23);
            cbRole.TabIndex = 4;
            // 
            // txtFname
            // 
            txtFname.Location = new Point(294, 85);
            txtFname.Name = "txtFname";
            txtFname.Size = new Size(209, 23);
            txtFname.TabIndex = 5;
            // 
            // txtUname
            // 
            txtUname.Location = new Point(294, 137);
            txtUname.Name = "txtUname";
            txtUname.Size = new Size(209, 23);
            txtUname.TabIndex = 6;
            // 
            // txtPass
            // 
            txtPass.Location = new Point(294, 195);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(209, 23);
            txtPass.TabIndex = 7;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtPass);
            panel1.Controls.Add(txtUname);
            panel1.Controls.Add(txtFname);
            panel1.Controls.Add(cbRole);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(97, 104);
            panel1.Name = "panel1";
            panel1.Size = new Size(683, 301);
            panel1.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(326, 37);
            label5.Name = "label5";
            label5.Size = new Size(211, 32);
            label5.TabIndex = 8;
            label5.Text = "User Registration";
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(116, 442);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(168, 44);
            btnRegister.TabIndex = 9;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(359, 442);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(168, 44);
            btnClear.TabIndex = 10;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnLogIn
            // 
            btnLogIn.Location = new Point(596, 442);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(168, 44);
            btnLogIn.TabIndex = 11;
            btnLogIn.Text = "Log In";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // Register
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(888, 529);
            Controls.Add(btnLogIn);
            Controls.Add(btnClear);
            Controls.Add(btnRegister);
            Controls.Add(label5);
            Controls.Add(panel1);
            Name = "Register";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register";
            Load += Register_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox cbRole;
        private TextBox txtFname;
        private TextBox txtUname;
        private TextBox txtPass;
        private Panel panel1;
        private Label label5;
        private Button btnRegister;
        private Button btnClear;
        private Button btnLogIn;
    }
}