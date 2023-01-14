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
    public partial class frmLogin : Form
    {
        private bool has_logined = false;
        int counter = 0;

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
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(textBox1.Text == ParamInfo.Password)
            {
                has_logined = true;
                this.Close();
            }
            else
            {
                textBox1.Text = "";
                textBox1.Focus();
            }

            counter++;

            if(counter >= 3)
            {
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
