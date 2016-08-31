/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace DRP.Mapping.SystemSecurity
{
    public class LogMap : EntityTypeConfiguration<LogEntity>
    {
        public LogMap()
        {
            this.ToTable("Sys_Log");
            this.HasKey(t => t.F_Id);
        }
    }
}
