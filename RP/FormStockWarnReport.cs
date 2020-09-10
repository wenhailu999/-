using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP.DataClass;
using ERP.ComClass;

namespace ERP.RP.FORM
{
    public partial class FormStockWarnReport : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormStockWarnReport()
        {
            InitializeComponent();
        }

        private void FormStockWarnReport_Load(object sender, EventArgs e)
        {
            //权限
            commUse.CortrolButtonEnabled(btnQuery, this);
            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxStoreCode, "StoreCode", "StoreName", "select StoreCode,StoreName from BSStore", "BSStore");
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            //
            cbxStoreCode.SelectedIndex = -1;
            cbxInvenCode.SelectedIndex = -1;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string strCondition = null;
            strCondition = "( {STStock.Quantity} < {BSInven.SmallStockNum} or {STStock.Quantity} > {BSInven.BigStockNum} )";

            //存货
            if (cbxStoreCode.SelectedValue != null)
            {
                strCondition += " and {STStock.StoreCode} = '" + cbxStoreCode.SelectedValue.ToString() + "' ";
            }

            //仓库
            if (cbxInvenCode.SelectedValue != null)
            {
                strCondition += " and {STStock.InvenCode} = '" + cbxInvenCode.SelectedValue.ToString() + "' ";
            }

            cryStockWarnReport.ReportSource = commUse.CrystalReports("CryStockWarnReport.rpt", strCondition);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
