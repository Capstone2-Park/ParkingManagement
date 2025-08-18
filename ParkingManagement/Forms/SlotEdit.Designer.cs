namespace ParkingManagement.Forms
{
    partial class SlotEdit
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
            btnSlotAddM = new Button();
            btnSlotRemoveM = new Button();
            txtVehicle = new TextBox();
            txtMotorcycle = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnSlotAddV = new Button();
            btnSlotRemoveV = new Button();
            SuspendLayout();
            // 
            // btnSlotAddM
            // 
            btnSlotAddM.Location = new Point(227, 98);
            btnSlotAddM.Name = "btnSlotAddM";
            btnSlotAddM.Size = new Size(109, 40);
            btnSlotAddM.TabIndex = 0;
            btnSlotAddM.Text = "Add a Slot";
            btnSlotAddM.UseVisualStyleBackColor = true;
            btnSlotAddM.Click += btnSlotAddM_Click;
            // 
            // btnSlotRemoveM
            // 
            btnSlotRemoveM.Location = new Point(492, 98);
            btnSlotRemoveM.Name = "btnSlotRemoveM";
            btnSlotRemoveM.Size = new Size(109, 40);
            btnSlotRemoveM.TabIndex = 4;
            btnSlotRemoveM.Text = "Remove a Slot";
            btnSlotRemoveM.UseVisualStyleBackColor = true;
            btnSlotRemoveM.Click += btnSlotRemoveM_Click;
            // 
            // txtVehicle
            // 
            txtVehicle.Location = new Point(354, 295);
            txtVehicle.Name = "txtVehicle";
            txtVehicle.Size = new Size(123, 23);
            txtVehicle.TabIndex = 6;
            // 
            // txtMotorcycle
            // 
            txtMotorcycle.Location = new Point(351, 98);
            txtMotorcycle.Name = "txtMotorcycle";
            txtMotorcycle.Size = new Size(123, 23);
            txtMotorcycle.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(342, 80);
            label1.Name = "label1";
            label1.Size = new Size(144, 15);
            label1.TabIndex = 7;
            label1.Text = "MotorCycle/2-wheels Slot";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(358, 263);
            label2.Name = "label2";
            label2.Size = new Size(119, 15);
            label2.TabIndex = 8;
            label2.Text = "Vehicle/4-wheels Slot";
            // 
            // btnSlotAddV
            // 
            btnSlotAddV.Location = new Point(227, 285);
            btnSlotAddV.Name = "btnSlotAddV";
            btnSlotAddV.Size = new Size(109, 40);
            btnSlotAddV.TabIndex = 9;
            btnSlotAddV.Text = "Add a Slot";
            btnSlotAddV.UseVisualStyleBackColor = true;
            btnSlotAddV.Click += btnSlotAddV_Click;
            // 
            // btnSlotRemoveV
            // 
            btnSlotRemoveV.Location = new Point(492, 278);
            btnSlotRemoveV.Name = "btnSlotRemoveV";
            btnSlotRemoveV.Size = new Size(109, 40);
            btnSlotRemoveV.TabIndex = 10;
            btnSlotRemoveV.Text = "Remove a Slot";
            btnSlotRemoveV.UseVisualStyleBackColor = true;
            btnSlotRemoveV.Click += btnSlotRemoveV_Click;
            // 
            // SlotEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSlotRemoveV);
            Controls.Add(btnSlotAddV);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtVehicle);
            Controls.Add(txtMotorcycle);
            Controls.Add(btnSlotRemoveM);
            Controls.Add(btnSlotAddM);
            Name = "SlotEdit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SlotEdit";
            Load += SlotEdit_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSlotAddM;
        private Button btnSlotRemoveM;
        private TextBox txtVehicle;
        private TextBox txtMotorcycle;
        private Label label1;
        private Label label2;
        private Button btnSlotAddV;
        private Button btnSlotRemoveV;
    }
}