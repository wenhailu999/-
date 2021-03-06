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

namespace ERP.SE
{
    public partial class FormSEOutStore : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormSEOutStore()
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
            //this.txtSEOrderCode.ReadOnly = !this.txtSEOrderCode.ReadOnly;
            this.cbxCustomerCode.Enabled = !this.cbxCustomerCode.Enabled;
            this.cbxStoreCode.Enabled = !this.cbxStoreCode.Enabled;
            this.cbxInvenCode.Enabled = !this.cbxInvenCode.Enabled;
            this.txtSellPrice.ReadOnly = !this.txtSellPrice.ReadOnly;
            this.txtQuantity.ReadOnly = !this.txtQuantity.ReadOnly;
            this.btnChoice.Enabled = !this.btnChoice.Enabled;
            this.cbxEmployeeCode.Enabled = !this.cbxEmployeeCode.Enabled;
        }

        /// <summary>
        /// 将控件恢复到原始状态
        /// </summary>
        private void ClearControls()
        {
            this.txtSEOutCode.Text = "";
            this.dtpSEOutDate.Value = Convert.ToDateTime("1900-01-01");
            this.cbxOperatorCode.SelectedIndex = -1;
            this.txtSEOrderCode.Text = "";
            this.cbxCustomerCode.SelectedIndex = -1;
            this.cbxStoreCode.SelectedIndex = -1;
            this.cbxInvenCode.SelectedIndex = -1;
            this.txtSellPrice.Text = "";
            this.txtQuantity.Text = "";
            this.txtSEMoney.Text = "";
            this.cbxEmployeeCode.SelectedIndex = -1;
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
            this.txtSEOutCode.Text = this.dgvSEOutStoreInfo["SEOutCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            this.dtpSEOutDate.Value = Convert.ToDateTime(this.dgvSEOutStoreInfo["SEOutDate", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value);
            this.cbxOperatorCode.SelectedValue = this.dgvSEOutStoreInfo["OperatorCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
            this.txtSEOrderCode.Text = this.dgvSEOutStoreInfo["SEOrderCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            this.cbxCustomerCode.SelectedValue = this.dgvSEOutStoreInfo["CustomerCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
            this.cbxStoreCode.SelectedValue = this.dgvSEOutStoreInfo["StoreCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
            this.cbxInvenCode.SelectedValue = this.dgvSEOutStoreInfo["InvenCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
            this.txtSellPrice.Text = this.dgvSEOutStoreInfo["SellPrice", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtQuantity.Text = this.dgvSEOutStoreInfo["Quantity", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtSEMoney.Text = this.dgvSEOutStoreInfo["SEMoney", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            this.cbxEmployeeCode.SelectedValue = this.dgvSEOutStoreInfo["EmployeeCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
            this.cbxIsFlag.SelectedValue = this.dgvSEOutStoreInfo["IsFlag", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value;
        }

        /// <summary>
        /// DataGridView控件绑定到数据源
        /// </summary>
        /// <param name="strWhere">Where条件子句</param>
        private void BindDataGridView(string strWhere)
        {
            string strSql = null;

            strSql = "SELECT * ";
            strSql += "FROM SEOutStore " + strWhere; ;

            try
            {
                this.dgvSEOutStoreInfo.DataSource = db.GetDataSet(strSql, "SEOutStore").Tables["SEOutStore"];
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
            db.Cmd.Parameters.AddWithValue("@SEOutCode", txtSEOutCode.Text.Trim());
            db.Cmd.Parameters.AddWithValue("@SEOutDate", dtpSEOutDate.Value);

            if (cbxOperatorCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@OperatorCode", DBNull.Value);
            }
            else
            {
                //把null对象化为DBNull
                db.Cmd.Parameters.AddWithValue("@OperatorCode", cbxOperatorCode.SelectedValue.ToString());
            }

            db.Cmd.Parameters.AddWithValue("@SEOrderCode", txtSEOrderCode.Text.Trim());

            if (cbxCustomerCode.SelectedValue == null)
            {
                //把null对象化为DBNull
                db.Cmd.Parameters.AddWithValue("@CustomerCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@CustomerCode", cbxCustomerCode.SelectedValue.ToString());
            }

            if (cbxStoreCode.SelectedValue == null)
            {
                //把null对象化为DBNull
                db.Cmd.Parameters.AddWithValue("@StoreCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@StoreCode", cbxStoreCode.SelectedValue.ToString());
            }

            if (cbxInvenCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@InvenCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@InvenCode", cbxInvenCode.SelectedValue.ToString());
            }

            if (String.IsNullOrEmpty(txtSellPrice.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@SellPrice", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@SellPrice", Convert.ToDecimal(txtSellPrice.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@Quantity", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtSEMoney.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@SEMoney", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@SEMoney", Convert.ToDecimal(txtSEMoney.Text.Trim()));
            }

            if (cbxEmployeeCode.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@EmployeeCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@EmployeeCode", cbxEmployeeCode.SelectedValue.ToString());
            }

            if (cbxIsFlag.SelectedValue == null)
            {
                db.Cmd.Parameters.AddWithValue("@IsFlag", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@IsFlag", cbxIsFlag.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// 计算销售金额
        /// </summary>
        private void ComputeMoney()
        {
            int int_Quantity;
            decimal dec_SellPrice;

            if (!String.IsNullOrEmpty(txtQuantity.Text.Trim()) && !String.IsNullOrEmpty(txtSellPrice.Text.Trim()))
            {
                int_Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
                dec_SellPrice = Convert.ToDecimal(txtSellPrice.Text.Trim());
                txtSEMoney.Text = Decimal.Round(int_Quantity * dec_SellPrice, 2).ToString();
            }
        }

        private void FormSEOutStore_Load(object sender, EventArgs e)
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
            commUse.BindComboBox(cbxStoreCode, "StoreCode", "StoreName", "select StoreCode,StoreName from BSStore", "BSStore");
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            commUse.BindComboBox(cbxEmployeeCode, "EmployeeCode", "EmployeeName", "select EmployeeCode,EmployeeName from BSEmployee", "BSEmployee");
            commUse.BindComboBox(cbxIsFlag, "Code", "Name", "select * from INCheckFlag", "INCheckFlag");

            //DataGridViewComboBoxColumn绑定到数据源
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["OperatorCode"], "OperatorCode", "OperatorName", "select OperatorCode,OperatorName from SYOperator", "SYOperator");
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["CustomerCode"], "CustomerCode", "CustomerName", "select CustomerCode,CustomerName from BSCustomer", "BSCustomer");
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["StoreCode"], "StoreCode", "StoreName", "select StoreCode,StoreName from BSStore", "BSStore");
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["InvenCode"], "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["EmployeeCode"], "EmployeeCode", "EmployeeName", "select EmployeeCode,EmployeeName from BSEmployee", "BSEmployee");
            commUse.BindComboBox(this.dgvSEOutStoreInfo.Columns["IsFlag"], "Code", "Name", "select * from INCheckFlag", "INCheckFlag");

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

            dtpSEOutDate.Value = DateTime.Today;
            cbxOperatorCode.SelectedValue = PropertyClass.OperatorCode;
            txtQuantity.Text = "1";
            cbxIsFlag.SelectedValue = "0";

            txtSEOutCode.Text = commUse.BuildBillCode("SEOutStore", "SEOutCode", "SEOutDate", dtpSEOutDate.Value);
        }

        private void toolAmend_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "EDIT"; //修改标识

        }

        private void dgvSEOutStoreInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (toolStrip1.Tag.ToString() == "EDIT" && dgvSEOutStoreInfo.RowCount > 0)
            {
                if (this.dgvSEOutStoreInfo["IsFlag", this.dgvSEOutStoreInfo.CurrentRow.Index].Value.ToString() == "1")
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

        private void txtSellPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputNumeric(e, sender as Control);
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputInteger(e);
        }

        private void txtSellPrice_TextChanged(object sender, EventArgs e)
        {
            ComputeMoney();
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            FormBrowseSEOrder formBrowseSEOrder = new FormBrowseSEOrder();
            formBrowseSEOrder.Owner = this;
            formBrowseSEOrder.ShowDialog();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            string strCode = null;

            if (String.IsNullOrEmpty(txtSEOutCode.Text.Trim()))
            {
                MessageBox.Show("单据编号不许为空！", "软件提示");
                txtSEOutCode.Focus();
                return;
            }

            if (cbxCustomerCode.SelectedValue == null)
            {
                MessageBox.Show("客户不许为空！", "软件提示");
                cbxCustomerCode.Focus();
                return;
            }

            if (cbxStoreCode.SelectedValue == null)
            {
                MessageBox.Show("仓库不许为空！", "软件提示");
                cbxStoreCode.Focus();
                return;
            }

            if (cbxInvenCode.SelectedValue == null)
            {
                MessageBox.Show("存货不许为空！", "软件提示");
                cbxInvenCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtSellPrice.Text.Trim()))
            {
                MessageBox.Show("单价不许为空！", "软件提示");
                txtSellPrice.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                MessageBox.Show("数量不许为空！", "软件提示");
                txtQuantity.Focus();
                return;
            }
            else if (Convert.ToInt32(txtQuantity.Text.Trim()) == 0)
            {
                MessageBox.Show("数量不能等于零", "软件提示");
                txtQuantity.Focus();
                return;
            }
            //添加
            if (toolStrip1.Tag.ToString() == "ADD")
            {
                if (Convert.ToInt32(txtQuantity.Text.Trim()) > Convert.ToInt32(db.GetDataSet("select Quantity from SEOrder where SEOrderCode='" + txtSEOrderCode.Text.Trim() + "'", "SEOrder").Tables[0].Rows[0][0]))
                {
                    MessageBox.Show("销售出库数量不能大于销售订单数量", "软件提示");
                    txtQuantity.Focus();
                    return;
                }
                try
                {
                    strCode = "INSERT INTO SEOutStore(SEOutCode,SEOutDate,OperatorCode,SEOrderCode,CustomerCode,StoreCode,InvenCode,SellPrice,Quantity,SEMoney,EmployeeCode,IsFlag) ";
                    strCode += "VALUES(@SEOutCode,@SEOutDate,@OperatorCode,@SEOrderCode,@CustomerCode,@StoreCode,@InvenCode,@SellPrice,@Quantity,@SEMoney,@EmployeeCode,@IsFlag)";

                    ParametersAddValue();

                    if (db.ExecDataBySql(strCode) > 0)
                    {
                        MessageBox.Show("保存成功！", "软件提示");
                        db.ExecDataBySql("update SEOrder set Quantity=(Quantity-" + Convert.ToInt32(txtQuantity.Text.Trim()) + ") where SEOrderCode='" + txtSEOrderCode.Text.Trim() + "'");//更新销售单
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
                if (Convert.ToInt32(txtQuantity.Text.Trim()) > Convert.ToInt32(db.GetDataSet("select Quantity from SEOutStore where SEOrderCode='" + txtSEOrderCode.Text.Trim() + "'", "SEOutStore").Tables[0].Rows[0][0]))
                {
                    MessageBox.Show("销售出库数量不能大于销售订单数量", "软件提示");
                    txtQuantity.Focus();
                    return;
                }
                string strSEOutCode = txtSEOutCode.Text.Trim();
                //更新数据库
                try
                {
                    strCode = "UPDATE SEOutStore SET SEOutCode = @SEOutCode,SEOutDate = @SEOutDate,OperatorCode = @OperatorCode,SEOrderCode = @SEOrderCode,CustomerCode = @CustomerCode,StoreCode = @StoreCode,";
                    strCode += "InvenCode = @InvenCode,SellPrice = @SellPrice,Quantity = @Quantity,";
                    strCode += "SEMoney = @SEMoney,EmployeeCode = @EmployeeCode,IsFlag = @IsFlag";
                    strCode += " WHERE SEOutCode = '" + strSEOutCode + "'";

                    ParametersAddValue();

                    if (db.ExecDataBySql(strCode) > 0)
                    {
                        MessageBox.Show("保存成功！", "软件提示");
                        db.ExecDataBySql("update SEOrder set Quantity=(Quantity-" + Convert.ToInt32(txtQuantity.Text.Trim()) + ") where SEOrderCode='" + txtSEOrderCode.Text.Trim() + "'");//更新销售单
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
            string strSEOutCode = null; //单据编号
            string strSql = null;
            string strFlag = null; //审核标记

            if (this.dgvSEOutStoreInfo.RowCount <= 0)
            {
                return;
            }

            strSEOutCode = this.dgvSEOutStoreInfo["SEOutCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strFlag = this.dgvSEOutStoreInfo["IsFlag", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();

            if (strFlag == "1")
            {
                MessageBox.Show("该单据已审核，不许删除！", "软件提示");
                return;
            }

            strSql = "DELETE FROM SEOutStore WHERE SEOutCode = '" + strSEOutCode + "'";

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
            SqlDataReader sdr = null;
            string strCode = null;
            string strSEOutCode = null; //单据编号
            string strIsFlag = null; //审核标记

            string strSEOutStoreSql = null; //表示提交SEOutStore表的SQL语句
            string strSTStockSql = null; //表示提交STStock表的SQL语句

            string strStoreCode = null; //仓库代码
            string strInvenCode = null; //产品代码

            int intQuantity;
            decimal decAvePrice;
            decimal decMoney;

            if (dgvSEOutStoreInfo.RowCount == 0)
            {
                return;
            }

            strStoreCode = this.dgvSEOutStoreInfo["StoreCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strInvenCode = this.dgvSEOutStoreInfo["InvenCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strSEOutCode = this.dgvSEOutStoreInfo["SEOutCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strIsFlag = this.dgvSEOutStoreInfo["IsFlag", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();

            intQuantity = Convert.ToInt32(this.dgvSEOutStoreInfo["Quantity", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value);

            if (strIsFlag == "1")
            {
                MessageBox.Show("该单据已审核过，不许再次审核！", "软件提示");
                return;
            }

            strCode = "SELECT Quantity,AvePrice,STMoney FROM STStock WHERE StoreCode = '" + strStoreCode + "' AND InvenCode = '" + strInvenCode + "'";

            try
            {
                sdr = db.GetDataReader(strCode);
                sdr.Read();

                if (!sdr.HasRows)
                {
                    MessageBox.Show("该存货无库存，无法审核！", "软件提示");
                    sdr.Close();
                    return;
                }

                if (sdr.GetInt32(0) < intQuantity)
                {
                    MessageBox.Show("库存不足，无法审核！","软件提示");
                    sdr.Close();
                    return;
                }
                else
                {
                    intQuantity = sdr.GetInt32(0) - intQuantity; //库存剩余数量
                }

                decAvePrice = sdr.GetDecimal(1);

                decMoney = Decimal.Round(decAvePrice * intQuantity,2);

                strSTStockSql = "UPDATE STStock SET Quantity = " + intQuantity + ",STMoney = "+decMoney+" ";
                strSTStockSql += "WHERE StoreCode = '" + strStoreCode + "' AND InvenCode = '" + strInvenCode + "'";

                strSEOutStoreSql = "UPDATE SEOutStore SET IsFlag = '1',UnitPrice = " + decAvePrice + " WHERE SEOutCode = '" + strSEOutCode + "'";

                sdr.Close();
                
                strSqls.Add(strSTStockSql);
                strSqls.Add(strSEOutStoreSql);

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
                sdr.Close();
                throw ex;
            }

            this.BindDataGridView("");
        }

        private void toolUnCheck_Click(object sender, EventArgs e)
        {
            List<string> strSqls = new List<string>();
            SqlDataReader sdr = null;
            string strCode = null;
            string strSEOutCode = null; //单据编号
            string strIsFlag = null; //审核标记

            string strSEOutStoreSql = null; //表示提交FISelCost表的SQL语句
            string strSTStockSql = null; //表示提交FISelCost表的SQL语句

            string strStoreCode = null; //仓库代码
            string strInvenCode = null; //产品代码

            int intQuantity;
            decimal decUnitPrice;

            decimal decMoney; //库存的总金额
            decimal decAvePrice; //库存的平均单价

            if (dgvSEOutStoreInfo.RowCount == 0)
            {
                return;
            }

            strIsFlag = this.dgvSEOutStoreInfo["IsFlag", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();

            if (strIsFlag == "0")
            {
                MessageBox.Show("该单据未审核，无需弃审！", "软件提示");
                return;
            }

            strStoreCode = this.dgvSEOutStoreInfo["StoreCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strInvenCode = this.dgvSEOutStoreInfo["InvenCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            strSEOutCode = this.dgvSEOutStoreInfo["SEOutCode", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value.ToString();
            
            intQuantity = Convert.ToInt32(this.dgvSEOutStoreInfo["Quantity", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value); //出库数量
            decUnitPrice = Convert.ToDecimal(this.dgvSEOutStoreInfo["UnitPrice", this.dgvSEOutStoreInfo.CurrentCell.RowIndex].Value); //出库时的即时成本价(是单价)
            
            strCode = "select * from SEGather where  SEOutCode = '" + strSEOutCode + "'";

            try
            {
                sdr = db.GetDataReader(strCode);
                sdr.Read();

                if (sdr.HasRows)
                {
                    MessageBox.Show("该单据已发生业务关系，无法弃审！", "软件提示");
                    sdr.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "软件提示");
                throw ex;
            }
            finally
            {
                sdr.Close();
            }

            strCode = "SELECT Quantity,AvePrice,STMoney FROM STStock WHERE StoreCode = '" + strStoreCode + "' AND InvenCode = '" + strInvenCode + "'";

            try
            {
                sdr = db.GetDataReader(strCode);
                sdr.Read();

                if (sdr.HasRows)
                {                
                    decMoney = sdr.GetDecimal(2) + decimal.Round(intQuantity * decUnitPrice, 2); //合计金额
                    intQuantity = sdr.GetInt32(0) + intQuantity; //总计数量
                    decAvePrice = decimal.Round(decMoney / intQuantity,2); //计算平均单价

                    strSTStockSql = "UPDATE STStock SET Quantity = " + intQuantity + ",STMoney = " + decMoney + ",AvePrice = " + decAvePrice+" ";
                    strSTStockSql += "WHERE StoreCode = '" + strStoreCode + "' AND InvenCode = '" + strInvenCode + "'";
                }
                else
                {
                    MessageBox.Show("库存数据异常，无法处理！", "软件提示");
                    sdr.Close();
                    return;
                }

                strSEOutStoreSql = "UPDATE SEOutStore SET IsFlag = '0',UnitPrice = null WHERE SEOutCode = '" + strSEOutCode + "'";

                sdr.Close();
                strSqls.Add(strSTStockSql);
                strSqls.Add(strSEOutStoreSql);

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
                sdr.Close();
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

                    strWhere = " WHERE SEOutCode LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                case "单据日期":

                    strWhere = " WHERE SUBSTRING(CONVERT(VARCHAR(20),SEOutDate,20),1,10) LIKE '%" + txtKeyWord.Text.Trim() + "%'";
                    this.BindDataGridView(strWhere);
                    break;

                default:
                    break;
            }
        }

        private void dgvSEOutStoreInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
