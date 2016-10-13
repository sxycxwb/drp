/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using System.Collections.Generic;
using System.Linq;

namespace DRP.Repository.SystemManage
{
    public class ProductModuleRepository : RepositoryBase<ProductModuleEntity>, IProductModuleRepository
    {
        public void DeleteForm(string keyValue)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitForm(ProductModuleEntity productModuleEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var productEntity = db.FindEntity<ProductEntity>(t => t.F_Id == productModuleEntity.F_ProductId);

                    #region 计算产品更新后的计费金额
                    var productMouleList = db.IQueryable<ProductModuleEntity>(t => t.F_ProductId == productModuleEntity.F_ProductId && t.F_DeleteMark == false && t.F_Id != keyValue).ToList();
                    decimal totalAmount = 0;
                    foreach (var module in productMouleList)
                    {
                        totalAmount += module.F_ChargeAmount;
                    }
                    totalAmount += productModuleEntity.F_ChargeAmount;
                    productEntity.F_ChargeAmount = totalAmount;
                    #endregion

                    productModuleEntity.Modify(keyValue);
                    productEntity.Modify(productModuleEntity.F_ProductId);

                    db.Update(productEntity);
                    db.Update(productModuleEntity);
                }
                else
                {
                    //TODO:: 此处未设置模块父级元素
                    productModuleEntity.F_ParentId = "0";

                    //插入模块信息，并更新产品计费金额数据
                    var productEntity = db.FindEntity<ProductEntity>(t => t.F_Id == productModuleEntity.F_ProductId);
                    productEntity.F_ChargeAmount += productModuleEntity.F_ChargeAmount;

                    productModuleEntity.Create();
                    productEntity.Modify(productModuleEntity.F_ProductId);

                    db.Insert(productModuleEntity);
                    db.Update(productEntity);
                }
                db.Commit();
            }
        }
    }
}
