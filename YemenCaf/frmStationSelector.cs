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
    public partial class frmStationSelector : Form
    {
        private Station s = null;

        public frmStationSelector()
        {
            InitializeComponent();
            Preparing();
        }
        internal bool Preparing()
        {
            PrintStations();
            return true;
        }

        internal bool PrintStations()
        {
            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                if (SessionManager.GetStations[i].SessionInfo.SessionState == State.CLOSED)
                {
                    listBox1.Items.Add(SessionManager.GetStations[i].Name);
                }
            }
            return true;
        }

        internal Station GetStation
        {
            get
            {
                return this.s;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count > 0)
            {
                string name = listBox1.SelectedItems[0].ToString();
                this.s = SessionManager.GetStations.Find(p => p.Name == name);
                this.Close();
            }
        }
    }
}
