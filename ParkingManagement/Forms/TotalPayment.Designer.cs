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
            label1 = new Label();
            rtbReceipt = new RichTextBox();
            btnReturn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(162, 38);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 2;
            label1.Text = "Client Info";
            label1.Click += label1_Click;
            // 
            // rtbReceipt
            // 
            rtbReceipt.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rtbReceipt.Location = new Point(162, 68);
            rtbReceipt.Name = "rtbReceipt";
            rtbReceipt.Size = new Size(351, 541);
            rtbReceipt.TabIndex = 4;
            rtbReceipt.Text = "";
            // 
            // btnReturn
            // 
            btnReturn.Location = new Point(537, 525);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(119, 38);
            btnReturn.TabIndex = 5;
            btnReturn.Text = "Return";
            btnReturn.UseVisualStyleBackColor = true;
            btnReturn.Click += btnReturn_Click;
            // 
            // TotalPayment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 726);
            Controls.Add(btnReturn);
            Controls.Add(rtbReceipt);
            Controls.Add(label1);
            Name = "TotalPayment";
            Text = "TotalPayment";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstClientInfo;
        private Label label1;
        private RichTextBox rtbReceipt;
        private Label label2;
        private Button btnReturn;
    }
}