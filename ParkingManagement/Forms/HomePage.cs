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
        }


        private void btnRental_Click(object sender, EventArgs e)
        {
            btnClientM.Visible = true;
            btnParkRental.Visible = true;
        }

        private void btnClientM_Click_1(object sender, EventArgs e)
        {
            panelHome.Controls.Clear();


            ClientManagement rentForm = new ClientManagement();

            rentForm.TopLevel = false;
            rentForm.FormBorderStyle = FormBorderStyle.None;
            rentForm.Dock = DockStyle.Fill;


            panelHome.Controls.Add(rentForm);


            rentForm.Show();
        }

        private void btnParkRental_Click_1(object sender, EventArgs e)
        {
            panelHome.Controls.Clear();


            ParkRental rentForm = new ParkRental();

            rentForm.TopLevel = false;
            rentForm.FormBorderStyle = FormBorderStyle.None;
            rentForm.Dock = DockStyle.Fill;


            panelHome.Controls.Add(rentForm);


            rentForm.Show();
        }

        private void btnFeeM_Click(object sender, EventArgs e)
        {
            FeeManagement FeeList = new FeeManagement();
            FeeList.Show();
            btnClientM.Visible = false;
            btnParkRental.Visible = false;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            btnClientM.Visible = false;
            btnParkRental.Visible = false;

        }

        private void btnRegularParking_Click(object sender, EventArgs e)
        {
            panelHome.Controls.Clear();


            Regular_Parking RegPark = new Regular_Parking();

            RegPark.TopLevel = false;
            RegPark.FormBorderStyle = FormBorderStyle.None;
            RegPark.Dock = DockStyle.Fill;


            panelHome.Controls.Add(RegPark);


            RegPark.Show();
        }
    }
}
