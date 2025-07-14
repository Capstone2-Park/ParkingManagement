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
            btnSelect = new Button();
            label1 = new Label();
            cbClient = new ComboBox();
            rtbReceipt = new RichTextBox();
            SuspendLayout();
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(162, 58);
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
            label1.Location = new Point(162, 112);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 2;
            label1.Text = "Client Info";
            label1.Click += label1_Click;
            // 
            // cbClient
            // 
            cbClient.FormattingEnabled = true;
            cbClient.Location = new Point(162, 29);
            cbClient.Name = "cbClient";
            cbClient.Size = new Size(130, 23);
            cbClient.TabIndex = 3;
            // 
            // rtbReceipt
            // 
            rtbReceipt.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rtbReceipt.Location = new Point(162, 142);
            rtbReceipt.Name = "rtbReceipt";
            rtbReceipt.Size = new Size(351, 541);
            rtbReceipt.TabIndex = 4;
            rtbReceipt.Text = "";
            // 
            // TotalPayment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 726);
            Controls.Add(rtbReceipt);
            Controls.Add(cbClient);
            Controls.Add(label1);
            Controls.Add(btnSelect);
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
        private RichTextBox rtbReceipt;
        private Label label2;
    }
}