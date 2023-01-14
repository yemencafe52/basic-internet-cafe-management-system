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
    internal partial class frmNoticeSender : Form
    {
        List<Station> stations = new List<Station>();
        internal frmNoticeSender(Station s)
        {
            InitializeComponent();
            stations.Clear();
            stations.Add(s);
        }

        internal frmNoticeSender(List<Station> ss)
        {
            InitializeComponent();
            stations.Clear();
            stations.AddRange(ss);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                return;
            }

            for(int i=0;i<stations.Count;i++)
            {
                if (stations[i].SessionInfo.SessionState == State.OPENND)
                {
                    byte[] ar = Encoding.UTF8.GetBytes(textBox1.Text);
                    stations[i].Send(new Packet(Command.CLOSE, ar)); // msg
                }
            }

            this.Close();
        }
    }
}
