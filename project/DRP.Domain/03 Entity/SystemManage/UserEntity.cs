/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;

namespace DRP.Domain.Entity.SystemManage
{
    public class UserEntity : IEntity<UserEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        /// <summary>
        /// 登录账户名
        /// </summary>
        public string F_Account { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string F_RealName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string F_HeadIcon { get; set; }
        public bool? F_Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? F_Birthday { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string F_MobilePhone { get; set; }
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string F_Email { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string F_LinkPerson { get; set; }
        public string F_OrganizeId { get; set; }
        public string F_DepartmentId { get; set; }
        public string F_RoleId { get; set; }
        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool? F_IsAdministrator { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
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
