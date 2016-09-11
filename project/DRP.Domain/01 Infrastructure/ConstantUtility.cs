using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRP.Domain
{
    public class ConstantUtility
    {
        /// <summary>
        /// 客户密码加密盐值
        /// </summary>
        public const string CUSTOMER_MD5_SECRETKEY = "DRP";

        /// <summary>
        /// 管理员账户名
        /// </summary>
        public const string ADMIN_CODE = "admin";
    }
}
