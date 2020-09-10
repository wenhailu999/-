using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel;//引入IComponent接口
using CrystalDecisions.Shared;//引入TableLogOnInfo类
using CrystalDecisions.CrystalReports.Engine;//引入ReportDocument类
using ERP.BS;							//引入基础管理模块的窗体类
using ERP.PU;							//引入采购管理模块的窗体类
using ERP.SE;							//引入销售管理模块的窗体类
using ERP.ST;							//引入仓库管理模块的窗体类
using ERP.PR;							//引入生产管理模块的窗体类
using ERP.CU;							//引入客户管理模块的窗体类
using ERP.FI;							//引入财务管理模块的窗体类
using ERP.SY;							//引入系统管理模块的窗体类
using ERP.RP.FORM;					//引入报表统计模块的窗体类
using ERP.DataClass;					//引入DataBase类的命名空间


namespace ERP.ComClass
{
    /// <summary>
    /// 公共的通用类，提供一些通用的方法
    /// </summary>
    public class CommonUse
    {
        DataBase db = new DataBase();

        public CommonUse()
        {
            
        }

        /// <summary>
        /// TreeView控件绑定到数据源
        /// </summary>
        /// <param name="tv">TreeView控件</param>
        /// <param name="imgList">ImageList控件</param>
        /// <param name="rootName">根节点的文本属性值</param>
        /// <param name="strTable">要绑定的数据表</param>
        /// <param name="strCode">数据表的代码列</param>
        /// <param name="strName">数据表的名称列</param>
        public void BuildTree(TreeView tv,ImageList imgList,string rootName, string strTable, string strCode, string strName)
        {
            string strSql = null;                                       //声明表示SQL语句的字符串
            DataSet ds = null;                                      //声明DataSet引用
            DataTable dt = null;                                    //声明DataTable引用
            TreeNode rootNode = null;                               //声明TreeView的根节点引用
            TreeNode childNode = null;                              //声明TreeView的子节点引用
            strSql = "select " + strCode + " , " + strName + " from " + strTable;   //查询数据源的SQL语句
            tv.Nodes.Clear();                                       //删除所有树节点
            tv.ImageList = imgList;                                 //设置包含树节点所使用的Image对象的ImageList
            rootNode = new TreeNode();                              //创建根节点
            rootNode.Tag = null;                                    //根节点的标签属性设置为空
            rootNode.Text = rootName;                               //设置根节点的Text属性
                                                                    //设置根节点处于未选定状态时的图像在列表中的索引值
            rootNode.ImageIndex = 1;
            rootNode.SelectedImageIndex = 0;                        //设置根节点处于选定状态时的图像索引值
            try
            {
                ds = db.GetDataSet(strSql, strTable);               //得到DataSet对象 
                dt = ds.Tables[strTable];                               //从ds的表集合中取出指定名称的DataTable对象
                foreach (DataRow row in dt.Rows)                    //遍历所有的行
                {
                    childNode = new TreeNode();                 //创建子节点
                    childNode.Tag = row[strCode];               //设置Tag属性值为代码字段值
                    childNode.Text = row[strName].ToString();       //设置Text属性值为名称字段值
                    childNode.ImageIndex = 1;                       //设置节点处于未选定状态时的图像索引值
                    childNode.SelectedImageIndex = 0;               //设置节点处于选定状态时的图像索引值
                    rootNode.Nodes.Add(childNode);              //将子节点添加到根节点集合的末尾
                }
                tv.Nodes.Add(rootNode);                         //TreeView控件添加根节点
                tv.ExpandAll();                                 //展开所有的节点
            }
            catch (Exception e)                                     //捕获异常
            {
                MessageBox.Show(e.Message, "软件提示");         //异常信息提示
                throw e;                                            //抛出异常
            }
        }

        /// <summary>
        /// 清空DataGridView
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        public void DataGridViewReset(DataGridView dgv)
        {
            if (dgv.DataSource != null)
            {
                //若DataGridView绑定的数据源为DataTable
                if (dgv.DataSource.GetType() == typeof(DataTable))
                {
                    DataTable dt = dgv.DataSource as DataTable;
                    dt.Clear();
                }

                //若DataGridView绑定的数据源为BindingSource
                if (dgv.DataSource.GetType() == typeof(BindingSource))
                {
                    BindingSource bs = dgv.DataSource as BindingSource;
                    DataTable dt = bs.DataSource as DataTable;
                    dt.Clear();
                }
            }
        }
        
