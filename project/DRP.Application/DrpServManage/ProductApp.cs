/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Application.DrpServManage
{
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();
        private IProductModuleRepository moduleService = new ProductModuleRepository();

        public List<ProductEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ProductEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                //expression = expression.And(t => t.F_Account.Contains(keyword));
                //expression = expression.Or(t => t.F_AccountCode.Contains(keyword));
                //expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            return service.FindList(expression, pagination);
        }
        public ProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (moduleService.IQueryable().Count(t => t.F_ProductId.Equals(keyValue) && (t.F_DeleteMark == false)) > 0)
            {
                throw new Exception("删除失败！该产品包含了下级模块数据！");
            }
            else
            {
                var productEntity = new ProductEntity();
                productEntity.Modify(keyValue);
                productEntity.F_DeleteMark = true;
                productEntity.F_Id = keyValue;
                service.Update(productEntity);
            }
        }
        public void SubmitForm(ProductEntity productEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (productEntity.F_ChargePattern == "0")
                {
                    if (moduleService.IQueryable().Count(t => t.F_ProductId.Equals(keyValue) && (t.F_DeleteMark == false)) > 0)
                    {
                        throw new Exception("修改失败！该产品包含了下级模块数据！");
                    }
                }
                productEntity.Modify(keyValue);
                service.Update(productEntity);
            }
            else
            {
                productEntity.Create();
                service.Insert(productEntity);
            }
        }
    }
}
