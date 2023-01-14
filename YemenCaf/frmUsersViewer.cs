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
    public partial class frmUsersViewer : Form
    {
        public frmUsersViewer()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {
            bool res = false;

            try
            {
                List<User> users = UserManager.GetUsers();
                PrintUsers(users);
                res = true;
            }
            catch
            {

            }

            return res;
        }

        private void PrintUsers(List<User> users)
        {
            listBox1.Items.Clear();

            foreach(User user in users)
            {
                listBox1.Items.Add(user.UserName);
            }

            toolStripStatusLabel2.Text = users.Count.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmUserMgrUI fumui = new frmUserMgrUI();
            fumui.ShowDialog();
            Preparing();
        }
    }
}
