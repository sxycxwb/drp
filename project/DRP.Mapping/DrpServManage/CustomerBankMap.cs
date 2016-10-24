﻿/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.DrpServManage;
using System.Data.Entity.ModelConfiguration;

namespace DRP.Mapping.DrpServManage
{
    public class CustomerBankMap : EntityTypeConfiguration<CustomerBankEntity>
    {
        public CustomerBankMap()
        {
            this.ToTable("Drp_CustomerBank");
            this.HasKey(t => t.F_Id);
        }
    }
}