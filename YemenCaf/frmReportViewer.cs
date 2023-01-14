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
    public partial class frmReportViewer : Form
    {
       
        public frmReportViewer()
        {
            InitializeComponent();
           
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
