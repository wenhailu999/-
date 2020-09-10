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

namespace ERP.SY
{
    public partial class FormAssignRight : Form
    {
        DataBase db = new DataBase();
        CommonUse commUse = new CommonUse();
        SqlDataAdapter sda = null;
        DataTable dt = null;

        public FormAssignRight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 在DataGridView控件中插入某个模块具有的操作功能及授权信息
        /// </summary>
        /// <param name="strModuleTag">模块标识</param>
        private void InsertOperation(string strModuleTag)
        {
            DataGridViewRow dgvr = null;                                //声明DataGridViewRow引用，并初始化null
                                                                        //若模块标识符合以下条件，则在DataGridView控件中显示添加、修改、删除权限
            if (strModuleTag.Substring(0, 1) == "1" || strModuleTag == "610" || strModuleTag == "620" ||
                strModuleTag == "910")
            {
                //在DataGridView控件的末尾添加行
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                //设置操作员代码
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;  //设置模块标识
                dgvr.Cells["RightTag"].Value = "Add";                   //设置操作标识（表示添加操作）
                dgvr.Cells["IsRight"].Value = "0";                          //设置授权标记的默认值为"0"（即无权限）
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "Amend";             //设置操作标识（表示修改操作）
                dgvr.Cells["IsRight"].Value = "0";
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "Delete";                    //设置操作标识（表示删除操作）
                dgvr.Cells["IsRight"].Value = "0";
            }
            //若模块标识符合以下条件，则在DataGridView控件中显示添加、修改、删除、审核、弃审权限
            if (strModuleTag.Substring(0, 1) == "2" || strModuleTag.Substring(0, 1) == "3" ||
                (strModuleTag.Substring(0, 1) == "4" && strModuleTag != "450") || (strModuleTag.Substring(0, 1)
                == "5" && strModuleTag != "530") || strModuleTag.Substring(0, 1) == "7")
            {
                //在DataGridView控件的末尾添加行
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                //设置操作员代码
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;  //设置模块标识
                dgvr.Cells["RightTag"].Value = "Add";                   //设置操作标识（表示添加操作）
                dgvr.Cells["IsRight"].Value = "0";                          //设置授权标记的默认值为"0"（即无权限）
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "Amend";             //设置操作标识（表示修改操作）
                dgvr.Cells["IsRight"].Value = "0";
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "Delete";                    //设置操作标识（表示删除操作）
                dgvr.Cells["IsRight"].Value = "0";
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "Check";                 //设置操作标识（表示审核操作）
                dgvr.Cells["IsRight"].Value = "0";
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "UnCheck";               //设置操作标识（表示弃审操作）
                dgvr.Cells["IsRight"].Value = "0";
            }
            //若模块标识符合以下条件，则在DataGridView控件中显示查询权限
            if (strModuleTag == "450" || strModuleTag == "630" || strModuleTag.Substring(0, 1) == "8")
            {
                //在DataGridView控件的末尾添加行
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                //设置操作员代码
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;  //设置模块标识
                dgvr.Cells["RightTag"].Value = "Query";                 //设置操作标识（表示查询操作）
                dgvr.Cells["IsRight"].Value = "0";                          //设置授权标记的默认值为"0"（即无权限）
            }
            //若模块标识符合以下条件，则在DataGridView控件中显示审核、弃审权限
            if (strModuleTag == "530")
            {
                //在DataGridView控件的末尾添加行
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                //设置操作员代码
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;  //设置模块标识
                dgvr.Cells["RightTag"].Value = "Check";                 //设置操作标识（表示审核操作）
                dgvr.Cells["IsRight"].Value = "0";                          //设置授权标记的默认值为"0"（即无权限）
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;
                dgvr.Cells["RightTag"].Value = "UnCheck";               //设置操作标识（表示弃审操作）
                dgvr.Cells["IsRight"].Value = "0";
            }
            //若模块标识符合以下条件，则在DataGridView控件中显示保存权限
            if (strModuleTag == "930")
            {
                //在DataGridView控件的末尾添加行
                dgvr = commUse.DataGridViewInsertRowAtEnd(dgvINRightInfo, bsINRight, dt);
                //设置操作员代码
                dgvr.Cells["OperatorCode"].Value = tvOperator.SelectedNode.Tag;
                dgvr.Cells["ModuleTag"].Value = tvModule.SelectedNode.Tag;  //设置模块标识
                dgvr.Cells["RightTag"].Value = "Save";                  //设置操作标识（表示保存操作）
                dgvr.Cells["IsRight"].Value = "0";                          //设置授权标记的默认值为"0"（即无权限）
            }
        }

