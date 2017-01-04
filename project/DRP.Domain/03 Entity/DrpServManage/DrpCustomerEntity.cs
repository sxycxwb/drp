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
    public class CustomerEntity : FullAuditedEntity<string>
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public string F_AccountCode { get; set; }
        /// <summary>
        /// 登录账户名
        /// </summary>
        public string F_Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string F_Password { get; set; }
        /// <summary>
        /// 公司姓名
        /// </summary>
        public string F_CompanyName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string F_MobilePhone { get; set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public string F_RoleId { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string F_Email { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string F_LinkPerson { get; set; }
        /// <summary>
        /// 所属人ID
        /// </summary>
        public string F_BelongPersonId { get; set; }
        /// <summary>
        /// 所属人姓名
        /// </summary>
        public string F_BelongPersonName { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal? F_AccountBalance { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 有效标识
        /// </summary>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 公司简介
        /// </summary>
        public string F_Description { get; set; }    
    }
}
