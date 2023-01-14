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
    public partial class frmCounter : Form
    {
        private int ticker = 0;
        private frmMain parent;
        public  frmCounter(frmMain parent)
        {
            InitializeComponent();
            this.parent = parent;
            Update();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.ticker = Environment.TickCount;
            linkLabel3.Visible = true;
            linkLabel4.Visible = true;
            this.Height = 92;
        }

        private void Update()
        {
            this.Top = 0;
            this.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);
            this.Width = 107;

            label1.Text = Session.Cost.ToString("#0.#0");

            if (ClientConnection.IsConnected)
            {
                label1.ForeColor = Color.White;
                linkLabel1.Enabled = true;
            }
            else
            {
                linkLabel1.Enabled = false;
                label1.ForeColor = Color.Red;
            }

            if (this.Height != 37)
            {
                if (Environment.TickCount - ticker > 1000 * 5)
                {
                    linkLabel3.Visible = false;
                    linkLabel4.Visible = false;
                    this.Height = 37;

                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult res = MessageBox.Show("هل تريد إنهاء الجلسة فعلاً","",MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                ClientConnection.Send(new Packet(Command.CLOSE));
            }
        }

        private  void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin fl = new frmLogin();
            fl.ShowDialog();

            if (fl.HasLogined)
            {
                lock (Constants.MyLocker)
                {
                    parent.CloseToSetup = true;
                    this.Close();
                }
            }

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("نظام ادارة مقاهي الانترنت لفريق يمن كافي");
        }

        private void frmCounter_Load(object sender, EventArgs e)
        {
            Update();
        }
    }
}
