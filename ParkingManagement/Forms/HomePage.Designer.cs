namespace ParkingManagement
{
    partial class HomePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            panelHome = new Panel();
            ptbLogo = new PictureBox();
            panel1 = new Panel();
            btnRegularParking = new Button();
            btnFeeM = new Button();
            btnClientM = new Button();
            btnRental = new Button();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)ptbLogo).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelHome
            // 
            panelHome.Anchor = AnchorStyles.None;
            panelHome.AutoScroll = true;
            panelHome.BackColor = Color.White;
            panelHome.Location = new Point(191, 15);
            panelHome.Name = "panelHome";
            panelHome.Size = new Size(1264, 929);
            panelHome.TabIndex = 0;
            // 
            // ptbLogo
            // 
            ptbLogo.BackgroundImage = (Image)resources.GetObject("ptbLogo.BackgroundImage");
            ptbLogo.BackgroundImageLayout = ImageLayout.Stretch;
            ptbLogo.Location = new Point(13, 15);
            ptbLogo.Name = "ptbLogo";
            ptbLogo.Size = new Size(161, 142);
            ptbLogo.TabIndex = 0;
            ptbLogo.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnRegularParking);
            panel1.Controls.Add(btnFeeM);
            panel1.Controls.Add(btnClientM);
            panel1.Controls.Add(btnRental);
            panel1.Location = new Point(13, 189);
            panel1.Name = "panel1";
            panel1.Size = new Size(161, 711);
            panel1.TabIndex = 0;
            // 
            // btnRegularParking
            // 
            btnRegularParking.BackColor = Color.FromArgb(255, 128, 128);
            btnRegularParking.Dock = DockStyle.Top;
            btnRegularParking.Location = new Point(0, 135);
            btnRegularParking.Name = "btnRegularParking";
            btnRegularParking.Size = new Size(161, 45);
            btnRegularParking.TabIndex = 7;
            btnRegularParking.Text = "Regular Parking";
            btnRegularParking.UseVisualStyleBackColor = false;
            btnRegularParking.Click += btnRegularParking_Click;
            // 
            // btnFeeM
            // 
            btnFeeM.BackColor = Color.FromArgb(255, 128, 128);
            btnFeeM.Dock = DockStyle.Top;
            btnFeeM.Location = new Point(0, 90);
            btnFeeM.Name = "btnFeeM";
            btnFeeM.Size = new Size(161, 45);
            btnFeeM.TabIndex = 6;
            btnFeeM.Text = "Fee management";
            btnFeeM.UseVisualStyleBackColor = false;
            btnFeeM.Click += btnFeeM_Click;
            // 
            // btnClientM
            // 
            btnClientM.BackColor = Color.FromArgb(255, 192, 192);
            btnClientM.Dock = DockStyle.Top;
            btnClientM.Location = new Point(0, 45);
            btnClientM.Name = "btnClientM";
            btnClientM.Size = new Size(161, 45);
            btnClientM.TabIndex = 4;
            btnClientM.Text = "Client Management";
            btnClientM.UseVisualStyleBackColor = false;
            btnClientM.Click += btnClientM_Click_1;
            // 
            // btnRental
            // 
            btnRental.BackColor = Color.FromArgb(255, 128, 128);
            btnRental.Dock = DockStyle.Top;
            btnRental.Location = new Point(0, 0);
            btnRental.Name = "btnRental";
            btnRental.Size = new Size(161, 45);
            btnRental.TabIndex = 3;
            btnRental.Text = "Rental";
            btnRental.UseVisualStyleBackColor = false;
            btnRental.Click += btnRental_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 128, 128);
            button1.Dock = DockStyle.Top;
            button1.Location = new Point(0, 180);
            button1.Name = "button1";
            button1.Size = new Size(161, 45);
            button1.TabIndex = 8;
            button1.Text = "Regular Parking";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // HomePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 64, 64);
            ClientSize = new Size(1370, 749);
            Controls.Add(panel1);
            Controls.Add(ptbLogo);
            Controls.Add(panelHome);
            Name = "HomePage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HomePage";
            Load += HomePage_Load;
            ((System.ComponentModel.ISupportInitialize)ptbLogo).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHome;
        private PictureBox ptbLogo;
        private Panel panel1;
        private Button btnRental;
        private Button btnClientM;
        private Button btnFeeM;
        private Button btnRegularParking;
        private Button button1;
    }
}