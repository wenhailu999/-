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

namespace ERP
{
    public partial class FormMain : Form
    {
        DataBase db = new DataBase();
        CommonUse common = new CommonUse();
      
        public FormMain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.statusLabelTime.Text = "当前时间：" + DateTime.Now.ToString();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            FormDataAnalyse formDataAnalyse = new FormDataAnalyse();
            formDataAnalyse.MdiParent = this;
            formDataAnalyse.WindowState=FormWindowState.Maximized;
            formDataAnalyse.Show();
            this.timerTime.Start();
            this.statusLabelOperator.Text = "当前操作员："+PropertyClass.OperatorName;
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            common.ShowForm((ToolStripMenuItem)sender, this);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            common.ShowForm(e.ClickedItem.Text, this);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
