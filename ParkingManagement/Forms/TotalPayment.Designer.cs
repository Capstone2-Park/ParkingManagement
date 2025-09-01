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
            btnConfirm = new Button();
            rtbChange = new RichTextBox();
            label3 = new Label();
            txtCash = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 48);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 2;
            label1.Text = "Client Info";
            label1.Click += label1_Click;
            // 
            // rtbReceipt
            // 
            rtbReceipt.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rtbReceipt.Location = new Point(60, 78);
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
            // btnConfirm
            // 
            btnConfirm.Location = new Point(683, 238);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(89, 25);
            btnConfirm.TabIndex = 13;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // rtbChange
            // 
            rtbChange.Location = new Point(481, 281);
            rtbChange.Name = "rtbChange";
            rtbChange.Size = new Size(290, 92);
            rtbChange.TabIndex = 12;
            rtbChange.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(688, 91);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 11;
            label3.Text = "Caash on Hand";
            // 
            // txtCash
            // 
            txtCash.Location = new Point(597, 109);
            txtCash.Name = "txtCash";
            txtCash.Size = new Size(175, 23);
            txtCash.TabIndex = 10;
            // 
            // TotalPayment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 726);
            Controls.Add(btnConfirm);
            Controls.Add(rtbChange);
            Controls.Add(label3);
            Controls.Add(txtCash);
            Controls.Add(btnReturn);
            Controls.Add(rtbReceipt);
            Controls.Add(label1);
            Name = "TotalPayment";
            Text = "TotalPayment";
            Load += TotalPayment_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstClientInfo;
        private Label label1;
        private RichTextBox rtbReceipt;
        private Label label2;
        private Button btnReturn;
        private Button btnConfirm;
        private RichTextBox rtbChange;
        private Label label3;
        private TextBox txtCash;
    }
}