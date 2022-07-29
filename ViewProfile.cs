using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BBsystem
{
    enum City
    {
        Alexandria,
        Cairo,
        Asyut,
        Suez,
        Faiyum,
        Matruh,
        Qena,
        Portsaid,
        Gharbia,
        Giza,
        Ismailia,
        Minya
    }
    public partial class ViewProfile : Form
    {
        User donor;
        public ViewProfile(User don)
        {
            donor = don;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox6.Visible = false;
            button3.Visible = false;
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            foreach (var textBox in this.Controls.OfType<TextBox>())
                textBox.ReadOnly = true;
            comboBox1.Enabled = false;

            var cmd = new SqlCommand("SELECT * FROM [USER] where userid=" + donor.userid, Start.connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                filldata();
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Enter Your Password To Continue");
            textBox6.Visible = true;
            button3.Visible = true;
            textBox6.ReadOnly = false;
            textBox6.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Enabled == false)
                return;
            DialogResult check = MessageBox.Show("Update user info ?", "", MessageBoxButtons.YesNo);
            if (check == DialogResult.No)
                return;
            if (!checK())
                return;
            var cmd = new SqlCommand("UPDATE [USER] SET FirstName='" + char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1) + "' ,LastName='" + char.ToUpper(textBox8.Text[0]) + textBox8.Text.Substring(1) + "' ,phone='" + textBox2.Text + "' ,city='" + comboBox1.Text + "' ,age=" + textBox7.Text + " ,password='" + textBox9.Text + "'" + "   WHERE userid=" + donor.userid, Start.connection);
            var reader = cmd.ExecuteReader();
            reader.Close();
            MessageBox.Show("Info Saved");
            updateuser();
            Form1_Load(sender, e);
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

        private void ViewProfile_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;

        private void ViewProfile_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == donor.password)
            {
                foreach (var textBox in this.Controls.OfType<TextBox>())
                    textBox.ReadOnly = false;
                comboBox1.Enabled = true;
                textBox10.ReadOnly = textBox4.ReadOnly = true;
                textBox9.PasswordChar = '\0';
            }
            else
                MessageBox.Show("Incorrect Password Please Try Again");
            return;
        }
        private bool checK()
        {
            var name = new Regex("[a-zA-Z][^#&<>\"~;$^%{}?]{1,20}$");
            if (name.IsMatch(textBox1.Text) == false)
            {
                MessageBox.Show("Your First Name Is Invalid");
                textBox1.Focus();
                return false;
            }
            else if (name.IsMatch(textBox8.Text) == false)
            {
                MessageBox.Show("Your Last Name Is Invalid");
                textBox8.Focus();
                return false;
            }
            if (textBox2.Text != donor.phone)
            {
                var query = "select count(*)from [user] where phone='" + textBox2.Text + "'";
                var command = new SqlCommand(query, Start.connection);
                var phone = new Regex("^(01)[0-9]{9}$");
                if (phone.IsMatch(textBox2.Text) == false)
                {
                    MessageBox.Show("Mobile Number Is Invalid");
                    textBox2.Focus();
                    return false;
                }
                else if ((int)command.ExecuteScalar() != 0)
                {
                    MessageBox.Show("Phone Is Already Used");
                    textBox2.Focus();
                    return false;
                }
            }
            if (textBox9.Text != donor.password)
            {
                var password = new Regex(@"(?=^.{8,12}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.");
                if (password.IsMatch(textBox9.Text) == false)
                {
                    MessageBox.Show("Use a Powerful Password(UpperCase,LowerCase And Numbers)");
                    textBox9.Focus();
                    return false;
                }
            }
            if (textBox7.Text.Length == 0)
            {
                MessageBox.Show("Enter A Correct Age");
                textBox7.Focus();
                return false;
            }
            if (comboBox1.Text == null)
            {
                MessageBox.Show("Choose A City");
                comboBox1.Focus();
                return false;
            }

            return true;
        }
        private void filldata()
        {
            textBox1.Text = donor.fName;
            textBox8.Text = donor.lName;
            textBox2.Text = donor.phone;
            comboBox1.Text = donor.city;
            textBox7.Text = donor.age.ToString();
            textBox4.Text = donor.email;
            textBox10.Text = donor.username;
            textBox9.Text = donor.password;
        }
        private void updateuser()

        {
            donor.fName= textBox1.Text;
             donor.lName= textBox8.Text;
            donor.phone = textBox2.Text;
             donor.city= comboBox1.Text;
             donor.age = Convert.ToInt32(textBox7.Text);
            donor.email= textBox4.Text;
            donor.username= textBox10.Text;
            donor.password=textBox9.Text ;
        }
    }
}