        /// <summary>
        /// 在DataGridView控件的指定位置插入行
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="bs">BindingSource组件</param>
        /// <param name="dt">DataTable内存数据表</param>
        /// <param name="intPosIndex">指定位置的索引值</param>
        /// <returns>DataGridViewRow对象的引用</returns>
        public DataGridViewRow DataGridViewInsertRow(DataGridView dgv, BindingSource bs, DataTable dt, int intPosIndex)
        {
            DataGridViewRow dgvr = null;

            try
            {
                DataRow dr = dt.NewRow(); //基于某个DataTable的结构( 列结构仍然使用初始时产生的结构(如：sda.Fill(dt)) )，创建一个DataRow对象
                dt.Rows.InsertAt(dr, intPosIndex); //在数据源中插入新创建的DataRow对象
                bs.DataSource = dt;
                dgv.DataSource = bs;
                dgvr = dgv.Rows[intPosIndex];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dgvr;
        }

        /// <summary>
        /// 在DataGridView控件的末尾添加行
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="bs">BingdingSource组件</param>
        /// <param name="dt">DataTable内存数据表</param>
        /// <returns>DataGridViewRow对象的引用</returns>
        public DataGridViewRow DataGridViewInsertRowAtEnd(DataGridView dgv, BindingSource bs, DataTable dt)
        {
            DataGridViewRow dgvr = null;

            try
            {
                DataRow dr = dt.NewRow(); 
                dt.Rows.Add(dr); //在结尾添加数据行对象
                bs.DataSource = dt;
                dgv.DataSource = bs;
                dgvr = dgv.Rows[dgv.RowCount - 1];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dgvr;
        }

        /// <summary>
        /// ComboBox或DataGridViewComboBoxColumn绑定到数据源
        /// </summary>
        /// <param name="obj">要绑定数据源的控件</param>
        /// <param name="strValueColumn">ValueMember属性要绑定的列名称</param>
        /// <param name="strTextColumn">DisplayMember属性要绑定的列名称</param>
        /// <param name="strSql">SQL查询语句</param>
        /// <param name="strTable">数据表的名称</param>
        public void BindComboBox(Object obj, string strValueColumn, string strTextColumn, string strSql, string strTable) //Component —替换—> Object
        {
            try
            {
                string strType = obj.GetType().ToString();      //获取obj的Type值，并转为字符串
                                                                //截取字符串，得到不包含命名空间的表示类型名称的字符串
                strType = strType.Substring(strType.LastIndexOf(".") + 1);
                switch (strType)                                //判断控件的类型
                {
                    case "ComboBox":                            //若是ComboBox类型
                        ComboBox cbx = (ComboBox)obj;       //类型显式转换
                        cbx.BeginUpdate();                      //当将多项一次一项地添加到ComboBox时维持性能
                                                                //设置数据源
                        cbx.DataSource = db.GetDataSet(strSql, strTable).Tables[strTable];
                        cbx.DisplayMember = strTextColumn;  //设置ComboBox的显示属性
                        cbx.ValueMember = strValueColumn;   //设置ComboBox中的项的实际值
                        cbx.EndUpdate();                        //恢复绘制ComboBox
                        break;
                    case "DataGridViewComboBoxColumn":      //若是DataGridViewComboBoxColumn类型
                                                            //类型显式转换
                        DataGridViewComboBoxColumn dgvcbx = (DataGridViewComboBoxColumn)obj;
                        //设置数据源
                        dgvcbx.DataSource = db.GetDataSet(strSql, strTable).Tables[strTable];
                        //设置DataGridViewComboBoxColumn的显示属性
                        dgvcbx.DisplayMember = strTextColumn;
                        //设置DataGridViewComboBoxColumn中的项的实际值
                        dgvcbx.ValueMember = strValueColumn;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)                                 //捕获异常
            {
                throw e;                                        //抛出异常
            }
        }

        /// <summary>
        /// 实例化ReportDocument
        /// </summary>
        /// <param name="strReportFileName">报表文件的名称</param>
        /// <param name="strSelectionFormula">记录的规则或公式</param>
        /// <returns>ReportDocument对象的引用</returns>
        public ReportDocument CrystalReports(string strReportFileName, string strSelectionFormula)
        {
            //获取报表路径
            string strReportPath = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, 
                Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\"));
            strReportPath += @"\RP\" + strReportFileName;
            //加载报表并设置查询规则
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(strReportPath);
            reportDoc.DataDefinition.RecordSelectionFormula = strSelectionFormula;

            //水晶报表动态链接数据库
            TableLogOnInfo logOnInfo = new TableLogOnInfo();
            logOnInfo.ConnectionInfo.ServerName = OperatorFile.GetIniFileString("DataBase", "Server", "", Application.StartupPath + "\\ERP.ini");
            logOnInfo.ConnectionInfo.DatabaseName = "db_ERP";
            logOnInfo.ConnectionInfo.UserID = OperatorFile.GetIniFileString("DataBase", "UserID", "", Application.StartupPath + "\\ERP.ini");
            logOnInfo.ConnectionInfo.Password = OperatorFile.GetIniFileString("DataBase", "Pwd", "", Application.StartupPath + "\\ERP.ini");

            // 对报表中的每个表依次循环(把连接信息存入每一个Table中)
            foreach (Table tb in reportDoc.Database.Tables)
            {
                tb.ApplyLogOnInfo(logOnInfo);
            }

            //返回ReportDocument对象 
            return reportDoc;
        }

        /// <summary>
        /// 实例化ReportDocument
        /// </summary>
        /// <param name="strReportFileName">报表文件的名称</param>
        /// <param name="strSql">查询SQL语句</param>
        /// <param name="strTable">数据表</param>
        /// <returns>ReportDocument对象的引用</returns>
        public ReportDocument CrystalReports(string strReportFileName, string strSql,string strTable)
        {
            //获取报表路径
            string strReportPath = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0,
                Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\"));
            strReportPath += @"\RP\" + strReportFileName;
            //得到dt数据源
            DataTable dt = db.GetDataTable(strSql, strTable);
            //ReportDocument对象加载rpt文件并绑定到数据源dt
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(strReportPath);
            reportDoc.SetDataSource(dt.DefaultView); //DataView是接口IEnumerable的实现子类,此处使用了“接口”的多态特性

            //水晶报表动态链接数据库
            TableLogOnInfo logOnInfo = new TableLogOnInfo();
            logOnInfo.ConnectionInfo.ServerName = OperatorFile.GetIniFileString("DataBase", "Server", "", Application.StartupPath + "\\ERP.ini");
            logOnInfo.ConnectionInfo.DatabaseName = "db_ERP";
            logOnInfo.ConnectionInfo.UserID = OperatorFile.GetIniFileString("DataBase", "UserID", "", Application.StartupPath + "\\ERP.ini");
            logOnInfo.ConnectionInfo.Password = OperatorFile.GetIniFileString("DataBase", "Pwd", "", Application.StartupPath + "\\ERP.ini");

            // 对报表中的每个表依次循环(把连接信息存入每一个Table中)
            foreach (Table tb in reportDoc.Database.Tables)
            {
                tb.ApplyLogOnInfo(logOnInfo);
            }

            //返回ReportDocument对象 
            return reportDoc;
        }

        /// <summary>
        /// 控制可编辑控件的键盘输入，该方法限定控件只可以接收表示非负十进制数的字符
        /// </summary>
        /// <param name="e">为 KeyPress 事件提供数据</param>
        /// <param name="con">可编辑文本控件</param>
        public void InputNumeric(KeyPressEventArgs e,Control con)
        {
            //在可编辑控件的Text属性为空的情况下，不允许输入".字符"
            if (String.IsNullOrEmpty(con.Text) && e.KeyChar.ToString() == ".")
            {
                //把Handled设为true，取消KeyPress事件，防止控件处理按键
                e.Handled = true;
            }

            //可编辑控件不允许输入多个"."字符
            if (con.Text.Contains(".") && e.KeyChar.ToString() == ".")
            {
                e.Handled = true;
            }

            //在可编辑控件中，只可以输入“数字字符”、".字符" 、"字符"(删除键对应的字符)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar.ToString() != "." && e.KeyChar.ToString() != "")
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 控制可编辑控件的键盘输入，该方法限定控件只可以接收表示非负整数的字符
        /// </summary>
        /// <param name="e">为 KeyPress 事件提供数据</param>
        public void InputInteger(KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar.ToString() != "")
            {
                //把Handled设为true，取消KeyPress事件，防止控件处理按键
                e.Handled = true;
            }
        }

        /// <summary>
        /// 获取数据库系统的时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        public DateTime GetDBTime()
        {
            DateTime dtDBTime;

            try
            {
                dtDBTime = Convert.ToDateTime(db.GetSingleObject("SELECT GETDATE()"));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "软件提示");
                throw e;
            }

            return dtDBTime;
        }

        /// <summary>
        /// 生成单据编号
        /// </summary>
        /// <param name="strTable">数据表</param>
        /// <param name="strBillCodeColumn">数据表中表示代码的列</param>
        /// <param name="strBillDateColumn">数据表中表示日期的列</param>
        /// <param name="dtBillDate">生成单据的日期</param>
        /// <returns>新单据编号</returns>
        public string BuildBillCode(string strTable, string strBillCodeColumn,string strBillDateColumn,DateTime dtBillDate)
        {
            string strSql;                                          //声明表示SQL语句的字符串
            string strBillDate;                                     //表示单据日期
            string strMaxSeqNum;                                    //表示某日最大单据编号的后4位
            string strNewSeqNum;                                    //表示新单据编号的后4位
            string strBillCode;                                     //表示单据编号
            try
            {
                strBillDate = dtBillDate.ToString("yyyyMMdd");      //单据日期格式化
                                                                    //使用SELECT语句查询数据表，得到某日最大单据编号的后4位
                strSql = "SELECT  SUBSTRING(MAX(" + strBillCodeColumn + "),10,4) FROM " + strTable + " WHERE " + strBillDateColumn + " = '" + dtBillDate.ToString("yyyy-MM-dd")+"'";
                strMaxSeqNum = db.GetSingleObject(strSql) as string;    //获取查询的结果集，并转为字符串
                if (String.IsNullOrEmpty(strMaxSeqNum))         //若某日无单据
                {
                    strMaxSeqNum = "0000";                      //默认最大单据编号的后4位为0000
                }
                //计算新单据编号的后4位
                strNewSeqNum = (Convert.ToInt32(strMaxSeqNum) + 1).ToString("0000");
                strBillCode = strBillDate + "-" + strNewSeqNum;     //得到新单据编号
            }
            catch (Exception ex)                                    //捕获异常
            {
                MessageBox.Show(ex.Message, "软件提示");            //异常信息提示
                throw ex;                                           //抛出异常
            }
            return strBillCode;										//返回新单据编号

        }

        /// <summary>
        /// 通过若干条领料单记录计算平均单价
        /// </summary>
        /// <param name="strPRProduceCode">生产单号</param>
        /// <param name="strInvenCode">存货代码</param>
        /// <returns>平均单价</returns>
        public decimal GetAvePriceBySTGetMaterial(string strPRProduceCode, string strInvenCode)
        {
            string strSql = null;
            decimal decAvePrice;

            strSql = "SELECT SUM(UnitPrice * Quantity) / SUM(Quantity) FROM STGetMaterial WHERE BillType = '1' AND IsFlag = '1'  AND PRProduceCode = '" + strPRProduceCode + "' AND InvenCode = '" + strInvenCode + "'";

            try
            {
               decAvePrice = Convert.ToDecimal(db.GetSingleObject(strSql));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "软件提示");
                throw ex;
            }

            return decAvePrice;
        }

        /// <summary>
        /// DataGridView导出到Excel
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="strTitle">导出的Excel标题</param>
        public void DataGridViewExportToExcel(DataGridView dgv,string strTitle)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = false;
            saveFileDialog.FileName = strTitle+".xls";

            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) //导出时，点击【取消】按钮
            {
                return;
            }

