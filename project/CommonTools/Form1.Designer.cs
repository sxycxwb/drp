namespace CommonTools
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtConStr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDesEncrypt = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(920, 58);
            this.btnEncrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(112, 34);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "加密";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtConStr
            // 
            this.txtConStr.Location = new System.Drawing.Point(189, 58);
            this.txtConStr.Margin = new System.Windows.Forms.Padding(4);
            this.txtConStr.Multiline = true;
            this.txtConStr.Name = "txtConStr";
            this.txtConStr.Size = new System.Drawing.Size(720, 49);
            this.txtConStr.TabIndex = 1;
            this.txtConStr.Text = "Data Source=124.193.171.214;port=3306;Initial Catalog=test_drpdb;user id=root;pas" +
    "sword=123abc..;Character Set=utf8;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "链接字符串：";
            // 
            // btnDesEncrypt
            // 
            this.btnDesEncrypt.Location = new System.Drawing.Point(1060, 58);
            this.btnDesEncrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnDesEncrypt.Name = "btnDesEncrypt";
            this.btnDesEncrypt.Size = new System.Drawing.Size(112, 34);
            this.btnDesEncrypt.TabIndex = 3;
            this.btnDesEncrypt.Text = "解密";
            this.btnDesEncrypt.UseVisualStyleBackColor = true;
            this.btnDesEncrypt.Click += new System.EventHandler(this.btnDesEncrypt_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(189, 155);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(983, 266);
            this.txtResult.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 596);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnDesEncrypt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConStr);
            this.Controls.Add(this.btnEncrypt);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtConStr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDesEncrypt;
        private System.Windows.Forms.TextBox txtResult;
    }
}

