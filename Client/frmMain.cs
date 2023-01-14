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
    public partial class frmMain : Form
    {
        private frmCounter fcounter;
        private bool closeToSetup = false;
        private int ticker = 0;
       
        public frmMain()
        {
            InitializeComponent();
            fcounter = new frmCounter(this);
        }

        internal bool CloseToSetup
        {
            get
            {
                return closeToSetup;
            }
            set
            {
                closeToSetup = value;
            }
        }


        private void frmMain_Resize(object sender, EventArgs e)
        {
            groupBox1.Left = (this.Width / 2) - (groupBox1.Width / 2);
            groupBox1.Top = (this.Height / 2) - ((groupBox1.Height / 2)+ splitContainer1.Panel1.Height);

            groupBox2.Left = groupBox1.Left;
            groupBox2.Top = groupBox1.Top;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (Constants.MyLocker)
            {
                if (this.closeToSetup)
                {
                    timer1.Enabled = false;

                    if (fcounter != null)
                    {
                        fcounter.Close();
                    }

                    this.Close();
                    return;
                }

                if (Session.SessionState == State.OPENND)
                {
                    this.Hide();
                    fcounter.Show();
                }
                else
                {
                    this.Show();
                    fcounter.Hide();
                }

                if (ClientConnection.IsConnected)
                {
                    groupBox1.BackColor = Color.DarkGreen;
                    linkLabel1.Enabled = true;
                }
                else
                {

                    groupBox1.BackColor = Color.DarkRed;
                    linkLabel1.Enabled = false;
                }

                label4.Text = DateTime.Now.ToString();

                if(groupBox2.Visible==true)
                {
                    if(Environment.TickCount - this.ticker > 1000*10)
                    {
                        groupBox1.Visible = true;
                        groupBox2.Visible = false;
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                return;
            }

            if(textBox1.Text == ParamInfo.Password)
            {
                ClientConnection.Stop();

            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            ticker = Environment.TickCount;
            textBox1.Focus();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(textBox1.Text == ParamInfo.Password)
            {  
                this.closeToSetup = true;
            }
            else
            {
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClientConnection.Send(new Packet(Command.OPEN));
        }
    }
}
        