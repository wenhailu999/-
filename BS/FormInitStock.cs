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
using System.Data.SqlClient;

namespace ERP.BS
{
    public partial class FormInitStock : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();

        public FormInitStock()
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
            //窗体控件状态切换
            this.cbxStoreCode.Enabled = !this.cbxStoreCode.Enabled;
            this.cbxInvenCode.Enabled = !this.cbxInvenCode.Enabled;
            this.txtQuantity.ReadOnly = !this.txtQuantity.ReadOnly;
            this.txtLossQuantity.ReadOnly = !this.txtLossMoney.ReadOnly;
            this.txtAvePrice.ReadOnly = !this.txtAvePrice.ReadOnly;
            this.txtLossMoney.ReadOnly = !this.txtLossMoney.ReadOnly;
        }

        /// <summary>
        /// 将控件恢复到原始状态
        /// </summary>
        private void ClearControls()
        {
            //窗体控件状态切换
            this.cbxStoreCode.SelectedIndex = -1;
            this.cbxInvenCode.SelectedIndex = -1;
            this.txtQuantity.Text = "";
            this.txtLossQuantity.Text = "";
            this.txtAvePrice.Text = "";
            this.txtSTMoney.Text = "";
            this.txtLossMoney.Text = "";
        }

        /// <summary>
        /// 计算库存金额
        /// </summary>
        private void ComputeMoney()
        {
            int int_Quantity;
            decimal dec_AvePrice;

            if (!String.IsNullOrEmpty(txtQuantity.Text.Trim()) && !String.IsNullOrEmpty(txtAvePrice.Text.Trim()))
            {
                int_Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
                dec_AvePrice = Convert.ToDecimal(txtAvePrice.Text.Trim());
                txtSTMoney.Text = Decimal.Round(int_Quantity * dec_AvePrice, 2).ToString();
            }
        }

        /// <summary>
        /// 设置控件的显示值
        /// </summary>
        private void FillControls()
        {
            this.cbxStoreCode.SelectedValue = this.dgvStockInfo[0, this.dgvStockInfo.CurrentCell.RowIndex].Value;
            this.cbxInvenCode.SelectedValue = this.dgvStockInfo[1, this.dgvStockInfo.CurrentCell.RowIndex].Value;
            this.txtQuantity.Text = this.dgvStockInfo[2, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtLossQuantity.Text = this.dgvStockInfo[3, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtAvePrice.Text = this.dgvStockInfo[4, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtSTMoney.Text = this.dgvStockInfo[5, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            this.txtLossMoney.Text = this.dgvStockInfo[6, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
        }

        /// <summary>
        /// DataGridView控件绑定到数据源
        /// </summary>
        /// <param name="strWhere">Where条件子句</param>
        private void BindDataGridView(string strWhere)
        {
            string strSql = null;

            strSql = "SELECT * ";
            strSql += "FROM STStock" + strWhere; ;

            try
            {
                this.dgvStockInfo.DataSource = db.GetDataSet(strSql, "STStock").Tables["STStock"];
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
                //把null对象化为DBNull
                db.Cmd.Parameters.AddWithValue("@InvenCode", DBNull.Value);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@InvenCode", cbxInvenCode.SelectedValue.ToString());
            }

            if (String.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@Quantity", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtLossQuantity.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@LossQuantity", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@LossQuantity", Convert.ToInt32(txtLossQuantity.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtAvePrice.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@AvePrice", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@AvePrice", Convert.ToDecimal(txtAvePrice.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtSTMoney.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@STMoney", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@STMoney", Convert.ToDecimal(txtSTMoney.Text.Trim()));
            }

            if (String.IsNullOrEmpty(txtLossMoney.Text.Trim()))
            {
                db.Cmd.Parameters.AddWithValue("@LossMoney", 0);
            }
            else
            {
                db.Cmd.Parameters.AddWithValue("@LossMoney", Convert.ToDecimal(txtLossMoney.Text.Trim()));
            }
        }

        private void FormInitStock_Load(object sender, EventArgs e)
        {
            //权限
            commUse.CortrolButtonEnabled(toolAdd, this);
            commUse.CortrolButtonEnabled(toolAmend, this);
            commUse.CortrolButtonEnabled(toolDelete, this);
            //ComboBox绑定到数据源
            commUse.BindComboBox(cbxStoreCode, "StoreCode", "StoreName", "select StoreCode,StoreName from BSStore", "BSStore");
            commUse.BindComboBox(cbxInvenCode, "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            //DataGridViewComboBoxColumn绑定到数据源
            commUse.BindComboBox(this.dgvStockInfo.Columns[0], "StoreCode", "StoreName", "select StoreCode,StoreName from BSStore", "BSStore");
            commUse.BindComboBox(this.dgvStockInfo.Columns[1], "InvenCode", "InvenName", "select InvenCode,InvenName from BSInven", "BSInven");
            BindDataGridView("");
            toolStrip1.Tag = "";
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "ADD"; //添加操作
        }

        private void toolAmend_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "EDIT"; //修改操作
        }

        private void toolreflush_Click(object sender, EventArgs e)
        {
            this.BindDataGridView("");
        }

        private void dgvStoreInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (toolStrip1.Tag.ToString() == "EDIT")
            {
                if (dgvStockInfo.RowCount > 0)
                {
                    FillControls();
                }
            }
        }

        private void toolCancel_Click(object sender, EventArgs e)
        {
            ControlStatus();
            ClearControls();
            toolStrip1.Tag = "";
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            this.ComputeMoney();
        }

        private void txtLossQuantity_TextChanged(object sender, EventArgs e)
        {
            this.ComputeMoney();
        }

        private void txtAvePrice_TextChanged(object sender, EventArgs e)
        {
            this.ComputeMoney();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            string strCode = null;
            SqlDataReader sdr = null;
            string strStoreCode = null; //仓库代码
            string strInvenCode = null; //存货代码

            if (cbxStoreCode.SelectedIndex == -1)
            {
                MessageBox.Show("请选择仓库！", "软件提示");
                cbxStoreCode.Focus();
                return;
            }

            if (cbxInvenCode.SelectedIndex == -1)
            {
                MessageBox.Show("请选择存货！", "软件提示");
                cbxInvenCode.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                MessageBox.Show("库存数量不许为空！", "软件提示"); 
                txtQuantity.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtAvePrice.Text.Trim()))
            {
                MessageBox.Show("成本价不许为空！", "软件提示"); 
                txtQuantity.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtLossQuantity.Text.Trim()))
            {
                MessageBox.Show("损失数量不许为空！", "软件提示"); 
                txtLossQuantity.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtLossMoney.Text.Trim()))
            {
                MessageBox.Show("损失金额不许为空！", "软件提示"); 
                txtLossMoney.Focus();
                return;
            }

            //添加操作
            if (toolStrip1.Tag.ToString() == "ADD")
            {
                strStoreCode = cbxStoreCode.SelectedValue.ToString(); //得到仓库代码
                strInvenCode = cbxInvenCode.SelectedValue.ToString(); //得到存货代码

                strCode = "select * from STStock where StoreCode = '" + strStoreCode + "' and InvenCode = '" + strInvenCode + "'";

                try
                {
                    sdr = db.GetDataReader(strCode);
                    sdr.Read();

                    if (!sdr.HasRows)
                    {
                        sdr.Close();

                        strCode = "INSERT INTO STStock(StoreCode,InvenCode,Quantity,LossQuantity,AvePrice,STMoney,LossMoney) ";
                        strCode += "VALUES(@StoreCode,@InvenCode,@Quantity,@LossQuantity,@AvePrice,@STMoney,@LossMoney)";

                        ParametersAddValue();

                        if (db.ExecDataBySql(strCode) > 0)
                        {
                            MessageBox.Show("保存成功！", "软件提示");
                            toolStrip1.Tag = "";
                            this.BindDataGridView("");
                            ControlStatus();
                        }
                        else
                        {
                            MessageBox.Show("保存失败！", "软件提示");
                        }
                    }
                    else
                    {
                        MessageBox.Show("该存货的库存已经被初始化过！", "软件提示");
                        this.cbxInvenCode.Focus();
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
            }

            //修改
            if (toolStrip1.Tag.ToString() == "EDIT")
            {
                string strOldStoreCode = null; //未修改之前的仓库代码
                string strOldInvenCode = null; //未修改之前的存货代码

                strOldStoreCode = this.dgvStockInfo[0, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
                strOldInvenCode = this.dgvStockInfo[1, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();

                strStoreCode = cbxStoreCode.SelectedValue.ToString(); //得到仓库代码
                strInvenCode = cbxInvenCode.SelectedValue.ToString(); //得到存货代码

                if (strOldStoreCode != cbxStoreCode.SelectedValue.ToString() || strOldInvenCode != cbxInvenCode.SelectedValue.ToString())
                {
                    strCode = "select * from STStock where StoreCode = '" + strStoreCode + "' and InvenCode = '" + strInvenCode + "'";

                    try
                    {
                        sdr = db.GetDataReader(strCode);
                        sdr.Read();
                        if (sdr.HasRows)
                        {
                            MessageBox.Show("该存货的库存已经被初始化过！", "软件提示");
                            this.cbxInvenCode.Focus();
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
                }

                //更新数据库
                try
                {
                    strCode = "UPDATE STStock SET StoreCode = @StoreCode,InvenCode = @InvenCode,";
                    strCode += "Quantity = @Quantity,LossQuantity = @LossQuantity,AvePrice = @AvePrice,";
                    strCode += "STMoney = @STMoney,LossMoney = @LossMoney ";
                    strCode += "WHERE StoreCode = '" + strOldStoreCode + "' AND InvenCode = '" + strOldInvenCode + "'";

                    ParametersAddValue();

                    if (db.ExecDataBySql(strCode) > 0)
                    {
                        MessageBox.Show("保存成功！", "软件提示");
                        toolStrip1.Tag = "";
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
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            string strStoreCode = null; //仓库代码
            string strInvenCode = null; //存货代码
            string strSql = null;

            if (this.dgvStockInfo.RowCount == 0)
            {
                return;
            }

            strStoreCode = this.dgvStockInfo[0, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            strInvenCode = this.dgvStockInfo[1, this.dgvStockInfo.CurrentCell.RowIndex].Value.ToString();
            strSql = "DELETE FROM STStock WHERE StoreCode = '" + strStoreCode + "' AND InvenCode = '"+strInvenCode+"'";

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

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputInteger(e);
        }

        private void txtAvePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            commUse.InputNumeric(e, sender as Control);
        }

    }
}
