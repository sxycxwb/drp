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
    public class CustomerProductEntity: CreationAuditedEntity<string>
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public string F_CustomerId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string F_ProductId { get; set; }
        /// <summary>
        /// 提成比
        /// </summary>
        public decimal F_RoyaltyRate { get; set; }
        /// <summary>
        /// 状态 1开通，0欠费，2停用
        /// </summary>
        public int F_Status { get; set; }
        /// <summary>
        /// 计费日期标识
        /// </summary>
        public string F_ChargingDateFlag { get; set; }
    }
}
