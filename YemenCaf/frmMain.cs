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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Preparing();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private bool Preparing()
        {
            PrintStaions();
            return true;
        }

        private void PrintStaions()
        {

            listView1.Items.Clear();

            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(SessionManager.GetStations[i].Name);
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");

                listView1.Items.Add(lvi);

            }
        }

        private void UpdateStationInfo(Station station)
        {
            ListViewItem lvi = listView1.FindItemWithText(station.Name);
            if (lvi != null)
            {
                int index = lvi.Index;

                listView1.Items[index].SubItems[1].Text = station.SessionInfo.SessionState.ToString();
                listView1.Items[index].SubItems[2].Text = TimeSpan.FromSeconds((int)station.SessionInfo.TimeLeft).Duration().ToString();
                listView1.Items[index].SubItems[3].Text = station.SessionInfo.Cost.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                UpdateStationInfo(SessionManager.GetStations[i]);
                Application.DoEvents();
            }

            toolStripStatusLabel1.Text = UserManager.GetActiveUser.UserName;
            toolStripStatusLabel2.Text = DateTime.Now.ToString();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int index = listView1.SelectedItems[0].Index;
                string name = listView1.Items[index].Text;
                Station s = SessionManager.GetStations.Find(p => p.Name == name);
                bool res = false;

                if (s != null)
                {
                    switch (s.SessionInfo.SessionState)
                    {
                        case State.CLOSED:
                            {
                                res = SessionManager.OpenNewSession(s);
                                break;
                            }
                        case State.OPENND:
                            {
                                res = SessionManager.CloseSession(s);
                                break;
                            }
                        case State.WAITTING:
                            {
                                res = SessionManager.EndSession(s);
                                break;
                            }
                    }
                }


                if (res)
                {
                    UpdateStationInfo(s);
                }
                else
                {
                    MessageBox.Show("تعذر تنفيذ العملية المطلوبة");
                }
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listView1_DoubleClick(sender, new EventArgs());
            }
            else if (e.KeyCode == Keys.Space)
            {
                listView1_DoubleClick(sender, new EventArgs());
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Event ee = new Event();

            while (LogManager.ReadEvent(ref ee))
            {
                ListViewItem lvi = new ListViewItem(ee.GetMessage);
                lvi.SubItems.Add(ee.Time.ToString());
                listView2.Items.Add(lvi);
                listView2.EnsureVisible(listView2.Items.Count - 1);

                Application.DoEvents();
            }
        }

        private void closeAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // bool res = false;


            if (listView1.SelectedItems.Count > 0)
            {
                int index = listView1.SelectedItems[0].Index;
                string name = listView1.Items[index].Text;
                Station s = SessionManager.GetStations.Find(p => p.Name == name);
               
                if (s != null)
                {
                    switch (s.SessionInfo.SessionState)
                    {
                        case State.OPENND:
                            {
                                if (!(s.Send(new Packet(Command.UPDATE))))
                                {
                                    LogManager.AddNewEvent(new Event("تعذر ارسال امر اغلاق كافة التطبيقات للمحطة " + s.Name, DateTime.Now));
                                }

                                break;
                            }
                        default:
                            {
                                MessageBox.Show("يجب ان تكون حالة المحطة مفتوحة ");
                                break;
                            }
                    }

                }
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutBox fab = new frmAboutBox();
            fab.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count >0)
            {

                bool res = false;
             
                string name = listView1.SelectedItems[0].Text;

                Station from = SessionManager.GetStations.Find(p => p.Name == name);
                if (from != null)
                {
                    frmStationSelector fss = new frmStationSelector();
                    fss.ShowDialog();

                    Station to = fss.GetStation;

                    res = SessionManager.Swap(from, to);

                    if(!res)
                    {
                        MessageBox.Show("تعذر تنفيذ العملية");
                    }

                }


            }
        }

        private void closeAppsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            { 
                string name = listView1.SelectedItems[0].Text;

                Station s = SessionManager.GetStations.Find(p => p.Name == name);
                if (s != null)
                {
                    s.Send(new Packet(Command.CLOSE)); // close opps 
                }
            }
        }

        private void sendNotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string name = listView1.SelectedItems[0].Text;

                Station s = SessionManager.GetStations.Find(p => p.Name == name);
                if (s != null)
                {
                    if (s.SessionInfo.SessionState == State.OPENND)
                    {

                        frmNoticeSender fns = new frmNoticeSender(s);
                        fns.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("لا يمكن تنفيذ الامر حاليا");

                }
            }

        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string name = listView1.SelectedItems[0].Text;

                Station s = SessionManager.GetStations.Find(p => p.Name == name);
                if (s != null)
                {
                    s.Send(new Packet(Command.CLOSE)); // restart
                }
            }
        }

        private void shutDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string name = listView1.SelectedItems[0].Text;

                Station s = SessionManager.GetStations.Find(p => p.Name == name);
                if (s != null)
                {
                    s.Send(new Packet(Command.CLOSE)); // shutdown
                }
            }
        }

        private void openAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i=0;i<SessionManager.GetStations.Count;i++)
            {
                if (SessionManager.GetStations[i].SessionInfo.SessionState == State.CLOSED)
                {
                    SessionManager.OpenNewSession(SessionManager.GetStations[i]);
                }

                Application.DoEvents();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                if (SessionManager.GetStations[i].SessionInfo.SessionState == State.OPENND)
                {
                    SessionManager.CloseSession(SessionManager.GetStations[i]);
                }

                Application.DoEvents();
            }
        }

        private void endAllSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                if (SessionManager.GetStations[i].SessionInfo.SessionState == State.WAITTING)
                {
                    SessionManager.EndSession(SessionManager.GetStations[i]);
                }

                Application.DoEvents();
            }
        }

        private void shutDownAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                if (SessionManager.GetStations[i].SessionInfo.SessionState == State.CLOSED)
                {
                    SessionManager.GetStations[i].Send(new Packet(Command.CLOSE)); // shutdown
                }

                Application.DoEvents();
            }
        }

        private void restartAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // wakeup on lan 
        }

        private void notieForAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNoticeSender fns = new frmNoticeSender(SessionManager.GetStations);
            fns.ShowDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(logToolStripMenuItem.Checked)
            {
                logToolStripMenuItem.Checked = false;
                splitContainer1.Panel2Collapsed = true;
            }
            else
            {
                logToolStripMenuItem.Checked = true;
                splitContainer1.Panel2Collapsed = false;
            }
        }

        private void dataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            DialogResult res = sfd.ShowDialog();

            if (res == DialogResult.OK)
            {
                string path = sfd.FileName;
                if(!Utilities.BackupDB(path))
                {
                    MessageBox.Show("تعذر تنفيذ العملية");
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode tv = treeView1.SelectedNode;

            if (tv!= null)
            {
                if(tv.Tag != null)
                {
                    switch(tv.Tag.ToString())
                    {
                        case "1":
                            {
                                frmSettings fs = new frmSettings();
                                fs.ShowDialog();
                                break;
                            }

                        case "2":
                            {
                                frmUsersViewer fuv = new frmUsersViewer();
                                fuv.ShowDialog();
                                break;
                            }

                        case "3":
                            {
                                frmReport r = new frmReport();
                                r.Show();
                                // report 
                                break;
                            }

                        case "4":
                            {
                                frmChangePassword fcp = new frmChangePassword();
                                fcp.ShowDialog();
                                break;
                            }

                        case "5":
                            {
                                break;
                            }

                        case "6":
                            {
                                break;
                            }

                    }

                }
            }
        }
    }
}