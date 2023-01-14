using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YemenCafe
{
    public partial class frmLogin : Form
    {

        private bool has_logined = false;
        private byte counter = 0;

        internal bool HasLogined
        {
            get
            {
                return this.has_logined;
            }
        }
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                return;
            }

            User user = new User(0, "", "", 0);
            if (UserManager.Login(textBox1.Text,textBox2.Text,ref user))
            {
                this.has_logined = true;
                this.Close();
            }
            else
            {
                counter++;
                if(counter > 2)
                {
                    MessageBox.Show(" وغير مصحرح لك بالولوج.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("اسم المستخدم او كلمة المرور غير صحيحة");
                }
            }
        }
    }
}
