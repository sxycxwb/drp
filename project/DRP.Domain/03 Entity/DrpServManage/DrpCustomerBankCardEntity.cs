/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;
using DRP.Domain.Entities.Auditing;

namespace DRP.Domain.Entity.DrpServManage
{
    public class CustomerBankEntity : FullAuditedEntity<string>
    {
        /// <summary>
        /// 账户名
        /// </summary>
        public string F_CustomerId { get; set; }
        /// <summary>
        /// 银行账户名
        /// </summary>
        public string F_BankAccountName { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string F_BankCardNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string F_Description { get; set; }
    }
}
