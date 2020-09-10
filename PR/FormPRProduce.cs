﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP.ComClass;
using ERP.DataClass;
using System.Data.SqlClient;

namespace ERP.PR
{
    public partial class FormPRProduce : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormPRProduce()
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
            this.btnChoice.Enabled = !this.btnChoice.Enabled;
            this.cbxDepartmentCode.Enabled = !this.cbxDepartmentCode.Enabled;
            this.txtQuantity.ReadOnly = !this.txtQuantity.ReadOnly;
            this.dtpStartDate.Enabled = !this.dtpStartDate.Enabled;
            this.dtpEndDate.Enabled = !this.dtpEndDate.Enabled;
        }

        /// <summary>
        /// 将控件恢复到原始状态
        /// </summary>
        private void ClearControls()
        {
            this.txtPRProduceCode.Text = "";
            this.dtpPRProduceDate.Value = Convert.ToDateTime("1900-01-01");
            this.cbxOperatorCode.SelectedIndex = -1;
            this.txtPRPlanCode.Text = "";
            this.cbxDepartmentCode.SelectedIndex = -1;
            this.cbxInvenCode.SelectedIndex = -1;
            this.txtQuantity.Text = "";
            this.dtpStartDate.Value = Convert.ToDateTime("1900-01-01");
            this.dtpEndDate.Value = Convert.ToDateTime("1900-01-01");
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
            this.txtPRProduceCode.Text = this.dgvPRProduceInfo["PRProduceCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            this.dtpPRProduceDate.Value = Convert.ToDateTime(this.dgvPRProduceInfo["PRProduceDate", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value);
            this.cbxOperatorCode.SelectedValue = this.dgvPRProduceInfo["OperatorCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value;
            this.txtPRPlanCode.Text = this.dgvPRProduceInfo["PRPlanCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            this.cbxDepartmentCode.SelectedValue = this.dgvPRProduceInfo["DepartmentCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value;
            this.cbxInvenCode.SelectedValue = this.dgvPRProduceInfo["InvenCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value;
            this.txtQuantity.Text = this.dgvPRProduceInfo["Quantity", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            this.dtpStartDate.Value = Convert.ToDateTime(this.dgvPRProduceInfo["StartDate", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value);
            this.dtpEndDate.Value = Convert.ToDateTime(this.dgvPRProduceInfo["EndDate", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value);
            this.cbxIsFlag.SelectedValue = this.dgvPRProduceInfo["IsFlag", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value;
        }

        /// <summary>
        /// DataGridView控件绑定数据源
        /// </summary>
        /// <param name="strWhere">Where条件子句</param>
        private void BindDataGridView(string strWhere)
        {
            string strSql = null;

            strSql = "SELECT * ";
            strSql += "FROM PRProduce " + strWhere; ;

            try
            {
                this.dgvPRProduceInfo.DataSource = db.GetDataSet(strSql, "PRProduce").Tables["PRProduce"];
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
            db.Cmd.Parameters.AddWithValue("@PRProduceCode", txtPRProduceCode.Text.Trim());
            db.Cmd.Parameters.AddWithValue("@PRProduceDate", dtpPRProduceDate.Value);

            if (cbxOperatorCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@OperatorCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@OperatorCode", cbxOperatorCode.SelectedValue.ToString());
            }

            db.Cmd.Parameters.AddWithValue("@PRPlanCode", txtPRPlanCode.Text.Trim());

            if (cbxDepartmentCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@DepartmentCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@DepartmentCode", cbxDepartmentCode.SelectedValue.ToString());
            }

            if (cbxInvenCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@InvenCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@InvenCode", cbxInvenCode.SelectedValue.ToString());
            }

            db.Cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text.Trim()));
            db.Cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
            db.Cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
            db.Cmd.Parameters.AddWithValue("@IsFlag", cbxIsFlag.SelectedValue.ToString());
            db.Cmd.Parameters.AddWithValue("@IsComplete", "0"); //默认未完工
        }

        private void FormPRProduce_Load(object sender, EventArgs e)
        {
            //权限
            commUse.CortrolButtonEnabled(toolAdd, this);
            commUse.CortrolButtonEnabled(toolAmend, this);
            commUse.CortrolButtonEnabled(toolDelete, this);
            commUse.CortrolButtonEnabled(toolCheck, this);
            commUse.CortrolButtonEnabled(toolUnCheck, this);

            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxOperatorCode, "OperatorCode", "OperatorName", "select OperatorCode,OperatorName from SYOperator", "SYOperator");
            commUse.BindComboBox(cbxDepartmentCode, "DepartmentCode", "DepartmentName", "select DepartmentCode,DepartmentName from BSDepartment", "BSDepartment");
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            commUse.BindComboBox(cbxIsFlag, "Code", "Name", "select * from INCheckFlag", "INCheckFlag");

            //DataGridViewComboBoxColumn绑定到数据源
            commUse.BindComboBox(this.dgvPRProduceInfo.Columns["OperatorCode"], "OperatorCode", "OperatorName", "select OperatorCode,OperatorName from SYOperator", "SYOperator");
            commUse.BindComboBox(this.dgvPRProduceInfo.Columns["DepartmentCode"], "DepartmentCode", "DepartmentName", "select DepartmentCode,DepartmentName from BSDepartment", "BSDepartment");
            commUse.BindComboBox(this.dgvPRProduceInfo.Columns["InvenCode"], "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            commUse.BindComboBox(this.dgvPRProduceInfo.Columns["IsFlag"], "Code", "Name", "select * from INCheckFlag", "INCheckFlag");
            //
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

            dtpPRProduceDate.Value = DateTime.Today;
            cbxOperatorCode.SelectedValue = PropertyClass.OperatorCode;
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
            cbxIsFlag.SelectedValue = "0";

            txtPRProduceCode.Text = commUse.BuildBillCode("PRProduce", "PRProduceCode", "PRProduceDate", dtpPRProduceDate.Value);
        }

        private void toolAmend_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "EDIT"; //修改标识

        }

        private void dgvPRProduceInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (toolStrip1.Tag.ToString() == "EDIT" && dgvPRProduceInfo.RowCount > 0)
            {
                if (this.dgvPRProduceInfo["IsFlag", this.dgvPRProduceInfo.CurrentRow.Index].Value.ToString() == "1")
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

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputInteger(e);
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            FormBrowsePRPlan formBrowsePRPlan = new FormBrowsePRPlan();
            formBrowsePRPlan.Owner = this;
            formBrowsePRPlan.ShowDialog();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            string strCode = null;
            string PRProduceSql = null; //表示提交PRProduce表的SQL语句
            string PRProduceItemSql = null; //表示提交PRProduceItem表的SQL语句
            SqlDataReader sdr = null;
            int intProQuantity; //产品的数量
            int intMatQuantity; //所需原材料的数量
            List<string> strSqls = new List<string>();

            if (String.IsNullOrEmpty(txtPRProduceCode.Text.Trim()))
            {
                MessageBox.Show("单据编号不许为空！", "软件提示");
                txtPRProduceCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtPRPlanCode.Text.Trim()))
            {
                MessageBox.Show("主生产计划号不许为空！", "软件提示");
                txtPRPlanCode.Focus();
                return;
            }

            if (cbxDepartmentCode.SelectedValue == null)
            {
                MessageBox.Show("生产车间不许为空！", "软件提示");
                cbxDepartmentCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                MessageBox.Show("生产数量不许为空！", "软件提示");
                txtQuantity.Focus();
                return;
            }
            else
            {
                if (Convert.ToInt32(txtQuantity.Text.Trim()) == 0)
                {
                    MessageBox.Show("生产数量不能等于零", "软件提示");
                    txtQuantity.Focus();
                    return;
                }

                intProQuantity = Convert.ToInt32(txtQuantity.Text.Trim());
            }

            if (dtpEndDate.Value < dtpStartDate.Value)
            {
                MessageBox.Show("开始日期不许大于结束日期！","软件提示");
                dtpStartDate.Focus();
                return;
            }

            //添加
            if (toolStrip1.Tag.ToString() == "ADD")
            {
                try
                {
                    //生产单的主表
                    PRProduceSql = "INSERT INTO PRProduce(PRProduceCode,PRProduceDate,OperatorCode,PRPlanCode,DepartmentCode,InvenCode,Quantity,StartDate,EndDate,IsFlag,IsComplete) ";
                    PRProduceSql += "VALUES(@PRProduceCode,@PRProduceDate,@OperatorCode,@PRPlanCode,@DepartmentCode,@InvenCode,@Quantity,@StartDate,@EndDate,@IsFlag,@IsComplete)";
                    ParametersAddValue();
                    strSqls.Add(PRProduceSql);
                    //生产单的子表
                    strCode = "Select ProInvenCode,MatInvenCode,Quantity From BSBom Where ProInvenCode = '" + cbxInvenCode.SelectedValue + "'";

                    sdr = db.GetDataReader(strCode);

                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            intMatQuantity = intProQuantity * sdr.GetInt32(2);
                            PRProduceItemSql = "INSERT INTO PRProduceItem(PRProduceCode,InvenCode,Quantity) ";
                            PRProduceItemSql += "VALUES('" + txtPRProduceCode.Text + "','" + sdr.GetString(1) + "'," + intMatQuantity + ")";
                            strSqls.Add(PRProduceItemSql);
                        }

                        sdr.Close();
                    }
                    else
                    {
                        MessageBox.Show("该产品的物料清单不存在！，无法保存", "软件提示");
                        sdr.Close();
                        return;
                    }

                    if (db.ExecDataBySqls(strSqls))
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
                string strPRProduceCode = txtPRProduceCode.Text.Trim();
                //更新数据库
                try
                {
                    //生产单的主表
                    strCode = "UPDATE PRProduce SET PRProduceCode = @PRProduceCode,PRProduceDate = @PRProduceDate,OperatorCode = @OperatorCode, PRPlanCode = @PRPlanCode,DepartmentCode = @DepartmentCode,";
                    strCode += "InvenCode = @InvenCode,Quantity = @Quantity,StartDate = @StartDate,";
                    strCode += "EndDate = @EndDate ";
                    strCode += "WHERE PRProduceCode = '" + strPRProduceCode + "'";
                    ParametersAddValue();
                    strSqls.Add(strCode);
                    //首先删除生产单子表中的旧原料配置
                    strPRProduceCode = "Delete From PRProduceItem Where PRProduceCode = '" + strPRProduceCode + "'";
                    strSqls.Add(strPRProduceCode);
                    //在生产单子表中插入新的原料配置
                    strCode = "Select ProInvenCode,MatInvenCode,Quantity From BSBom Where ProInvenCode = '" + cbxInvenCode.SelectedValue + "'";
                    sdr = db.GetDataReader(strCode);

                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            intMatQuantity = intProQuantity * sdr.GetInt32(2);
                            PRProduceItemSql = "INSERT INTO PRProduceItem(PRProduceCode,InvenCode,Quantity) ";
                            PRProduceItemSql += "VALUES('" + txtPRProduceCode.Text + "','" + sdr.GetString(1) + "','" + intMatQuantity + "')";
                            strSqls.Add(PRProduceItemSql);
                        }

                        sdr.Close();
                    }
                    else
                    {
                        MessageBox.Show("该产品的物料清单不存在！，无法保存", "软件提示");
                        sdr.Close();
                        return;
                    }

                    if (db.ExecDataBySqls(strSqls))
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

        private void toolCheck_Click(object sender, EventArgs e)
        {
            string strPRProduceCode = null; //生产单编号
            string strSql = null; 
            string strFlag = null; //审核标记

            if (dgvPRProduceInfo.RowCount == 0)
            {
                return;
            }

            strPRProduceCode = this.dgvPRProduceInfo["PRProduceCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            strFlag = this.dgvPRProduceInfo["IsFlag", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();

            if (strFlag == "1")
            {
                MessageBox.Show("该单据已审核过，不许再次审核！", "软件提示");
                return;
            }

            strSql = "UPDATE PRProduce SET IsFlag = '1' WHERE PRProduceCode = '" + strPRProduceCode + "'";

            try
            {
                if (db.ExecDataBySql(strSql) > 0)
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
            string strPRProduceCode = null; //生产单编号
            string strSql = null;
            string strFlag = null; //审核标记
            string strIsComplete = null; //生产完工标记
            Nullable<Int32> intGetQuantity = null; //领料数量(可空类型)

            if (dgvPRProduceInfo.RowCount == 0)
            {
                return;
            }

            strPRProduceCode = this.dgvPRProduceInfo["PRProduceCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            strFlag = this.dgvPRProduceInfo["IsFlag", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            strIsComplete = this.dgvPRProduceInfo["IsComplete", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();

            if (strFlag == "0")
            {
                MessageBox.Show("该单据未审核过，无需弃审！", "软件提示");
                return;
            }

            if (strIsComplete == "1")
            {
                MessageBox.Show("该单据已完工，无法弃审！", "软件提示");
                return;
            }

            strSql = "Select Sum(GetQuantity) From PRProduceItem Where PRProduceCode = '" + strPRProduceCode + "'";
            
            try
            {
                intGetQuantity = db.GetSingleObject(strSql) as Nullable<Int32>;

                //判断该生产单是否已经领料
                if (intGetQuantity.HasValue && intGetQuantity.Value != 0)
                {
                    MessageBox.Show("该生产单已经领料，不许弃审！","软件提示");
                    return;
                }

                strSql = "UPDATE PRProduce SET IsFlag = '0' WHERE PRProduceCode = '" + strPRProduceCode + "'";

                if (db.ExecDataBySql(strSql) > 0)
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

        private void toolDelete_Click(object sender, EventArgs e)
        {
            string strPRProduceCode = null; //生产单号
            string strSql = null;
            string strFlag = null; //审核标记
            List<string> strSqls = new List<string>();

            if (this.dgvPRProduceInfo.RowCount <= 0)
            {
                return;
            }

            strPRProduceCode = this.dgvPRProduceInfo["PRProduceCode", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();
            strFlag = this.dgvPRProduceInfo["IsFlag", this.dgvPRProduceInfo.CurrentCell.RowIndex].Value.ToString();

            if (strFlag == "1")
            {
                MessageBox.Show("该单据已审核，不许删除！", "软件提示");
                return;
            }

            //删除生产单子表
            strSql = "DELETE FROM PRProduceItem WHERE PRProduceCode = '" + strPRProduceCode + "'";
            strSqls.Add(strSql);
            //删除生产单主表
            strSql = "DELETE FROM PRProduce WHERE PRProduceCode = '" + strPRProduceCode + "'";
            strSqls.Add(strSql);

            if (MessageBox.Show("确定要删除吗？", "软件提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                try
                {
                    if (db.ExecDataBySqls(strSqls))
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

        private void txtOK_Click(object sender, EventArgs e)
        {
            string strWhere = String.Empty;
            string strConditonName = String.Empty;

            strConditonName = this.cbxCondition.Items[this.cbxCondition.SelectedIndex].ToString();
            switch (strConditonName)
            {
                case "单据编号":

                    strWhere = " WHERE PRProduceCode LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                case "单据日期":

                    strWhere = " WHERE SUBSTRING(CONVERT(VARCHAR(20),PRProduceDate,20),1,10) LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                default:
                    break;
            }
        }

        private void dgvPRProduceInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
