using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BBsystem
{
    public partial class home : Form
    {
        readonly User donor ;

        public home(User donn)
        {
            donor = donn;
            InitializeComponent();
        }

        public void home_Load(object sender, EventArgs e)
        {
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            if (donor.usertype == 1)
                pictureBox7.Hide();
            if (donor.usertype != 3)
                pictureBox8.Hide();
            else
                pictureBox8.Show();
            switch (donor.bloodtype)
            {
                case 0:
                    pictureBox2.Image = Properties.Resources.ap;
                    break;
                case 1:
                    pictureBox2.Image = Properties.Resources.an;
                    break;
                case 2:
                    pictureBox2.Image = Properties.Resources.bp;
                    break;
                case 3:
                    pictureBox2.Image = Properties.Resources.bn;
                    break;
                case 4:
                    pictureBox2.Image = Properties.Resources.op;
                    break;
                case 5:
                    pictureBox2.Image = Properties.Resources.on;
                    break;
                case 6:
                    pictureBox2.Image = Properties.Resources.abp;
                    break;
                case 7:
                    pictureBox2.Image = Properties.Resources.abn;
                    break;
            }
                label13.Text = "Welcome Back,";
            label4.Text = donor.fName+" "+donor.lName;

            loadDonors();
        }

        private void loadDonors()
        {
            SqlCommand load = new SqlCommand("select count(*) from DonationRequest where bloodtype=0 and completed=1", Start.connection);
            label5.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=1 and completed =1", Start.connection);
            label6.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=2 and completed =1", Start.connection);
            label7.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=3 and completed =1", Start.connection);
            label8.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=4 and completed =1", Start.connection);
            label9.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=5 and completed =1", Start.connection);
            label10.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=6 and completed =1", Start.connection);
            label11.Text = Convert.ToString(load.ExecuteScalar());
            load = new SqlCommand("select count(*) from [DonationRequest] where bloodtype=7 and completed =1", Start.connection);
            label12.Text = Convert.ToString(load.ExecuteScalar());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Start.connection.Close();
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var message = "Do you want to Log Out?";
            var title = "Log Out";
            var buttons = MessageBoxButtons.YesNo;
            var result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
                Close();
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);

        }
        Point lastPoint;

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }


        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.StartForm(new ViewProfile(donor));

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.StartForm(new ViewRequest(donor));

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.StartForm(new ContactUs(donor));

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.StartForm(new AdminPanel());

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.StartForm(new ViewDonations(donor));

        }

    }
}
