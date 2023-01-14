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
    public partial class frmReport : Form
    {
        private List<User> users = new List<User>();

        public frmReport()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {

            comboBox1.Items.Add("تفصيلي");
            comboBox1.Items.Add("إجمالي");
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;

            comboBox2.Items.Add("الكل");

            for (int i = 0; i < SessionManager.GetStations.Count; i++)
            {
                comboBox2.Items.Add(i + 1);
            }

            comboBox2.SelectedIndex = 0;

            users.Add(new User(0, "الكل", "", 0));
            users.AddRange(UserManager.GetUsers());


            comboBox3.ValueMember = "Number";
            comboBox3.DisplayMember = "UserName";
            comboBox3.DataSource = users;
            comboBox3.SelectedIndex = 0;

            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePicker2.Value = DateTime.Now;

            return true;

        }
        private void frmReport_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmReportViewer frmRV = new frmReportViewer();
            DataTable dt = new DSet.tblSessionsDataTable();

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {

                        if (comboBox2.SelectedIndex == 0)
                        {
                            if (comboBox3.SelectedIndex == 0)
                            {
                                dt = Report.GetSessionsInfo(dateTimePicker1.Value, dateTimePicker2.Value);
                            }
                            else
                            {
                                dt = Report.GetSessionsInfo(dateTimePicker1.Value, dateTimePicker2.Value, new User (byte.Parse(comboBox3.SelectedValue.ToString())));
                            }
                        }
                        else
                        {
                            if (comboBox3.SelectedIndex == 0)
                            {
                                dt = Report.GetSessionsInfo(dateTimePicker1.Value, dateTimePicker2.Value, new Station(byte.Parse(comboBox2.SelectedIndex.ToString())));
                            }
                            else
                            {
                                dt = Report.GetSessionsInfo(dateTimePicker1.Value, dateTimePicker2.Value, new Station(byte.Parse(comboBox2.SelectedIndex.ToString())), new User(byte.Parse(comboBox3.SelectedValue.ToString())));
                            }
                        }
                        break;
                    }

                  


            }

            
            Microsoft.Reporting.WinForms.ReportDataSource rpt = new Microsoft.Reporting.WinForms.ReportDataSource("tblSessions", dt);

            
            frmRV.reportViewer1.LocalReport.DataSources.Clear();

            // عرض الفترة في التقرير 
            string desc = " للفترة تبدأ من "+ dateTimePicker1.Value.ToString("yyyy/MM/dd")  +" وتنتهي إلى  " + dateTimePicker2.Value.ToString("yyyy/MM/dd");
            Microsoft.Reporting.WinForms.ReportParameter[] parameters = new Microsoft.Reporting.WinForms.ReportParameter[1];
            parameters[0] = new Microsoft.Reporting.WinForms.ReportParameter("p", desc);
            frmRV.reportViewer1.LocalReport.SetParameters(parameters);

            frmRV.reportViewer1.LocalReport.DataSources.Add(rpt);
            frmRV.reportViewer1.RefreshReport();
            frmRV.Show();
        }
    }
}
