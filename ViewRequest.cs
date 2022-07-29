using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BBsystem
{
    public partial class ViewRequest : Form
    {
        readonly User donor;
        public ViewRequest(User don)
        {
            InitializeComponent();
            donor = don;
        }
        private void ViewRequest_Load(object sender, EventArgs e)
        {
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            var command = new SqlCommand("SELECT* FROM DisplayRequest("+ donor.userid + ") order by donatedate desc", Start.connection);
            var reader = new SqlDataAdapter(command);
            var filldata = new DataTable();
            reader.Fill(filldata);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = filldata;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Start.connection.Close();
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void ViewRequest_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }

        private Point lastPoint;

        private void ViewRequest_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var command = new SqlCommand($"SELECT dbo.[RequestExist] ({donor.userid})", Start.connection);
            var x = Convert.ToInt32(command.ExecuteScalar());
            if (x!=0)
            {
                MessageBox.Show("You Already Have A Pending Donate Request");
            }
            else 
            {
                command =new SqlCommand("exec InsertDonateRequest " + donor.userid + ", " + donor.bloodtype + "", Start.connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Request Added Successfuly");
            }
            ViewRequest_Load(sender, e);
        }
    }
}
