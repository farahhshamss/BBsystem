using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BBsystem
{
    enum authority
    {
        User =1,
        Hospital,
        Admin
    }
    public partial class AdminPanel : Form
    {
        DataTable dbdataset;
        public AdminPanel()
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

        private void AdminPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;

        private void AdminPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);

        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            if (Start.connection.State.Equals(ConnectionState.Closed))
            {
                Start.connection.Open();
            }
            cc();


            var cmd = new SqlCommand("select subject,recivedate from contactMSG ORDER BY recivedate DESC;", Start.connection);
            try
            {
                var sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                dbdataset = new DataTable();
                sda.Fill(dbdataset);
                var bsource = new BindingSource();
                bsource.DataSource = dbdataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            var c = new SqlCommand("select username,email,registerdate from [user] ORDER BY registerdate DESC;", Start.connection);
            try
            {
                var da = new SqlDataAdapter {SelectCommand = c};
                var tb = new DataTable();
                da.Fill(tb);
                var source = new BindingSource {DataSource = tb};
                dataGridView2.DataSource = source;
                da.Update(tb);
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);
            }
        }

        private void insert_Click(object sender, EventArgs e)
        {
            
            if (Start.connection.State == ConnectionState.Open)
            {
                if ((txt_FirstName.Text == "") || (txt_LastName.Text == "") || (text_email.Text == "") || (text_username.Text == "") || (text_password.Text == "") || (text_age.Text == "")  || (cbbloodtype.Text == "") || (cbusertype.Text == ""))
                {
                    MessageBox.Show("Please enter all the information required!");
                    return;
                }
                if (!rbmale.Checked && !rbfemale.Checked)
                {
                    MessageBox.Show("Please choose gender");
                    return;
                }
                var gender = rbmale.Checked ? 'm' : 'f';
                var BloodType = Convert.ToInt32(Enum.Parse(typeof(bloodtype), cbbloodtype.Text.Replace("+", "Positive").Replace("-", "Negative")));
                var UserType = Convert.ToInt32(Enum.Parse(typeof(authority), cbusertype.Text)) ;
                var q = $"INSERT INTO [User] VALUES ('{ char.ToUpper(txt_FirstName.Text[0]) + txt_FirstName.Text.Substring(1)}','{ char.ToUpper(txt_LastName.Text[0]) + txt_LastName.Text.Substring(1)}','{text_phone.Text}','{cbcity.Text}',{text_age.Text},{BloodType},'{gender}','{text_email.Text}','{text_username.Text}','{text_password.Text}',{UserType},GETDATE());";
                var cmd = new SqlCommand(q, Start.connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("The values has inserted");
            }

          
            txt_FirstName.Text = "";
            txt_LastName.Text = "";
            text_phone.Text = "";
            text_email.Text = "";
            text_username.Text = "";
            text_password.Text = "";
            text_age.Text = "";
            cbcity.Text = "";
            cbbloodtype.Text = "";
            cbusertype.Text = "";
            gbgender.Text = "";
        }

        private void remove_Click(object sender, EventArgs e)
        {
            DialogResult remove = MessageBox.Show("Remove user account ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (remove == DialogResult.No)
                return;

            var d = new SqlDataAdapter("Select username from [User] where username='" + usertxt.Text + "'", Start.connection);
            var t = new DataTable();
            d.Fill(t);
            if (t.Rows.Count == 0)
            {
                MessageBox.Show("please enter a valid username!");
            }
            else
            {
                var cmd = Start.connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "delete from [User] where username='" + usertxt.Text + "'";
                cmd.ExecuteNonQuery();

                Fn.Text = "";
                ln.Text = "";
                phone.Text = "";
                mail.Text = "";
                usernam.Text = "";
                password.Text = "";
                age.Text = "";
                city.Text = "";
                usertxt.Text = "";
                bdtype.Text = "";

                MessageBox.Show("record is deleted successfully!");
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            DialogResult update = MessageBox.Show("Update user info ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (update == DialogResult.No)
                return;
            if ((Fn.Text == "") || (ln.Text == "") || (mail.Text == "") || (usernam.Text == "") || (password.Text == "") || (age.Text == "") || (bdtype.Text=="" ) || (usertype.Text=="")||(phone.Text=="")||city.Text=="")
            {
                MessageBox.Show("Please enter all the information required!");
                return;
            }
            if (cbgndr.Text != "m" && cbgndr.Text !="f")
            {
                MessageBox.Show("choose correct gender");

                return;
            }
            var cmd = Start.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            var BloodType = Convert.ToInt32(Enum.Parse(typeof(bloodtype), bdtype.Text.Replace("+", "Positive").Replace("-", "Negative")));
            var UserType = Convert.ToInt32(Enum.Parse(typeof(authority), usertype.Text)) ;

            cmd.CommandText = "update [User] set gender= '"+cbgndr.Text+"',FirstName='" +   char.ToUpper(Fn.Text[0]) + Fn.Text.Substring(1)+ "' ,LastName='" + char.ToUpper(ln.Text[0]) + ln.Text.Substring(1)+ "',phone='" +phone.Text + "' ,city='" + city.Text + "',age=" + age.Text + ",email='" + mail.Text + "',username='" + usernam.Text + "',password='" + password.Text + "',usertype=" + UserType + ",BloodType="+BloodType+" where username='" + usertxt.Text + "'";
            cmd.ExecuteNonQuery();
            Fn.Text = "";
            ln.Text = "";
            phone.Text = "";
            mail.Text = "";
            usernam.Text = "";
            password.Text = "";
            age.Text = "";
            city.Text = "";
            usertype.Text = "";
            bdtype.Text = "";
            cbgndr.Text = "";
            MessageBox.Show("record is updated successfully!");
        }

        private void search_Click(object sender, EventArgs e) { 
        

            var sqlselectquery = "select * From [User] where username='" + usertxt.Text.ToString() + "'";
            var d = new SqlDataAdapter("Select username from [User] where username='" + usertxt.Text + "'", Start.connection);
            var t = new DataTable();
            d.Fill(t);
            if (t.Rows.Count == 0)
            {
                MessageBox.Show("Please enter a valid username!");
             
            }
            else
            {
                var cmd = new SqlCommand(sqlselectquery, Start.connection);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    Fn.Text = (dr["FirstName"].ToString());
                    ln.Text = (dr["LastName"].ToString());
                    phone.Text = (dr["phone"].ToString());
                    mail.Text = dr["email"].ToString();
                    usernam.Text = dr["username"].ToString();
                    password.Text = dr["password"].ToString();
                    age.Text = dr["age"].ToString();
                    city.Text = dr["city"].ToString();
                    usertype.Text = Helper.GetUsertypeString(dr["usertype"].ToString());
                    bdtype.Text = Helper.GetBloodtypeString(dr["BloodType"].ToString());
                    cbgndr.Text = dr["gender"].ToString();

                   

                    dr.Close();
                }
            }
        }

        private void usertxt_TextChanged(object sender, EventArgs e)
        {

            Fn.Text = "";
            ln.Text = "";
            phone.Text = "";
            mail.Text = "";
            usernam.Text = "";
            password.Text = "";
            age.Text = "";
            city.Text = "";
            usertype.Text = "";
            bdtype.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (Id.Text == "")
            {    MessageBox.Show("Request Not Found");
            return; 
            }
            var dadapter = new SqlDataAdapter("select* from FNview("+int.Parse(Id.Text)+")", Start.connection);

            var t = new DataTable();
            dadapter.Fill(t);
            if (t.Rows.Count == 0)
            {
                MessageBox.Show("Please enter a valid ID!");


            }
            else
            {
                var sqlselectquery = " SELECT * FROM DonationRequest WHERE requestId=" + int.Parse(Id.Text);
                var cmd = new SqlCommand(sqlselectquery, Start.connection);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    donorid.Text = (dr["donorId"].ToString());
                    bloodtype.Text = (dr["bloodtype"].ToString());
                    redate.Text = (dr["requestdate"].ToString());
                    complete.Text = (dr["completed"].ToString());

                }
                dr.Close();
                var cm = Start.connection.CreateCommand();
                cm.CommandType = CommandType.Text;
                cm.CommandText = " SELECT * FROM [user] WHERE UserID=" + int.Parse(donorid.Text);
                cm.ExecuteNonQuery();
                var dt = new DataTable();
                var da = new SqlDataAdapter(cm);
                da.Fill(dt);

                foreach (DataRow d in dt.Rows)
                {

                    username.Text = d["username"].ToString();


                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult donaterequest = MessageBox.Show("Update donate request info ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (donaterequest == DialogResult.No)
                return;
            var cmd = Start.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
                cmd.CommandText = "EXEC p_update  @request_id=" + int.Parse(Id.Text) + ",@complete=" + cbcom.Text;
            cmd.ExecuteNonQuery();

            cbcom.Text = "";

            MessageBox.Show("record is updated successfully!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult donaterequest = MessageBox.Show("Remove donate request ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (donaterequest == DialogResult.No)
                return;
            var dadapter = new SqlDataAdapter("Select requestId from [DonationRequest] where requestId=" + int.Parse(Id.Text), Start.connection);
            var t = new DataTable();
            dadapter.Fill(t);
            if (t.Rows.Count == 0)
            {
                MessageBox.Show("please enter a valid ID!");


            }
            else
            {
                var cmd = Start.connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "delete from DonationRequest where requestId=" + int.Parse(Id.Text);
                cmd.ExecuteNonQuery();

                donorid.Text = "";
                bloodtype.Text = "";
                redate.Text = "";
                complete.Text = "";
                username.Text = "";
                Id.Text = "";



                MessageBox.Show("record is deleted successfully!");
            }
        }

        private void Id_TextChanged(object sender, EventArgs e)
        {
            donorid.Text = "";
            bloodtype.Text = "";
            redate.Text = "";
            complete.Text = "";
            username.Text = "";
        }

        private void cbcom_SelectedIndexChanged(object sender, EventArgs e)
        {
            complete.Text = cbcom.Text;
        }
        public void cc()
        {
            cbsearch.Items.Clear();

            var cmd = Start.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * From contactMSG";
            cmd.ExecuteNonQuery();
            var dt = new DataTable();
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cbsearch.Items.Add(dr["subject"].ToString());
            }

        }

        private void Contact_Click(object sender, EventArgs e)
        {

        }

        private void cbsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmd = Start.connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * From contactMSG where msgID='" +( cbsearch.SelectedIndex+1) + "'";
            var cl = Start.connection.CreateCommand();
            cmd.ExecuteNonQuery();
            var dt = new DataTable();
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {

                e1.Text = dr["email"].ToString();
                m1.Text = dr["message"].ToString();
                f1.Text = dr["fname"].ToString();

                cl.CommandType = CommandType.Text;
                cl.CommandText = "Select* from [User] where email = '" + e1.Text + "'";
                var d = new SqlDataAdapter("Select email from [User] where email='" + e1.Text + "'", Start.connection);
                var t = new DataTable();
                d.Fill(t);
                if (t.Rows.Count > 0)
                {
                    var readd = cl.ExecuteReader();
                    if (readd.Read())
                    {
                        u1.Text = (readd["username"].ToString());

                    }
                    readd.Close();


                }
                else

                {
                    u1.Text = "not registed";
                }

            }
        
    }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Edit_Click(object sender, EventArgs e)
        {

        }

    }
}
