/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace DRP.Mapping.DrpServManage
{
    public class ProductCategoryMap : EntityTypeConfiguration<ItemsEntity>
    {
        public ProductCategoryMap()
        {
            this.ToTable("Drp_productcategory");
            this.HasKey(t => t.F_Id);
        }
    }
}
