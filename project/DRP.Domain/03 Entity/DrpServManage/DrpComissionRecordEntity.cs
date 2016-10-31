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
    public class DrpComissionRecordEntity : CreationAuditedEntity<string>
    {
        /// <summary>
        /// 受益人ID
        /// </summary>
        public string F_CommissionPersonId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal F_CommissionAmount { get; set; }
    }
}
