using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP.ComClass;
using ERP.DataClass;

namespace ERP.RP.FORM
{
    public partial class FormPurReport : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormPurReport()
        {
            InitializeComponent();
        }

        private void FormPurReport_Load(object sender, EventArgs e)
        {
            //权限
            commUse.CortrolButtonEnabled(btnQuery, this);
            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxSupplierCode, "SupplierCode", "SupplierName", "select SupplierCode,SupplierName from BSSupplier", "BSSupplier");
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            cbxSupplierCode.SelectedIndex = -1;
            cbxInvenCode.SelectedIndex = -1;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string strCondition = null; 

            if (dtpStartDate.Value.Date > dtpEndDate.Value.Date)
            {
                MessageBox.Show("开始日期不许大于结束日期","软件提示");
                return;
            }

            strCondition = "Select * From PUInStore Where IsFlag = '1' ";

            //起始日期
            if (dtpStartDate.ShowCheckBox == true)
            {
                if (dtpStartDate.Checked == true)
                {
                    strCondition += " and PUInDate >= '" + dtpStartDate.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }

            //截止日期
            if (dtpEndDate.ShowCheckBox == true)
            {
                if (dtpEndDate.Checked == true)
                {
                    strCondition += " and PUInDate <= '" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }

            //供应商
            if(cbxSupplierCode.SelectedValue != null)
            {
                strCondition += " and SupplierCode = '" + cbxSupplierCode.SelectedValue.ToString() + "' ";
            }

            //原材料
            if (cbxInvenCode.SelectedValue != null)
            {
                strCondition += " and InvenCode = '" + cbxInvenCode.SelectedValue.ToString() + "' ";
            }

            cryPurReport.ReportSource = commUse.CrystalReports("CryPurReport.rpt", strCondition, "PUInStore");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
