namespace ParkingManagement.Forms
{
    partial class TotalPayment
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
            lstClientInfo = new ListBox();
            btnSelect = new Button();
            label1 = new Label();
            cbClient = new ComboBox();
            rtbTotal = new RichTextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // lstClientInfo
            // 
            lstClientInfo.FormattingEnabled = true;
            lstClientInfo.ItemHeight = 15;
            lstClientInfo.Location = new Point(162, 151);
            lstClientInfo.Name = "lstClientInfo";
            lstClientInfo.Size = new Size(365, 259);
            lstClientInfo.TabIndex = 0;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(162, 70);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(98, 35);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "Select Client";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(162, 124);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 2;
            label1.Text = "Client Info";
            label1.Click += label1_Click;
            // 
            // cbClient
            // 
            cbClient.FormattingEnabled = true;
            cbClient.Location = new Point(162, 41);
            cbClient.Name = "cbClient";
            cbClient.Size = new Size(130, 23);
            cbClient.TabIndex = 3;
            // 
            // rtbTotal
            // 
            rtbTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rtbTotal.Location = new Point(554, 151);
            rtbTotal.Name = "rtbTotal";
            rtbTotal.Size = new Size(186, 134);
            rtbTotal.TabIndex = 4;
            rtbTotal.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(554, 124);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 5;
            label2.Text = "Total Payment";
            // 
            // TotalPayment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 437);
            Controls.Add(label2);
            Controls.Add(rtbTotal);
            Controls.Add(cbClient);
            Controls.Add(label1);
            Controls.Add(btnSelect);
            Controls.Add(lstClientInfo);
            Name = "TotalPayment";
            Text = "TotalPayment";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstClientInfo;
        private Button btnSelect;
        private Label label1;
        private ComboBox cbClient;
        private RichTextBox rtbTotal;
        private Label label2;
    }
}