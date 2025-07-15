using ParkingManagement.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingManagement
{

    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.MinimumSize = new Size(800, 600);
        }

        public void ShowFormInPanel(Form childForm)
        {
            panelHome.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelHome.Controls.Add(childForm);
            childForm.Show();
        }

        private void btnRental_Click(object sender, EventArgs e)
        {
            btnClientM.Visible = true;
        }

        private void btnClientM_Click_1(object sender, EventArgs e)
        {
            ShowFormInPanel(new ClientManagement());
        }

        private void btnFeeM_Click(object sender, EventArgs e)
        {
            FeeManagement FeeList = new FeeManagement();
            FeeList.Show();
            btnClientM.Visible = false;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            btnClientM.Visible = false;
        }

        private void btnRegularParking_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Regular_Parking());
        }

        private void btnParkSlot_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ParkRental());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ParkRental());
        }

        
    }
}
