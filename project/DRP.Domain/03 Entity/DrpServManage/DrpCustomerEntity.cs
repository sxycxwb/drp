/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;

namespace DRP.Domain.Entity.DrpServManage
{
    public class CustomerEntity : IEntity<CustomerEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public string F_Id { get; set; }
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
        /// 排序码
        /// </summary>
        public int? F_SortCode { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 有效标识
        /// </summary>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 公司简介
        /// </summary>
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除用户
        /// </summary>
        public string F_DeleteUserId { get; set; }
    }
}
