/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;
using System.Collections.Generic;
using DRP.Code;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain;

namespace DRP.Repository.DrpServManage
{
    public class CustomerBankRepository : RepositoryBase<CustomerBankEntity>, ICustomerBankRepository
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var customerEntity = db.FindEntity<CustomerBankEntity>(keyValue);
                customerEntity.F_DeleteMark = true;
                db.Update(customerEntity);
                db.Commit();
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public CustomerBankEntity SeleceForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var customerEntity = db.FindEntity<CustomerBankEntity>(t => t.F_BankAccountName == keyValue);
                return customerEntity;
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="customerBankEntity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(CustomerBankEntity customerBankEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Insert(customerBankEntity);
                db.Commit();
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="customerBankEntity"></param>
        /// <param name="keyValue"></param>
        public void UpdateForm(CustomerBankEntity customerBankEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Update(customerBankEntity);
                db.Commit();
            }
        }
    }
}
