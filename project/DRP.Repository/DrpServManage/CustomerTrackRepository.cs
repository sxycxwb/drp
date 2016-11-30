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
    public class CustomerTrackRepository : RepositoryBase<CustomerTrackEntity>, ICustomerTrackRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                var customerEntity = db.FindEntity<CustomerTrackEntity>(keyValue);
                customerEntity.F_DeleteMark = true;
                db.Update(customerEntity);
                db.Commit();
            }
        }

        public void SubmitForm(CustomerTrackEntity customerTrackEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(customerTrackEntity);
                    var LoginInfo = OperatorProvider.Provider.GetCurrent();
                    if (LoginInfo != null)
                        customerTrackEntity.F_LastModifyUserName = LoginInfo.UserName;
                }
                else
                {
                    #region 处理顾客 记事簿 新增时的属性
                    customerTrackEntity.F_DeleteMark = false;
                    var LoginInfo = OperatorProvider.Provider.GetCurrent();
                    if (LoginInfo != null)
                        customerTrackEntity.F_CreatorUserName = LoginInfo.UserName;
                    #endregion
                    db.Insert(customerTrackEntity);
                }
                db.Commit();
            }
        }

       
    }
}
