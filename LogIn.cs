using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BBsystem
{    
    public partial class LogIn : Form
    {
        private readonly User donor;
        public LogIn() { 
            InitializeComponent();
            donor = new User();
            
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            if(Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Start.connection.Close();
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {      
            Close();
        }

        private void Form7_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;

        private void Form7_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }




        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            var query = $"SELECT dbo.[MailExist]('{textBox1.Text}','{textBox2.Text}')";
            var command = new SqlCommand(query, Start.connection);
            var x = Convert.ToInt32(command.ExecuteScalar());
            if (x == 0) {
                MessageBox.Show("Wrong Username Or Password");
                textBox1.Focus();
                return;
            }
            query = "select * from [User] where username='" + textBox1.Text + "'and password='" + textBox2.Text + "'";
            command = new SqlCommand(query, Start.connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                donor.username = textBox1.Text;
                donor.password = textBox2.Text;
                donor.email= reader["email"].ToString();
                donor.fName = reader["FirstName"].ToString();
                donor.lName = reader["LastName"].ToString();
                donor.usertype = (int)reader["usertype"];
                donor.userid= (int)reader["userID"];
                donor.bloodtype = (byte) reader["BloodType"];
                donor.phone= reader["phone"].ToString();
                donor.city= reader["city"].ToString();
                donor.gender = Convert.ToChar(reader["gender"]);
                donor.age = (int)reader["age"];

            }
            reader.Close();
            if(donor.usertype==3)
                MessageBox.Show("welcome Back Admin");
            donor.fName = char.ToUpper(donor.fName[0]) + donor.fName.Substring(1);


            var form5 = new home(donor);
            textBox1.Text = null;
            textBox2.Text = null;
            textBox1.Focus();
            this.StartForm(form5);
        }

        private void LogIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox3_Click(sender,e);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                textBox2.PasswordChar = '*';
            else
                textBox2.PasswordChar = '\0';

        }
    }
    public class User
    {
        public int usertype, userid,age;
        public byte bloodtype;
        public char gender;
        public string fName,lName,phone,city,email,username,password;
        public void delete()
        {
            usertype = userid = age = 0;
            bloodtype =0;
            gender = ' ';
        }
      public User()
        {
            usertype = userid = age = 0;
            bloodtype =  0;
            gender = ' ';

        }
    }
}
