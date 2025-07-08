namespace ParkingManagement.Forms
{
    partial class ParkingSlot
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
            flpPakingSlot = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // flpPakingSlot
            // 
            flpPakingSlot.Location = new Point(71, 122);
            flpPakingSlot.Name = "flpPakingSlot";
            flpPakingSlot.Size = new Size(868, 433);
            flpPakingSlot.TabIndex = 0;
            // 
            // ParkingSlot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1040, 684);
            Controls.Add(flpPakingSlot);
            Name = "ParkingSlot";
            Text = "ParkingSlot";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpPakingSlot;
    }
}