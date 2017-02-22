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
    /// <summary>
    /// 收益记录
    /// </summary>
    public class ProfitRecordEntity : CreationAuditedEntity<string>
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string F_CustomerId { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string F_CustomerName { get; set; }

        /// <summary>
        /// 受益人ID
        /// </summary>
        public string F_ProfitPersonId { get; set; }
        
        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string F_ProductName { get; set; }

        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal F_ProfitAmount { get; set; }

        /// <summary>
        /// 受益人类型
        /// </summary>
        public string F_Type { get; set; }
    }
}
