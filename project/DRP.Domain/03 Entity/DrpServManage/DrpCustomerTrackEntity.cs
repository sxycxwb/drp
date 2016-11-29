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
    public class CustomerTrackEntity : FullAuditedEntity<string>
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string F_CustomerId { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string F_CustomerName { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string F_DeptmentId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string F_Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string F_Content { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public int F_Status { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string F_CreatorUserName { get; set; }
        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string F_LastModifyUserName { get; set; }

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
