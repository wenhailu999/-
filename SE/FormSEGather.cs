﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ERP.ComClass;
using ERP.DataClass;

namespace ERP.SE
{
    public partial class FormSEGather : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormSEGather()
        {
            InitializeComponent();
        }

        private void ControlStatus()
        {
            //工具栏按钮状态切换
            this.toolSave.Enabled = !this.toolSave.Enabled;
            this.toolCancel.Enabled = !this.toolCancel.Enabled;
            commUse.CortrolButtonEnabled(toolAdd, this);
            commUse.CortrolButtonEnabled(toolAmend, this);
            commUse.CortrolButtonEnabled(toolDelete, this);
            commUse.CortrolButtonEnabled(toolCheck, this);
            commUse.CortrolButtonEnabled(toolUnCheck, this);

            //窗体控件状态切换
            //this.txtSEOutCode.ReadOnly = !this.txtSEOutCode.ReadOnly;
            this.btnChoice.Enabled = !this.btnChoice.Enabled;
            this.txtSEMoney.ReadOnly = !this.txtSEMoney.ReadOnly;
            this.cbxAccountCode.Enabled = !this.cbxAccountCode.Enabled;
            this.cbxEmployeeCode.Enabled = !this.cbxEmployeeCode.Enabled;
            this.txtRemark.ReadOnly = !this.txtRemark.ReadOnly;
        }

        /// <summary>
        /// 将控件恢复到原始状态
        /// </summary>
        private void ClearControls()
        {
            this.txtSEGatherCode.Text = "";
            this.dtpSEGatherDate.Value = Convert.ToDateTime("1900-01-01");
            this.cbxOperatorCode.SelectedIndex = -1;
            this.txtSEOutCode.Text = "";
            this.dtpSEOutDate.Value = Convert.ToDateTime("1900-01-01");
            this.cbxCustomerCode.SelectedIndex = -1;
            this.txtSEMoney.Text = "";
            this.cbxAccountCode.SelectedIndex = -1;
            this.cbxEmployeeCode.SelectedIndex = -1;
            this.txtRemark.Text = "";
            this.cbxIsFlag.SelectedIndex = -1;
        }

        private void BindToolStripComboBox()
        {
            this.cbxCondition.Items.Add("单据编号");
            this.cbxCondition.Items.Add("单据日期");
        }

