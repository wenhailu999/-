namespace ERP
{
    partial class Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.errInfo = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.picLogin = new System.Windows.Forms.PictureBox();
            this.picReset = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.errInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReset)).BeginInit();
            this.SuspendLayout();
            // 
            // errInfo
            // 
            this.errInfo.ContainerControl = this;
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPwd.Location = new System.Drawing.Point(151, 184);
            this.txtPwd.MaxLength = 20;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(179, 14);
            this.txtPwd.TabIndex = 2;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwd_KeyDown);
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCode.Location = new System.Drawing.Point(151, 146);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(179, 14);
            this.txtCode.TabIndex = 1;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // picLogin
            // 
            this.picLogin.BackColor = System.Drawing.Color.Transparent;
            this.picLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLogin.Location = new System.Drawing.Point(160, 218);
            this.picLogin.Name = "picLogin";
            this.picLogin.Size = new System.Drawing.Size(74, 30);
            this.picLogin.TabIndex = 4;
            this.picLogin.TabStop = false;
            this.picLogin.Click += new System.EventHandler(this.picLogin_Click);
            // 
            // picReset
            // 
            this.picReset.BackColor = System.Drawing.Color.Transparent;
            this.picReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picReset.Location = new System.Drawing.Point(249, 218);
            this.picReset.Name = "picReset";
            this.picReset.Size = new System.Drawing.Size(72, 30);
            this.picReset.TabIndex = 5;
            this.picReset.TabStop = false;
            this.picReset.Click += new System.EventHandler(this.picReset_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(422, 260);
            this.Controls.Add(this.picReset);
            this.Controls.Add(this.picLogin);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtPwd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            ((System.ComponentModel.ISupportInitialize)(this.errInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errInfo;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.PictureBox picLogin;
        private System.Windows.Forms.PictureBox picReset;
    }
}

