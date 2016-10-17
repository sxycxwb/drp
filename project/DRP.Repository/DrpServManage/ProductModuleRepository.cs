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
            using (var db = new RepositoryBase().BeginTrans())
            {
                var productModuleEntity = db.FindEntity<ProductModuleEntity>(t => t.F_Id == keyValue);
                var productEntity = db.FindEntity<ProductEntity>(t => t.F_Id == productModuleEntity.F_ProductId);
                productEntity.F_ChargeAmount -= productModuleEntity.F_ChargeAmount;
                productEntity.F_CostPrice -= productModuleEntity.F_CostPrice;

                productModuleEntity.Remove();
                db.Update(productModuleEntity);

                productEntity.Modify(productModuleEntity.F_ProductId);
                db.Update(productEntity);

                db.Commit();
            }
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
                    decimal chargeAmount = 0, costPrice = 0;
                    foreach (var module in productMouleList)
                    {
                        chargeAmount += module.F_ChargeAmount;
                        costPrice += module.F_CostPrice;
                    }
                    chargeAmount += productModuleEntity.F_ChargeAmount;
                    costPrice += productModuleEntity.F_CostPrice;
                    productEntity.F_ChargeAmount = chargeAmount;
                    productEntity.F_CostPrice = costPrice;
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
                    productEntity.F_CostPrice += productModuleEntity.F_CostPrice;

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
