/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;

namespace DRP.Code
{
    public class ClientOperatorModel
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal? AccountBalance { get; set; }
        /// <summary>
        /// 用户角色ID
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        public string UserPwd { get; set; }
        public string LoginIPAddress { get; set; }
        public string LoginIPAddressName { get; set; }
        public string LoginToken { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsSystem { get; set; }
    }
}
