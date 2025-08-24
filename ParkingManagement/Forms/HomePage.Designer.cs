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
            btnRecords = new Button();
            btnFinance = new Button();
            btnReports = new Button();
            btnReport = new Button();
            lblNotif = new Label();
            pbNotif = new PictureBox();
            btnSlotNav = new Button();
            button1 = new Button();
            btnRegParkSess = new Button();
            btnFeeM = new Button();
            btnClientM = new Button();
            ((System.ComponentModel.ISupportInitialize)ptbLogo).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbNotif).BeginInit();
            SuspendLayout();
            // 
            // panelHome
            // 
            panelHome.Anchor = AnchorStyles.None;
            panelHome.AutoScroll = true;
            panelHome.BackColor = Color.White;
            panelHome.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            panel1.Controls.Add(btnRecords);
            panel1.Controls.Add(btnFinance);
            panel1.Controls.Add(btnReports);
            panel1.Controls.Add(btnReport);
            panel1.Controls.Add(lblNotif);
            panel1.Controls.Add(pbNotif);
            panel1.Controls.Add(btnSlotNav);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnRegParkSess);
            panel1.Controls.Add(btnFeeM);
            panel1.Controls.Add(btnClientM);
            panel1.Location = new Point(13, 189);
            panel1.Name = "panel1";
            panel1.Size = new Size(161, 711);
            panel1.TabIndex = 0;
            // 
            // btnRecords
            // 
            btnRecords.BackColor = Color.FromArgb(255, 128, 128);
            btnRecords.Dock = DockStyle.Top;
            btnRecords.Location = new Point(0, 360);
            btnRecords.Name = "btnRecords";
            btnRecords.Size = new Size(161, 45);
            btnRecords.TabIndex = 17;
            btnRecords.Text = "Historical Records";
            btnRecords.UseVisualStyleBackColor = false;
            btnRecords.Visible = false;
            btnRecords.Click += btnRecords_Click;
            // 
            // btnFinance
            // 
            btnFinance.BackColor = Color.FromArgb(255, 128, 128);
            btnFinance.Dock = DockStyle.Top;
            btnFinance.Location = new Point(0, 315);
            btnFinance.Name = "btnFinance";
            btnFinance.Size = new Size(161, 45);
            btnFinance.TabIndex = 16;
            btnFinance.Text = "Financial Summaries";
            btnFinance.UseVisualStyleBackColor = false;
            btnFinance.Visible = false;
            btnFinance.Click += btnFinance_Click;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.FromArgb(255, 128, 128);
            btnReports.Dock = DockStyle.Top;
            btnReports.Location = new Point(0, 270);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(161, 45);
            btnReports.TabIndex = 15;
            btnReports.Text = "Report";
            btnReports.UseVisualStyleBackColor = false;
            btnReports.Visible = false;
            btnReports.Click += btnReports_Click;
            // 
            // btnReport
            // 
            btnReport.BackColor = Color.FromArgb(255, 128, 128);
            btnReport.Dock = DockStyle.Top;
            btnReport.Location = new Point(0, 225);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(161, 45);
            btnReport.TabIndex = 14;
            btnReport.Text = "Report Management";
            btnReport.UseVisualStyleBackColor = false;
            btnReport.Click += btnReport_Click;
            // 
            // lblNotif
            // 
            lblNotif.AutoSize = true;
            lblNotif.BackColor = Color.FromArgb(255, 128, 128);
            lblNotif.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNotif.ForeColor = Color.Red;
            lblNotif.Location = new Point(89, 413);
            lblNotif.Name = "lblNotif";
            lblNotif.Size = new Size(0, 21);
            lblNotif.TabIndex = 12;
            // 
            // pbNotif
            // 
            pbNotif.BackColor = Color.FromArgb(255, 128, 128);
            pbNotif.Image = Properties.Resources.hi;
            pbNotif.Location = new Point(50, 413);
            pbNotif.Name = "pbNotif";
            pbNotif.Size = new Size(39, 38);
            pbNotif.SizeMode = PictureBoxSizeMode.StretchImage;
            pbNotif.TabIndex = 11;
            pbNotif.TabStop = false;
            pbNotif.Click += pbNotif_Click;
            // 
            // btnSlotNav
            // 
            btnSlotNav.BackColor = Color.FromArgb(255, 128, 128);
            btnSlotNav.Dock = DockStyle.Top;
            btnSlotNav.Location = new Point(0, 180);
            btnSlotNav.Name = "btnSlotNav";
            btnSlotNav.Size = new Size(161, 45);
            btnSlotNav.TabIndex = 10;
            btnSlotNav.Text = "Slot Management";
            btnSlotNav.UseVisualStyleBackColor = false;
            btnSlotNav.Click += btnSlotNav_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 128, 128);
            button1.Dock = DockStyle.Top;
            button1.Location = new Point(0, 135);
            button1.Name = "button1";
            button1.Size = new Size(161, 45);
            button1.TabIndex = 9;
            button1.Text = "Regular Parking LogOut";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_2;
            // 
            // btnRegParkSess
            // 
            btnRegParkSess.BackColor = Color.FromArgb(255, 128, 128);
            btnRegParkSess.Dock = DockStyle.Top;
            btnRegParkSess.Location = new Point(0, 90);
            btnRegParkSess.Name = "btnRegParkSess";
            btnRegParkSess.Size = new Size(161, 45);
            btnRegParkSess.TabIndex = 8;
            btnRegParkSess.Text = "Regular Parking Session";
            btnRegParkSess.UseVisualStyleBackColor = false;
            btnRegParkSess.Click += btnRegParkSess_Click;
            // 
            // btnFeeM
            // 
            btnFeeM.BackColor = Color.FromArgb(255, 128, 128);
            btnFeeM.Dock = DockStyle.Top;
            btnFeeM.Location = new Point(0, 45);
            btnFeeM.Name = "btnFeeM";
            btnFeeM.Size = new Size(161, 45);
            btnFeeM.TabIndex = 6;
            btnFeeM.Text = "Fee management";
            btnFeeM.UseVisualStyleBackColor = false;
            btnFeeM.Click += btnFeeM_Click;
            // 
            // btnClientM
            // 
            btnClientM.BackColor = Color.FromArgb(255, 128, 128);
            btnClientM.Dock = DockStyle.Top;
            btnClientM.Location = new Point(0, 0);
            btnClientM.Name = "btnClientM";
            btnClientM.Size = new Size(161, 45);
            btnClientM.TabIndex = 4;
            btnClientM.Text = "Client Management";
            btnClientM.UseVisualStyleBackColor = false;
            btnClientM.Click += btnClientM_Click_1;
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
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbNotif).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHome;
        private PictureBox ptbLogo;
        private Panel panel1;
        private Button btnClientM;
        private Button btnFeeM;
        private Button btnRegParkSess;
        private Button button1;
        private Button btnSlotNav;
        private PictureBox pbNotif;
        private Label lblNotif;
        private Button btnReport;
        private Button btnRecords;
        private Button btnFinance;
        private Button btnReports;
    }
}