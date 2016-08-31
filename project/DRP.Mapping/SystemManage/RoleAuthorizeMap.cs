/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace DRP.Mapping.SystemManage
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<RoleAuthorizeEntity>
    {
        public RoleAuthorizeMap()
        {
            this.ToTable("Sys_RoleAuthorize");
            this.HasKey(t => t.F_Id);
        }
    }
}
