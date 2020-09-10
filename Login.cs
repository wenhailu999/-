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
using System.Data.SqlClient;

namespace ERP
{
    public partial class Login : Form
    {
        DataBase db = new DataBase();
        SqlDataReader sdr = null;

        public Login()
        {
            InitializeComponent();
        }
        //登录用户文本框敲回车键
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwd.Focus();
            }
        }
        //登录密码文本框敲回车键
        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                picLogin_Click(sender,e);
            }
        }
        //登录
        private void picLogin_Click(object sender, EventArgs e)
        {
            this.errInfo.Clear();

            if (String.IsNullOrEmpty(this.txtCode.Text.Trim()))
            {
                try
                {
                    this.errInfo.SetError(this.txtCode, "用户编码不能为空！");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }
                finally
                {

                }
            }

            if (String.IsNullOrEmpty(this.txtPwd.Text.Trim()))
            {
                try
                {
                    this.errInfo.SetError(this.txtPwd, "用户密码不能为空！");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示");
                    throw ex;
                }
                finally
                {

                }
            }

            string strSql = "select * from SYOperator where OperatorCode = '" + txtCode.Text.Trim() + "' and PassWord = '" + txtPwd.Text.Trim() + "'";

            try
            {
                sdr = db.GetDataReader(strSql);
                sdr.Read();
                if (sdr.HasRows)
                {
                    FormMain formMain = new FormMain();
                    this.Hide();
                    PropertyClass.OperatorCode = sdr["OperatorCode"].ToString();
                    PropertyClass.OperatorName = sdr["OperatorName"].ToString();
                    PropertyClass.PassWord = sdr["PassWord"].ToString();
                    PropertyClass.IsAdmin = sdr["IsAdmin"].ToString();
                    formMain.Show();
                }
                else
                {
                    MessageBox.Show("用户编码或用户密码不正确！", "软件提示");
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
        //重置
        private void picReset_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtPwd.Text = "";
        }
    }
}
