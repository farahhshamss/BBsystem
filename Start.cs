using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;


namespace BBsystem
{
    public partial class Start : Form
    {
        private const string ConString = @"Data Source=MIDOKIM-PC\MKK;Initial Catalog=BloodBankDB;Integrated Security=True";
        public static SqlConnection connection;
        public Start()
        {
          InitializeComponent();
          connection = new SqlConnection(ConString);

        }       

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void Start_Load(object sender, EventArgs e)
        {
            if (connection.State.Equals(ConnectionState.Closed))
            {
                connection.Open();
            }
            pictureBox1.Image = Properties.Resources.conusbutton;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.StartForm(new ContactUs());

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.StartForm(new About());

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.StartForm(new LogIn());
        }

        
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.StartForm(new SignUp());

        }

    }
}