        /// <summary>
        /// 设置控件的显示值
        /// </summary>
        private void FillControls()
        {
            this.txtSEGatherCode.Text = this.dgvSEGatherInfo[0, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            this.dtpSEGatherDate.Value = Convert.ToDateTime(this.dgvSEGatherInfo[1, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value);
            this.cbxOperatorCode.SelectedValue = this.dgvSEGatherInfo[2, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value;
            this.txtSEOutCode.Text = this.dgvSEGatherInfo[3, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            this.dtpSEOutDate.Value = Convert.ToDateTime(this.dgvSEGatherInfo[4, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value);
            this.cbxCustomerCode.SelectedValue = this.dgvSEGatherInfo[5, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value;
            this.txtSEMoney.Text = this.dgvSEGatherInfo[6, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            this.cbxAccountCode.SelectedValue = this.dgvSEGatherInfo[7, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value;
            this.cbxEmployeeCode.SelectedValue = this.dgvSEGatherInfo[8, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value;
            this.txtRemark.Text = this.dgvSEGatherInfo[9, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            this.cbxIsFlag.SelectedValue = this.dgvSEGatherInfo[10, this.dgvSEGatherInfo.CurrentCell.RowIndex].Value;
        }

        /// <summary>
        /// DataGridView控件绑定到数据源
        /// </summary>
        /// <param name="strWhere">Where条件子句</param>
        private void BindDataGridView(string strWhere)
        {
            string strSql = null;

            strSql = "SELECT * ";
            strSql += "FROM SEGather " + strWhere; ;

            try
            {
                this.dgvSEGatherInfo.DataSource = db.GetDataSet(strSql, "SEGather").Tables["SEGather"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "软件提示");
                throw ex;
            }
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        private void ParametersAddValue()
        {
            db.Cmd.Parameters.Clear();
            db.Cmd.Parameters.AddWithValue("@SEGatherCode", txtSEGatherCode.Text.Trim());
            db.Cmd.Parameters.AddWithValue("@SEGatherDate", dtpSEGatherDate.Value);

            if (cbxOperatorCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@OperatorCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@OperatorCode", cbxOperatorCode.SelectedValue.ToString());
            }

            db.Cmd.Parameters.AddWithValue("@SEOutCode", txtSEOutCode.Text.Trim());
            db.Cmd.Parameters.AddWithValue("@SEOutDate", dtpSEOutDate.Value);

            if (cbxCustomerCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@CustomerCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@CustomerCode", cbxCustomerCode.SelectedValue.ToString());
            }

            if (String.IsNullOrEmpty(txtSEMoney.Text.Trim()))
            {
                //把null对象化为DBNull
                db.Cmd.Parameters.AddWithValue("@SEMoney", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@SEMoney", Convert.ToDecimal(txtSEMoney.Text.Trim()));
            }

            if (cbxAccountCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@AccountCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@AccountCode", cbxAccountCode.SelectedValue.ToString());
            }

            if (cbxEmployeeCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@EmployeeCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@EmployeeCode", cbxEmployeeCode.SelectedValue.ToString());
            }

            db.Cmd.Parameters.AddWithValue("@Remark", txtRemark.Text.Trim());

            if (cbxIsFlag.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@IsFlag", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@IsFlag", cbxIsFlag.SelectedValue.ToString());
            }
        }

        private void FormSEGather_Load(object sender, EventArgs e)
        {
            //权限
            commUse.CortrolButtonEnabled(toolAdd, this);
            commUse.CortrolButtonEnabled(toolAmend, this);
            commUse.CortrolButtonEnabled(toolDelete, this);
            commUse.CortrolButtonEnabled(toolCheck, this);
            commUse.CortrolButtonEnabled(toolUnCheck, this);

            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxOperatorCode, "OperatorCode", "OperatorName", "select OperatorCode,OperatorName from SYOperator", "SYOperator");
            commUse.BindComboBox(cbxCustomerCode, "CustomerCode", "CustomerName", "select CustomerCode,CustomerName from BSCustomer", "BSCustomer");
            commUse.BindComboBox(cbxAccountCode, "AccountCode", "AccountName", "select AccountCode,AccountName from BSAccount", "BSAccount");
            commUse.BindComboBox(cbxEmployeeCode, "EmployeeCode", "EmployeeName", "select EmployeeCode,EmployeeName from BSEmployee", "BSEmployee");
            commUse.BindComboBox(cbxIsFlag, "Code", "Name", "select * from INCheckFlag", "INCheckFlag");

            //DataGridViewComboBoxColumn绑定到数据源
            commUse.BindComboBox(this.dgvSEGatherInfo.Columns["OperatorCode"], "OperatorCode", "OperatorName", "select OperatorCode,OperatorName from SYOperator", "SYOperator");
            commUse.BindComboBox(this.dgvSEGatherInfo.Columns["CustomerCode"], "CustomerCode", "CustomerName", "select CustomerCode,CustomerName from BSCustomer", "BSCustomer");
            commUse.BindComboBox(this.dgvSEGatherInfo.Columns["AccountCode"], "AccountCode", "AccountName", "select AccountCode,AccountName from BSAccount", "BSAccount");
            commUse.BindComboBox(this.dgvSEGatherInfo.Columns["EmployeeCode"], "EmployeeCode", "EmployeeName", "select EmployeeCode,EmployeeName from BSEmployee", "BSEmployee");
            commUse.BindComboBox(this.dgvSEGatherInfo.Columns["IsFlag"], "Code", "Name", "select * from INCheckFlag", "INCheckFlag");

            BindDataGridView("");
            this.BindToolStripComboBox();
            this.cbxCondition.SelectedIndex = 0;
            toolStrip1.Tag = "";
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "ADD"; //添加标识

            dtpSEGatherDate.Value = DateTime.Today;
            cbxOperatorCode.SelectedValue = PropertyClass.OperatorCode;
            cbxIsFlag.SelectedValue = "0";

            txtSEGatherCode.Text = commUse.BuildBillCode("SEGather", "SEGatherCode", "SEGatherDate", dtpSEGatherDate.Value);
        }

        private void toolAmend_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "EDIT"; //修改标识
        }

        private void dgvSEGatherInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (toolStrip1.Tag.ToString() == "EDIT" && dgvSEGatherInfo.RowCount > 0)
            {
                if (this.dgvSEGatherInfo[10, this.dgvSEGatherInfo.CurrentRow.Index].Value.ToString() == "1")
                {
                    MessageBox.Show("该记录已审核，不允许编辑！", "软件提示");
                    return;
                }

                FillControls();
            }
        }

        private void toolCancel_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "";
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSEMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputNumeric(e,sender as Control);
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            //实例化已审核的销售出库单窗体
            FormBrowseSEOutStore formBrowseSEOutStore = new FormBrowseSEOutStore();
            formBrowseSEOutStore.Owner = this;//设置拥有此窗体的窗体，即销售收款单窗体
            formBrowseSEOutStore.ShowDialog();//将窗体显示为模式对话框
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            string strCode = null;

            if (String.IsNullOrEmpty(txtSEGatherCode.Text.Trim()))
            {
                MessageBox.Show("单据编号不许为空！","软件提示");
                txtSEGatherCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtSEOutCode.Text.Trim()))
            {
                MessageBox.Show("出库单号不许为空！", "软件提示");
                txtSEOutCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtSEMoney.Text.Trim()))
            {
                MessageBox.Show("收款款金额不许为空！", "软件提示");
                txtSEMoney.Focus();
                return;
            }
            else
            {
                if (Convert.ToDecimal(txtSEMoney.Text.Trim()) == 0)
                {
                    MessageBox.Show("收款金额不能等于零", "软件提示");
                    txtSEMoney.Focus();
                    return;
                }
            }

            if (cbxAccountCode.SelectedValue == null)
            {
                MessageBox.Show("结算账户不许为空！", "软件提示");
                cbxAccountCode.Focus();
                return;
            }

            //添加
            if (toolStrip1.Tag.ToString() == "ADD")
            {
                try
                {
                    strCode = "INSERT INTO SEGather(SEGatherCode,SEGatherDate,OperatorCode,SEOutCode,SEOutDate,CustomerCode,SEMoney,AccountCode,EmployeeCode,Remark,IsFlag) ";
                    strCode += "VALUES(@SEGatherCode,@SEGatherDate,@OperatorCode,@SEOutCode,@SEOutDate,@CustomerCode,@SEMoney,@AccountCode,@EmployeeCode,@Remark,@IsFlag)";

                    ParametersAddValue();

                    if (db.ExecDataBySql(strCode) > 0)
                    {
                        MessageBox.Show("保存成功！", "软件提示");
                        this.BindDataGridView("");
                        ControlStatus();
                    }
                    else
                    {
                        MessageBox.Show("保存失败！", "软件提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }
            }

            //修改
            if (toolStrip1.Tag.ToString() == "EDIT")
            {
                string strSEGatherCode = txtSEGatherCode.Text.Trim();
                //更新数据库
                try
                {
                    strCode = "UPDATE SEGather SET SEGatherCode = @SEGatherCode,SEGatherDate = @SEGatherDate,OperatorCode = @OperatorCode, SEOutCode = @SEOutCode,SEOutDate = @SEOutDate,";
                    strCode += "CustomerCode = @CustomerCode,SEMoney = @SEMoney,AccountCode = @AccountCode,";
                    strCode += "EmployeeCode = @EmployeeCode,Remark = @Remark,IsFlag = @IsFlag ";
                    strCode += "WHERE SEGatherCode = '" + strSEGatherCode + "'";

                    ParametersAddValue();

                    if (db.ExecDataBySql(strCode) > 0)
                    {
                        MessageBox.Show("保存成功！", "软件提示");
                        this.BindDataGridView("");
                        ControlStatus();
                    }
                    else
                    {
                        MessageBox.Show("保存失败！", "软件提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }
            }

            toolStrip1.Tag = "";
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            string strSEGatherCode = null; //单据编号
            string strSql = null;
            string strIsFlag = null; //审核标记

            if (this.dgvSEGatherInfo.RowCount <= 0)
            {
                return;
            }

            strSEGatherCode = this.dgvSEGatherInfo["SEGatherCode", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            strIsFlag = this.dgvSEGatherInfo["IsFlag", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();

            if (strIsFlag == "1")
            {
                MessageBox.Show("该单据已审核，不许删除！", "软件提示");
                return;
            }

            strSql = "DELETE FROM SEGather WHERE SEGatherCode = '" + strSEGatherCode + "'";

            if (MessageBox.Show("确定要删除吗？", "软件提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                try
                {
                    if (db.ExecDataBySql(strSql) > 0)
                    {
                        MessageBox.Show("删除成功！", "软件提示");
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "软件提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }

                this.BindDataGridView("");
            }
        }

        private void toolCheck_Click(object sender, EventArgs e)
        {
            List<string> strSqls = new List<string>();

            decimal decMoney;
            string strSEGatherCode = null; //单据编号
            string strIsFlag = null; //审核标记
            string strAccountCode = null; //帐号代码

            string strPUPaySql = null; //表示提交PUPay表的SQL语句
            string strAccountSql = null; //表示提交BSAccount表的SQL语句

            if (dgvSEGatherInfo.RowCount == 0)
            {
                return;
            }

            strSEGatherCode = this.dgvSEGatherInfo["SEGatherCode", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            strAccountCode = this.dgvSEGatherInfo["AccountCode", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            strIsFlag = this.dgvSEGatherInfo["IsFlag", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            decMoney = Convert.ToDecimal(this.dgvSEGatherInfo["SEMoney", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value);

            if (strIsFlag == "1")
            {
                MessageBox.Show("该单据已审核过，不许再次审核！", "软件提示");
                return;
            }

            strAccountSql = "UPDATE BSAccount SET AccMoney = Accmoney + " + decMoney + " WHERE AccountCode = '" + strAccountCode + "'";
            strPUPaySql = "UPDATE SEGather SET IsFlag = '1' WHERE SEGatherCode = '" + strSEGatherCode + "'";

            strSqls.Add(strAccountSql);
            strSqls.Add(strPUPaySql);

            try
            {
                if (db.ExecDataBySqls(strSqls))
                {
                    MessageBox.Show("审核成功！", "软件提示");
                }
                else
                {
                    MessageBox.Show("审核失败！", "软件提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "软件提示");
                throw ex;
            }

            this.BindDataGridView("");
        }

        private void toolUnCheck_Click(object sender, EventArgs e)
        {
            List<string> strSqls = new List<string>();
            SqlDataReader sdr = null;
            string strCode = null;

            decimal decMoney;
            string strSEGatherCode = null; //单据编号
            string strIsFlag = null; //审核标记
            string strAccountCode = null; //帐户代码

            string strSEGatherSql = null; //表示提交SEGather表的SQL语句
            string strAccountSql = null; //表示提交BSAccount表的SQL语句

            if (dgvSEGatherInfo.RowCount == 0)
            {
                return;
            }

            strSEGatherCode = this.dgvSEGatherInfo["SEGatherCode", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            strAccountCode = this.dgvSEGatherInfo["AccountCode", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            strIsFlag = this.dgvSEGatherInfo["IsFlag", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value.ToString();
            decMoney = Convert.ToDecimal(this.dgvSEGatherInfo["SEMoney", this.dgvSEGatherInfo.CurrentCell.RowIndex].Value);

            if (strIsFlag == "0")
            {
                MessageBox.Show("该单据未审核，无需弃审！", "软件提示");
                return;
            }

            strCode = "Select AccMoney From BSAccount Where AccountCode = '" + strAccountCode + "'";

            try
            {
                sdr = db.GetDataReader(strCode);
                sdr.Read();

                if (sdr.HasRows)
                {
                    if (sdr.GetDecimal(0) < decMoney)
                    {
                        MessageBox.Show("该笔销售款已经发生了相关业务，无法弃审！", "软件提示");
                        sdr.Close();
                        return;
                    }
                    else
                    {
                        sdr.Close();
                    }  
                }
                else
                {
                    MessageBox.Show("帐户数据异常，无法处理！", "软件提示");
                    sdr.Close();
                    return;
                }

                strAccountSql = "UPDATE BSAccount SET AccMoney = Accmoney - " + decMoney + " WHERE AccountCode = '" + strAccountCode + "'";
                strSEGatherSql = "UPDATE SEGather SET IsFlag = '0' WHERE SEGatherCode = '" + strSEGatherCode + "'";

                strSqls.Add(strAccountSql);
                strSqls.Add(strSEGatherSql);

                if (db.ExecDataBySqls(strSqls))
                {
                    MessageBox.Show("弃审成功！", "软件提示");
                }
                else
                {
                    MessageBox.Show("弃审失败！", "软件提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "软件提示");
                throw ex;
            }

            this.BindDataGridView("");
        }

        private void txtOK_Click(object sender, EventArgs e)
        {
            string strWhere = String.Empty;
            string strConditonName = String.Empty;

            strConditonName = this.cbxCondition.Items[this.cbxCondition.SelectedIndex].ToString();
            switch (strConditonName)
            {
                case "单据编号":

                    strWhere = " WHERE SEGatherCode LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                case "单据日期":

                    strWhere = " WHERE SUBSTRING(CONVERT(VARCHAR(20),SEGatherDate,20),1,10) LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                default:
                    break;
            }
        }

        private void dgvSEGatherInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
