/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Repository.DrpServManage;

namespace DRP.Application.DrpServManage
{
    public class ProductCategoryApp
    {
        private IProductCategoryRepository service = new ProductCategoryRepository();

        public List<ProductCategoryEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public ProductCategoryEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(ProductCategoryEntity productCategoryEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                productCategoryEntity.Modify(keyValue);
                service.Update(productCategoryEntity);
            }
            else
            {
                productCategoryEntity.Create();
                service.Insert(productCategoryEntity);
            }
        }
    }
}
