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
    public partial class FormSelProfitCollectReport : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormSelProfitCollectReport()
        {
            InitializeComponent();
        }

        private void FormSelProfitCollectReport_Load(object sender, EventArgs e)
        {
            //设置用户的操作权限
            commUse.CortrolButtonEnabled(btnQuery, this);
            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            //设置产品默认不选择任何项
            cbxInvenCode.SelectedIndex = -1;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string strCondition = null;//声明表示SQL语句的字符串

            if (dtpStartDate.Value.Date > dtpEndDate.Value.Date)
            {
                MessageBox.Show("开始日期不许大于结束日期", "软件提示");
                return;
            }
            //查询已审核的销售出库单
            strCondition = "Select * From SEOutStore Where SEOutStore.IsFlag = '1' ";

            //起始日期
            if (dtpStartDate.ShowCheckBox == true)//在日期控件中显示复选框
            {
                if (dtpStartDate.Checked == true)//选中日期控件中的复选框
                {
                    //单据日期大于或等于开始日期
                    strCondition += " and  SEOutStore.SEOutDate >= '" + dtpStartDate.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }
            //截止日期
            if (dtpEndDate.ShowCheckBox == true)//在日期控件中显示复选框
            {
                if (dtpEndDate.Checked == true)//选中日期控件中的复选框
                {
                    //单据日期小于或等于结束日期
                    strCondition += " and SEOutStore.SEOutDate <= '" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }
            if (cbxInvenCode.SelectedValue != null)
            {
                //指定产品
                strCondition += " and SEOutStore.InvenCode  = '" + cbxInvenCode.SelectedValue.ToString() + "' ";
            }
            //将统计得到的报表绑定到CrystalReportViewer控件
            crySelProfitCollectReport.ReportSource = commUse.CrystalReports("CrystalSelProfitCollectReport.rpt", strCondition, "SEOutStore");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
