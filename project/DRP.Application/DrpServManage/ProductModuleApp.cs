/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.SystemManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Application.DrpServManage
{
    public class ProductModuleApp
    {
        private IProductModuleRepository service = new ProductModuleRepository();

        public List<ProductModuleEntity> GetList(string productId = "")
        {
            var expression = ExtLinq.True<ProductModuleEntity>();
            if (!string.IsNullOrEmpty(productId))
            {
                expression = expression.And(t => t.F_ProductId == productId);
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            return service.IQueryable(expression).OrderByDescending(t => t.F_CreatorTime).ToList();
        }
        public ProductModuleEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            var productModuleEntity = new ProductModuleEntity();
            productModuleEntity.Modify(keyValue);
            productModuleEntity.F_DeleteMark = true;
            productModuleEntity.F_Id = keyValue;
            service.Update(productModuleEntity);
        }
        public void SubmitForm(ProductModuleEntity productModuleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                productModuleEntity.Modify(keyValue);
                service.Update(productModuleEntity);
            }
            else
            {
                productModuleEntity.F_ParentId = "0";
                productModuleEntity.Create();
                service.Insert(productModuleEntity);
            }
        }
    }
}
