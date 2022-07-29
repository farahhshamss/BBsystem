using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace BBsystem
{
    enum bloodtype
     {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        OPositive,
        ONegative,
        ABPositive,
        ABNegative
    }

    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
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

        private void Form6_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;
        private void Form6_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (!check())
                return;
            var BloodType = Convert.ToInt32(Enum.Parse(typeof(bloodtype), comboBox1.Text.Replace("+", "Positive").Replace("-", "Negative")));
            var x = radioButton1.Checked ? 'm' : 'f';
            var command = new SqlCommand($"INSERT INTO [User] VALUES('{ char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1)}','{char.ToUpper(textBox2.Text[0]) + textBox2.Text.Substring(1)}','{textBox3.Text}','{comboBox2.Text}','{textBox4.Text}','{BloodType}','{x}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}',1,GETDATE());", Start.connection);
            command.ExecuteNonQuery();
            MessageBox.Show("Email Created Successfully");
            Close();
        }

        private bool check()
        {
            var name = new Regex("[a-zA-Z][^#&<>\"~;$^%{}?]{1,20}$");
            if (name.IsMatch(textBox1.Text)==false)
            {
                MessageBox.Show("Your First Name Is Invalid");
                textBox1.Focus();
                return false;
            }
            else if (name.IsMatch(textBox2.Text)==false)
            {
                MessageBox.Show("Your Last Name Is Invalid");
                textBox2.Focus();
                return false;
            }
            var query = "select count(*)from [user] where phone='" + textBox3.Text + "'";
            var command = new SqlCommand(query, Start.connection);
            var phone = new Regex("^(01)[0-9]{9}$");
            if (phone.IsMatch(textBox3.Text) == false)
            {
                MessageBox.Show("Mobile Number Is Invalid");
                textBox3.Focus();
                return false;
            }
            else if ((int)command.ExecuteScalar() != 0)
            {
                MessageBox.Show("Phone Is Already Used");
                textBox3.Focus();
                return false;
            }
            query = "select count(*)from [user] where email='" + textBox6.Text + "'";
             command = new SqlCommand(query, Start.connection);
            var email = new Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (email.IsMatch(textBox6.Text) == false)
            {
                MessageBox.Show("Email Is Incorrect");
                textBox6.Focus();
                return false;
            }
            else if ((int)command.ExecuteScalar() != 0)
            {
                MessageBox.Show("Email Is Already Used");
                textBox6.Focus();
                return false;
            }
             query = "select count(*)from [user] where username='" + textBox7.Text + "'";
             command = new SqlCommand(query, Start.connection);
            if (textBox7.Text.Length < 4)
            {
                MessageBox.Show("Enter A Valid Username of atleast 4 characters");
                textBox7.Focus();
                return false;
            }
            else if((int)command.ExecuteScalar() != 0)
            {
                MessageBox.Show("Username Is Already Used");
                textBox7.Focus();
                return false;
            }
            var password = new Regex(@"(?=^.{8,12}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.");
            if (password.IsMatch(textBox8.Text) == false)
            {
                MessageBox.Show("Use A Powerful Password(UpperCase,LowerCase And Numbers)");
                textBox8.Focus();
                return false;
            }
         if(textBox4.Text.Length==0)
            {
                MessageBox.Show("Enter A Correct Age");
                textBox4.Focus();
                return false;
            }
            if (comboBox2.SelectedItem==null)
            {
                MessageBox.Show("Choose A City");
                comboBox2.Focus();
                return false;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Choose A BloodType");
                comboBox1.Focus();
                return false;
            }
            if (!(this.radioButton1.Checked || this.radioButton2.Checked))

            {

                MessageBox.Show("Select Gender");
                radioButton1.Focus();
                return false;

            }
            return true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox1.Items.IndexOf(comboBox1.SelectedItem);
            switch (index)
            {
                case 0:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.ap;
                    break;
                case 1:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.an;
                    break;
                case 2:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.bp;
                    break;
                case 3:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.bn;
                    break;
                case 4:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.op;
                    break;
                case 5:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.on;
                    break;
                case 6:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.abp;
                    break;
                case 7:
                    this.pictureBox4.Image = global::BBsystem.Properties.Resources.abn;
                    break;
            }
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            var TP = new ToolTip();
            TP.ShowAlways = true;
            TP.SetToolTip(textBox3, "01*********");
            TP.SetToolTip(textBox6, "yourname@mail.com");
            TP.SetToolTip(textBox7, "Choose a unique username");
            TP.SetToolTip(textBox8, "Password must be at least 8 letters ,contains Upper and lower Case and numbers");
        }

    }
}

        


