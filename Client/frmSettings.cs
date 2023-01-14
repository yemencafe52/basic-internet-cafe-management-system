using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {
            bool res = false;

            textBox1.Text = ParamInfo.IP;
            textBox2.Text = ParamInfo.Password;
            textBox3.Text = ParamInfo.Password;
            textBox4.Text = ParamInfo.Name;

            return res;
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

            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.Focus();
                return;
            }

            if ((textBox3.Text != textBox2.Text ))
            {
                textBox3.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox4.Text))
            {
                textBox4.Focus();
                return;
            }

            if(!ParamInfo.UpdateConfig(textBox1.Text,textBox2.Text,textBox4.Text))
            {
                MessageBox.Show("تعذر تنفيذ العملية");
                return;
            }

            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
