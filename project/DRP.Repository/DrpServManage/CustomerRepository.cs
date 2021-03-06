﻿/*******************************************************************************
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
using DRP.Domain;

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
                    customerEntity.F_Password = null;
                    db.Update(customerEntity);
                }
                else
                {
                    #region 处理顾客新增时的属性
                    customerEntity.F_DeleteMark = false;
                    //F_Password
                    customerEntity.F_Password = Md5.md5(DESEncrypt.Encrypt(customerEntity.F_Password, ConstantUtility.CUSTOMER_MD5_SECRETKEY).ToLower(), 32).ToLower();
                    //F_AccountCode 00100825114540129865
                    customerEntity.F_AccountCode = string.Format("N{0}{1}","00",Common.CreateNo("yyMMddHHmmssff"));
                    #endregion
                    db.Insert(customerEntity);
                }
                db.Commit();
            }
        }
    }
}