            Stream myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
            
            string strHeaderText = "";

            try
            {   
                //写标题
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        strHeaderText += "\t";
                    }

                    strHeaderText += dgv.Columns[i].HeaderText;
                }

                sw.WriteLine(strHeaderText);
                
                //写内容
                string strItemValue = "";

                for (int j = 0; j < dgv.RowCount; j++)
                {
                    strItemValue = "";

                    for (int k = 0; k < dgv.ColumnCount; k++)
                    {
                        if (k > 0)
                        {
                            strItemValue += "\t";
                        }

                        strItemValue += dgv.Rows[j].Cells[k].Value.ToString();
                    }

                    sw.WriteLine(strItemValue); //把dgv的每一行的信息写为sw的每一行
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"软件提示");
                throw ex;
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }

        /// <summary>
        /// 判断数据表中记录的主键值是否存在外键约束
        /// </summary>
        /// <param name="strPrimaryTable">主键表</param>
        /// <param name="strPrimaryValue">数据表中某条记录主键的值</param>
        /// <returns></returns>
        public bool IsExistConstraint(string strPrimaryTable,string strPrimaryValue)
        {
            bool booIsExist = false;
            string strSql = null;
            string strForeignColumn = null;
            string strForeignTable = null;
            SqlDataReader sdr = null;

            try
            {
                //创建SqlParameter对象，并赋值
                SqlParameter param = new SqlParameter("@PrimaryTable", SqlDbType.VarChar);
                param.Value = strPrimaryTable;
               //创建泛型
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(param);
                //把泛型中的元素复制到数组中
                SqlParameter[] inputParameters = parameters.ToArray();
                //通过存储过程得到外键表的相关数据
                DataTable dt = db.GetDataTable("P_QueryForeignConstraint", inputParameters);
                
                //循环这些相关数据
                foreach (DataRow dr in dt.Rows)
                {
                    strForeignTable = dr["ForeignTable"].ToString();
                    strForeignColumn = dr["ForeignColumn"].ToString();
                    strSql = "Select " + strForeignColumn + " From " + strForeignTable + " Where " + strForeignColumn + " = '" + strPrimaryValue + "'";
                    sdr = db.GetDataReader(strSql);

                    if (sdr.HasRows)
                    {
                        booIsExist = true;
                        sdr.Close();
                        //跳出循环
                        break;
                    }

                    sdr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"软件提示");
                throw ex;
            }

            return booIsExist;
        }

        /// <summary>
        /// 通过控制按钮的Enabled属性来达到控制操作权限的目的
        /// </summary>
        /// <param name="iComp">Button或ToolStripButton按钮</param>
        /// <param name="form">被打开的窗体</param>
        public void CortrolButtonEnabled(IComponent iComp, Form form)
        {
            string strRightTag = null;
            Button btn = null;
            ToolStripButton tsb = null;

            //若是“Button”
            if (iComp.GetType() == typeof(Button))
            {
                btn = (Button)iComp;
                strRightTag = btn.Name.Substring(3);
            }
            
            //若是“ToolStripButton”
            if (iComp.GetType() == typeof(ToolStripButton))
            {
                tsb = (ToolStripButton)iComp;
                strRightTag = tsb.Name.Substring(4);
            }

            //系统管理员不受限制
            if (PropertyClass.IsAdmin == "1")
            {
                if (btn != null)
                {
                    btn.Enabled = true;
                }
                else
                {
                    tsb.Enabled = !tsb.Enabled;
                }
            }
            else
            {
                string strSql = "Select IsRight From SYAssignRight Where OperatorCode = '" + PropertyClass.OperatorCode + "' ";
                strSql += "and ModuleTag = '" + form.Tag.ToString() + "'and RightTag = '" + strRightTag + "'";

                try
                {
                    DataTable dt = db.GetDataTable(strSql, "SYAssignRight");

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        DataColumn dc = dt.Columns["IsRight"];

                        if (dr[dc].ToString() == "1") //若具有权限
                        {
                            if (btn != null)
                            {
                                btn.Enabled = true;
                            }
                            else
                            {
                                tsb.Enabled = !tsb.Enabled;
                            }
                        }
                        else //若无权限
                        {
                            if (btn != null)
                            {
                                btn.Enabled = false;
                            }
                            else
                            {
                                tsb.Enabled = tsb.Enabled;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }
            }
        }
        
        /// <summary>
        /// 打开Form窗体
        /// </summary>
        /// <param name="menuItem">菜单项的引用</param>
        /// <param name="form">要打开的窗体的引用</param>
        public void ShowForm(ToolStripMenuItem menuItem, Form form)
        {
            switch (menuItem.Tag.ToString())
            {
                case "111"://存货类别

                    FormBSInvenType invenType = new FormBSInvenType();
                    invenType.MdiParent = form;
                    invenType.StartPosition = FormStartPosition.CenterScreen;
                    invenType.Tag = menuItem.Tag.ToString();
                    invenType.Show();
                    break;

                case "112"://部门分类

                    FormBSDepartment department = new FormBSDepartment();
                    department.MdiParent = form;
                    department.StartPosition = FormStartPosition.CenterScreen;
                    department.Tag = menuItem.Tag.ToString();
                    department.Show();
                    break;

                case "113"://费用类别

                    FormBSCostType costType = new FormBSCostType();
                    costType.MdiParent = form;
                    costType.StartPosition = FormStartPosition.CenterScreen;
                    costType.Tag = menuItem.Tag.ToString();
                    costType.Show();
                    break;

                case "121"://存货档案

                    FormBSInven inven = new FormBSInven();
                    inven.MdiParent = form;
                    inven.StartPosition = FormStartPosition.CenterScreen;
                    inven.Tag = menuItem.Tag.ToString();
                    inven.Show();
                    break;

                case "122"://供应商档案

                    FormBSSupplier supplier = new FormBSSupplier();
                    supplier.MdiParent = form;
                    supplier.StartPosition = FormStartPosition.CenterScreen;
                    supplier.Tag = menuItem.Tag.ToString();
                    supplier.Show();
                    break;
                case "123"://客户档案

                    FormBSCustomer customer = new FormBSCustomer();
                    customer.MdiParent = form;
                    customer.StartPosition = FormStartPosition.CenterScreen;
                    customer.Tag = menuItem.Tag.ToString();
                    customer.Show();
                    break;

                case "124"://费用档案

                    FormBSCost cost = new FormBSCost();
                    cost.MdiParent = form;
                    cost.StartPosition = FormStartPosition.CenterScreen;
                    cost.Tag = menuItem.Tag.ToString();
                    cost.Show();
                    break;

                case "125"://仓库档案

                    FormBSStore store = new FormBSStore();
                    store.MdiParent = form;
                    store.StartPosition = FormStartPosition.CenterScreen;
                    store.Tag = menuItem.Tag.ToString();
                    store.Show();
                    break;

                case "126"://员工档案

                    FormBSEmployee employee = new FormBSEmployee();
                    employee.MdiParent = form;
                    employee.StartPosition = FormStartPosition.CenterScreen;
                    employee.Tag = menuItem.Tag.ToString();
                    employee.Show();
                    break;

                case "130"://结算账户

                    FormBSAccount account = new FormBSAccount();
                    account.MdiParent = form;
                    account.StartPosition = FormStartPosition.CenterScreen;
                    account.Tag = menuItem.Tag.ToString();
                    account.Show();
                    break;

                case "140"://物料清单

                    FormBSBom bom = new FormBSBom();
                    bom.MdiParent = form;
                    bom.StartPosition = FormStartPosition.CenterScreen;
                    bom.Tag = menuItem.Tag.ToString();
                    bom.Show();
                    break;

                case "150"://初始化库存

                    FormInitStock initStock = new FormInitStock();
                    initStock.MdiParent = form;
                    initStock.StartPosition = FormStartPosition.CenterScreen;
                    initStock.Tag = menuItem.Tag.ToString();
                    initStock.Show();
                    break;

                case "210"://采购订单

                    FormPUOrder puOrder = new FormPUOrder();
                    puOrder.MdiParent = form;
                    puOrder.StartPosition = FormStartPosition.CenterScreen;
                    puOrder.Tag = menuItem.Tag.ToString();
                    puOrder.Show();
                    break;


                case "220"://采购入库单

                    FormPUInStore puInStore = new FormPUInStore();
                    puInStore.MdiParent = form;
                    puInStore.StartPosition = FormStartPosition.CenterScreen;
                    puInStore.Tag = menuItem.Tag.ToString();
                    puInStore.Show();
                    break;

                case "230"://采购付款单

                    FormPUPay formPUPay = new FormPUPay();
                    formPUPay.MdiParent = form;
                    formPUPay.StartPosition = FormStartPosition.CenterScreen;
                    formPUPay.Tag = menuItem.Tag.ToString();
                    formPUPay.Show();
                    break;

                case "310"://销售订单

                    FormSEOrder formSEOrder = new FormSEOrder();
                    formSEOrder.MdiParent = form;
                    formSEOrder.StartPosition = FormStartPosition.CenterScreen;
                    formSEOrder.Tag = menuItem.Tag.ToString();
                    formSEOrder.Show();
                    break;

                case "320"://销售出库单

                    FormSEOutStore formSEOutStore = new FormSEOutStore();
                    formSEOutStore.MdiParent = form;
                    formSEOutStore.StartPosition = FormStartPosition.CenterScreen;
                    formSEOutStore.Tag = menuItem.Tag.ToString();
                    formSEOutStore.Show();
                    break;

                case "330"://销售收款单

                    FormSEGather formSEGather = new FormSEGather();
                    formSEGather.MdiParent = form;
                    formSEGather.StartPosition = FormStartPosition.CenterScreen;
                    formSEGather.Tag = menuItem.Tag.ToString();
                    formSEGather.Show();
                    break;

                case "410"://领料单

                    FormSTGetMaterial formSTGetMaterial = new FormSTGetMaterial();
                    formSTGetMaterial.MdiParent = form;
                    formSTGetMaterial.StartPosition = FormStartPosition.CenterScreen;
                    formSTGetMaterial.Tag = menuItem.Tag.ToString();
                    formSTGetMaterial.Show();
                    break;

                case "420"://退料单

                    FormSTReturnMaterial formSTReturnMaterial = new FormSTReturnMaterial();
                    formSTReturnMaterial.MdiParent = form;
                    formSTReturnMaterial.StartPosition = FormStartPosition.CenterScreen;
                    formSTReturnMaterial.Tag = menuItem.Tag.ToString();
                    formSTReturnMaterial.Show();
                    break;

                case "430"://报损清单

                    FormSTLoss formSTLoss = new FormSTLoss();
                    formSTLoss.MdiParent = form;
                    formSTLoss.StartPosition = FormStartPosition.CenterScreen;
                    formSTLoss.Tag = menuItem.Tag.ToString();
                    formSTLoss.Show();
                    break;

                case "440"://库存盘点

                    FormSTCheck formSTCheck = new FormSTCheck();
                    formSTCheck.MdiParent = form;
                    formSTCheck.StartPosition = FormStartPosition.CenterScreen;
                    formSTCheck.Tag = menuItem.Tag.ToString();
                    formSTCheck.Show();
                    break;

                case "450"://库存清单

                    FormStockQuery formStockQuery = new FormStockQuery();
                    formStockQuery.MdiParent = form;
                    formStockQuery.StartPosition = FormStartPosition.CenterScreen;
                    formStockQuery.Tag = menuItem.Tag.ToString();
                    formStockQuery.Show();
                    break;

                case "510"://主生成计划

                    FormPRPlan formPRPlan = new FormPRPlan();
                    formPRPlan.MdiParent = form;
                    formPRPlan.StartPosition = FormStartPosition.CenterScreen;
                    formPRPlan.Tag = menuItem.Tag.ToString();
                    formPRPlan.Show();
                    break;

                case "520"://生产单

                    FormPRProduce formPRProduce = new FormPRProduce();
                    formPRProduce.MdiParent = form;
                    formPRProduce.StartPosition = FormStartPosition.CenterScreen;
                    formPRProduce.Tag = menuItem.Tag.ToString();
                    formPRProduce.Show();
                    break;

                case "530"://生产完工处理

                    FormProduceComplete formProduceComplete = new FormProduceComplete();
                    formProduceComplete.MdiParent = form;
                    formProduceComplete.StartPosition = FormStartPosition.CenterScreen;
                    formProduceComplete.Tag = menuItem.Tag.ToString();
                    formProduceComplete.Show();
                    break;

                case "540"://生产入库单

                    FormPRInStore formPRInStore = new FormPRInStore();
                    formPRInStore.MdiParent = form;
                    formPRInStore.StartPosition = FormStartPosition.CenterScreen;
                    formPRInStore.Tag = menuItem.Tag.ToString();
                    formPRInStore.Show();
                    break;

                case "610"://客户进程

                    FormCustomerCourse formCustomerCourse = new FormCustomerCourse();
                    formCustomerCourse.MdiParent = form;
                    formCustomerCourse.StartPosition = FormStartPosition.CenterScreen;
                    formCustomerCourse.Tag = menuItem.Tag.ToString();
                    formCustomerCourse.Show();
                    break;

                case "620"://客户基础分类

                    FormBaseType formBaseType = new FormBaseType();
                    formBaseType.MdiParent = form;
                    formBaseType.StartPosition = FormStartPosition.CenterScreen;
                    formBaseType.Tag = menuItem.Tag.ToString();
                    formBaseType.Show();
                    break;

                case "630"://类型分析

                    FormCustomerAnalyse formCustomerAnalyse = new FormCustomerAnalyse();
                    formCustomerAnalyse.MdiParent = form;
                    formCustomerAnalyse.StartPosition = FormStartPosition.CenterScreen;
                    formCustomerAnalyse.Tag = menuItem.Tag.ToString();
                    formCustomerAnalyse.Show();
                    break;

                case "710"://银行存取款单

                    FormFIDeposit formFIDeposit = new FormFIDeposit();
                    formFIDeposit.MdiParent = form;
                    formFIDeposit.StartPosition = FormStartPosition.CenterScreen;
                    formFIDeposit.Tag = menuItem.Tag.ToString();
                    formFIDeposit.Show();
                    break;

                case "720"://采购费用单

                    FormFIPurCost formFIPurCost = new FormFIPurCost();
                    formFIPurCost.MdiParent = form;
                    formFIPurCost.StartPosition = FormStartPosition.CenterScreen;
                    formFIPurCost.Tag = menuItem.Tag.ToString();
                    formFIPurCost.Show();
                    break;

                case "730"://销售费用单

                    FormFISelCost formFISelCost = new FormFISelCost();
                    formFISelCost.MdiParent = form;
                    formFISelCost.StartPosition = FormStartPosition.CenterScreen;
                    formFISelCost.Tag = menuItem.Tag.ToString();
                    formFISelCost.Show();
                    break;

                case "810"://原材料采购明细表

                    FormPurReport formPurReport = new FormPurReport();
                    formPurReport.MdiParent = form;
                    formPurReport.StartPosition = FormStartPosition.CenterScreen;
                    formPurReport.Tag = menuItem.Tag.ToString();
                    formPurReport.Show();
                    break;

                case "820"://原材料采购汇总表

                    FormPurCollectReport formPurCollectReport = new FormPurCollectReport();
                    formPurCollectReport.MdiParent = form;
                    formPurCollectReport.StartPosition = FormStartPosition.CenterScreen;
                    formPurCollectReport.Tag = menuItem.Tag.ToString();
                    formPurCollectReport.Show();
                    break;

                case "830"://产品销售明细表

                    FormSelReport formSelReport = new FormSelReport();
                    formSelReport.MdiParent = form;
                    formSelReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelReport.Tag = menuItem.Tag.ToString();
                    formSelReport.Show();
                    break;

                case "840"://产品销售汇总表

                    FormSelCollectReport formSelCollectReport = new FormSelCollectReport();
                    formSelCollectReport.MdiParent = form;
                    formSelCollectReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelCollectReport.Tag = menuItem.Tag.ToString();
                    formSelCollectReport.Show();
                    break;
                        
                case "850"://产品销售毛利明细表

                    FormSelProfitReport formSelProfitReport = new FormSelProfitReport();
                    formSelProfitReport.MdiParent = form;
                    formSelProfitReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelProfitReport.Tag = menuItem.Tag.ToString();
                    formSelProfitReport.Show();
                    break;


                case "860"://产品销售毛利汇总表

                    FormSelProfitCollectReport formSelProfitCollectReport = new FormSelProfitCollectReport();
                    formSelProfitCollectReport.MdiParent = form;
                    formSelProfitCollectReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelProfitCollectReport.Tag = menuItem.Tag.ToString();
                    formSelProfitCollectReport.Show();
                    break;

                case "870"://存货库存报警明细表

                    FormStockWarnReport formStockWarnReport = new FormStockWarnReport();
                    formStockWarnReport.MdiParent = form;
                    formStockWarnReport.StartPosition = FormStartPosition.CenterScreen;
                    formStockWarnReport.Tag = menuItem.Tag.ToString();
                    formStockWarnReport.Show();
                    break;

                case "910"://操作员管理

                    FormSYOperator formSYOperator = new FormSYOperator();
                    formSYOperator.MdiParent = form;
                    formSYOperator.StartPosition = FormStartPosition.CenterScreen;
                    formSYOperator.Tag = menuItem.Tag.ToString();
                    formSYOperator.Show();
                    break;

                case "920"://密码修改

                    FormPassWord formPassWord = new FormPassWord();
                    formPassWord.MdiParent = form;
                    formPassWord.StartPosition = FormStartPosition.CenterScreen;
                    formPassWord.Tag = menuItem.Tag.ToString();
                    formPassWord.Show();
                    break;

                case "930"://操作权限

                    FormAssignRight formAssignRight = new FormAssignRight();
                    formAssignRight.MdiParent = form;
                    formAssignRight.StartPosition = FormStartPosition.CenterScreen;
                    formAssignRight.Tag = menuItem.Tag.ToString();
                    formAssignRight.Show();
                    break;

                default:

                    break;
            }
        }

        /// <summary>
        /// 打开Form窗体
        /// </summary>
        /// <param name="str">工具栏按钮文本</param>
        /// <param name="form">要打开的窗体的引用</param>
        public void ShowForm(string str, Form form)
        {
            switch (str)
            {
               
                case "采购入库单"://采购入库单

                    FormPUInStore puInStore = new FormPUInStore();
                    puInStore.MdiParent = form;
                    puInStore.StartPosition = FormStartPosition.CenterScreen;
                    puInStore.Show();
                    break;
                case "销售出库单"://销售出库单

                    FormSEOutStore formSEOutStore = new FormSEOutStore();
                    formSEOutStore.MdiParent = form;
                    formSEOutStore.StartPosition = FormStartPosition.CenterScreen;
                    formSEOutStore.Show();
                    break;
                case "库存盘点"://库存盘点

                    FormSTCheck formSTCheck = new FormSTCheck();
                    formSTCheck.MdiParent = form;
                    formSTCheck.StartPosition = FormStartPosition.CenterScreen;
                    formSTCheck.Show();
                    break;
                case "类型分析"://类型分析

                    FormCustomerAnalyse formCustomerAnalyse = new FormCustomerAnalyse();
                    formCustomerAnalyse.MdiParent = form;
                    formCustomerAnalyse.StartPosition = FormStartPosition.CenterScreen;
                    formCustomerAnalyse.Show();
                    break;
                case "采购费用单"://采购费用单

                    FormFIPurCost formFIPurCost = new FormFIPurCost();
                    formFIPurCost.MdiParent = form;
                    formFIPurCost.StartPosition = FormStartPosition.CenterScreen;
                    formFIPurCost.Show();
                    break;

                case "销售费用单"://销售费用单

                    FormFISelCost formFISelCost = new FormFISelCost();
                    formFISelCost.MdiParent = form;
                    formFISelCost.StartPosition = FormStartPosition.CenterScreen;
                    formFISelCost.Show();
                    break;

                case "原材料采购明细表"://原材料采购明细表

                    FormPurReport formPurReport = new FormPurReport();
                    formPurReport.MdiParent = form;
                    formPurReport.StartPosition = FormStartPosition.CenterScreen;
                    formPurReport.Show();
                    break;
                case "产品销售明细表"://产品销售明细表

                    FormSelReport formSelReport = new FormSelReport();
                    formSelReport.MdiParent = form;
                    formSelReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelReport.Show();
                    break;
                case "产品销售毛利明细表"://产品销售毛利明细表

                    FormSelProfitReport formSelProfitReport = new FormSelProfitReport();
                    formSelProfitReport.MdiParent = form;
                    formSelProfitReport.StartPosition = FormStartPosition.CenterScreen;
                    formSelProfitReport.Show();
                    break;
            }
        }
    }
}
