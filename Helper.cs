using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBsystem
{
    static class Helper
    {
        public static void StartForm(this Form mainForm,Form Startform)
        {
            mainForm.Visible = false;
            if (Startform.ShowDialog() == DialogResult.Abort)
            {
                mainForm.DialogResult = DialogResult.Abort;
                Startform.Dispose();
                Application.Exit();
                return;
            }

            mainForm.Visible = true;
        }

        public static string GetBloodtypeString(string value)
        {
            return Enum.Parse(typeof(bloodtype), value).ToString().Replace("Positive", "+").Replace("Negative", "-");
        }

        public static string GetUsertypeString(string value)
        {
            return Enum.Parse(typeof(authority), value).ToString();
        }
    }
}
