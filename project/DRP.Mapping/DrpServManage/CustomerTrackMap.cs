/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.DrpServManage;
using System.Data.Entity.ModelConfiguration;

namespace DRP.Mapping.DrpServManage
{
    public class CustomerTrackMap : EntityTypeConfiguration<CustomerTrackEntity>
    {
        public CustomerTrackMap()
        {
            this.ToTable("Drp_CustomerTrack");
            this.HasKey(t => t.F_Id);
        }
    }
}
