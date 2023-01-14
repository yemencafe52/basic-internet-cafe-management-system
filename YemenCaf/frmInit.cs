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
    public partial class frmInit : Form
    {

        private bool success = false;

        internal bool Success
        {
            get
            {
                return this.success;
            }
        }
        public frmInit()
        {
            InitializeComponent();
        }

        private void frmInit_Shown(object sender, EventArgs e)
        {
            //var p = new Progress<int>{(x=>
            
            
            //});
          
            success = SessionManager.Init();
        }
    }
}
