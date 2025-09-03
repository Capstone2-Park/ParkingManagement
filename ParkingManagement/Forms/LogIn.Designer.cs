namespace ParkingManagement.Forms
{
    partial class LogIn
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
            lblCoolDown = new Label();
            pbShow = new PictureBox();
            txtPass = new TextBox();
            txtUname = new TextBox();
            label4 = new Label();
            label3 = new Label();
            btnLogIn = new Button();
            btnClear = new Button();
            label5 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(lblCoolDown);
            panel1.Controls.Add(pbShow);
            panel1.Controls.Add(txtPass);
            panel1.Controls.Add(txtUname);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(103, 107);
            panel1.Name = "panel1";
            panel1.Size = new Size(683, 301);
            panel1.TabIndex = 12;
            // 
            // lblCoolDown
            // 
            lblCoolDown.AutoSize = true;
            lblCoolDown.Location = new Point(242, 208);
            lblCoolDown.Name = "lblCoolDown";
            lblCoolDown.Size = new Size(0, 15);
            lblCoolDown.TabIndex = 9;
            // 
            // pbShow
            // 
            pbShow.BackColor = SystemColors.ActiveCaption;
            pbShow.Location = new Point(483, 155);
            pbShow.Name = "pbShow";
            pbShow.Size = new Size(24, 23);
            pbShow.TabIndex = 8;
            pbShow.TabStop = false;
            pbShow.Click += pbShow_Click_1;
            // 
            // txtPass
            // 
            txtPass.Location = new Point(298, 155);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(209, 23);
            txtPass.TabIndex = 7;
            // 
            // txtUname
            // 
            txtUname.Location = new Point(298, 97);
            txtUname.Name = "txtUname";
            txtUname.Size = new Size(209, 23);
            txtUname.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(153, 97);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 3;
            label4.Text = "Username";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(153, 155);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 2;
            label3.Text = "Password";
            // 
            // btnLogIn
            // 
            btnLogIn.Location = new Point(217, 436);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(168, 44);
            btnLogIn.TabIndex = 16;
            btnLogIn.Text = "Log In";
            btnLogIn.UseVisualStyleBackColor = true;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(540, 436);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(168, 44);
            btnClear.TabIndex = 15;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(378, 44);
            label5.Name = "label5";
            label5.Size = new Size(144, 32);
            label5.TabIndex = 13;
            label5.Text = "User Log In";
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(888, 529);
            Controls.Add(panel1);
            Controls.Add(btnLogIn);
            Controls.Add(btnClear);
            Controls.Add(label5);
            Name = "LogIn";
            Text = "LogIn";
            Load += LogIn_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private TextBox txtPass;
        private TextBox txtUname;
        private Label label4;
        private Label label3;
        private Button btnLogIn;
        private Button btnClear;
        private Label label5;
        private PictureBox pbShow;
        private Label lblCoolDown;
    }
}