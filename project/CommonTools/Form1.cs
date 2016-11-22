using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DRP.Code;

namespace CommonTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string connStr = txtConStr.Text.Trim();
            txtResult.Text = DESEncrypt.Encrypt(connStr);
        }

        private void btnDesEncrypt_Click(object sender, EventArgs e)
        {
            string connStr = txtConStr.Text.Trim();
            txtResult.Text = DESEncrypt.Decrypt(connStr);
        }
    }
}