        private void FormAssignRight_Load(object sender, EventArgs e)
        {
            //设置用户操作权限
            commUse.CortrolButtonEnabled(toolSave, this);
            //TreeView控件绑定到数据源，显示操作员
            commUse.BuildTree(tvOperator, imageList1, "操作员", "SYOperator Where IsAdmin <> '1'", "OperatorCode", "OperatorName");
            //TreeView控件绑定到数据源，显示系统模块
            commUse.BuildTree(tvModule, imageList1, "功能模块", "INModule", "ModuleTag", "ModuleName");
            //DataGridView控件的“RightTag”列绑定到数据源
            commUse.BindComboBox(dgvINRightInfo.Columns["RightTag"], "RightTag", "RightName", "Select RightTag,RightName From INRight", "INRight");
        }

        private void tvModule_AfterSelect(object sender, TreeViewEventArgs e)
        {
            commUse.DataGridViewReset(dgvINRightInfo);              //清空DataGridView控件
            if (tvOperator.SelectedNode != null)                            //若操作员节点不为空
            {
                if (tvOperator.SelectedNode.Tag != null)                    //若操作员节点为非根节点
                {
                    if (tvModule.SelectedNode != null)                  //若模块节点为非空
                    {
                        if (tvModule.SelectedNode.Tag != null)          //若模块节点为非根节点
                        {
                            //查询某个操作员的某个模块的操作权限信息
                            string strSql = "Select OperatorCode,ModuleTag,RightTag,IsRight From SYAssignRight ";
                            strSql += "Where OperatorCode = '" + tvOperator.SelectedNode.Tag.ToString() + "' and ModuleTag = '" + tvModule.SelectedNode.Tag.ToString() + "'";
                            try
                            {
                                sda = new SqlDataAdapter(strSql, db.Conn);  //实例化SqlDataAdapter
                                 //实例化SqlCommandBuilder，用于将数据源所做的更改与关联的SQL Server数据库的更改相协调
                                                      SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                                dt = new DataTable();                   //实例化DataTable
                                sda.Fill(dt);                           //将得到的数据源填充到dt中
                                bsINRight.DataSource = dt;              //BindingSource组件绑定到数据源
                                                                        //DataGridView控件绑定到BindingSource组件
                                dgvINRightInfo.DataSource = bsINRight;
                                //若无数据行，则插入该模块具有的操作功能及授权信息
                                if (dgvINRightInfo.RowCount == 0)
                                {
                                    InsertOperation(tvModule.SelectedNode.Tag.ToString());
                                }
                            }
                            catch (Exception ex)                        //捕获异常信息
                            {
                                MessageBox.Show(ex.Message, "软件提示");//异常信息提示
                                throw ex;
                            }
                        }
                    }
                }
            }
        }

        private void tvOperator_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tvModule_AfterSelect(sender, e);
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            if (tvOperator.SelectedNode != null)//操作员节点不为空
            {
                if (tvOperator.SelectedNode.Tag != null)//操作员节点为非根节点
                {
                    if (tvModule.SelectedNode != null)//模块节点为非空
                    {
                        if (tvModule.SelectedNode.Tag != null)//模块节点为非根节点
                        {
                            try
                            {
                                dgvINRightInfo.EndEdit(); //当前单元格结束编辑
                                bsINRight.EndEdit(); //将挂起的更改应用于基础数据源。

                                sda.Update(dt);//执行更改数据命令
                                MessageBox.Show("保存成功！", "软件提示");
                            }
                            catch (Exception ex)//捕获系统异常
                            {
                                MessageBox.Show("保存失败！(" + ex.Message + ")", "软件提示");
                            }
                        }
                    }
                }
            }
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
