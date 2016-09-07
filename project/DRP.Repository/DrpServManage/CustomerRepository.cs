/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;
using DRP.Code;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Repository.DrpServManage
{
    public class CustomerRepository : RepositoryBase<CustomerEntity>, ICustomerRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var customerEntity = db.FindEntity<CustomerEntity>(keyValue);
                customerEntity.F_DeleteMark = true;
                db.Update(customerEntity);
                db.Commit();
            }
        }

        public void SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(customerEntity);
                }
                else
                {
                    db.Insert(customerEntity);
                }
                db.Commit();
            }
        }
    }
}
