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
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            Preparing();
        }

        private void Preparing()
        {
            numericUpDown1.Value = (decimal)ParamInfo.GetUnitPerSecond;
            numericUpDown2.Value = (decimal)ParamInfo.PricePerUnit;
            numericUpDown3.Value = (decimal)ParamInfo.StationsCount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ParamInfo.UpdateParamInfo((double)numericUpDown1.Value, (double)numericUpDown2.Value, "C:\\" ,(byte)numericUpDown3.Value))
            {
                MessageBox.Show("تعذر اتمام العملية المطلوبة");
                return;
            }

            MessageBox.Show("يجب اعادة تشغيل النظام لتفعيل الاعدادات");
            this.Close();

        }
    }
}
