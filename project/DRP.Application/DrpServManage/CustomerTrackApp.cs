﻿/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Repository.DrpServManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain;

namespace DRP.Application.DrpServManage
{
    public class CustomerTrackApp
    {
        private ICustomerTrackRepository service = new CustomerTrackRepository();

        public List<CustomerTrackEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CustomerTrackEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_CustomerName.Contains(keyword));
                //expression = expression.Or(t => t.F_AccountCode.Contains(keyword));
                //expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark == false);
            
            if (!string.IsNullOrEmpty(ClientOperatorProvider.Provider.GetCurrent().SystemUserId)&& ClientOperatorProvider.Provider.GetCurrent().SystemUserCode!="admin")
            {
                var currentUserId = ClientOperatorProvider.Provider.GetCurrent().SystemUserId;
                expression = expression.And(t => t.F_CreatorUserId == currentUserId);
            }

            return service.FindList(expression, pagination);
        }
        public CustomerTrackEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void SubmitForm(CustomerTrackEntity customerTrackEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                customerTrackEntity.Modify(keyValue);
                if (ClientOperatorProvider.Provider.GetCurrent() != null)
                {
                    if (!string.IsNullOrEmpty(ClientOperatorProvider.Provider.GetCurrent().SystemUserId))
                    {
                        customerTrackEntity.F_LastModifyUserId =
                            ClientOperatorProvider.Provider.GetCurrent().SystemUserId;
                        customerTrackEntity.F_LastModifyUserName =
                            ClientOperatorProvider.Provider.GetCurrent().SystemUserName;
                    }
                }
            }
            else
            {
                customerTrackEntity.Create();
                if (ClientOperatorProvider.Provider.GetCurrent() != null)
                {
                    if (!string.IsNullOrEmpty(ClientOperatorProvider.Provider.GetCurrent().SystemUserId))
                    {
                        customerTrackEntity.F_CreatorUserId =
                           ClientOperatorProvider.Provider.GetCurrent().SystemUserId;
                        customerTrackEntity.F_CreatorUserName =
                            ClientOperatorProvider.Provider.GetCurrent().SystemUserName;
                    }
                }
            }
            service.SubmitForm(customerTrackEntity, keyValue);
        }


        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void UpdateForm(CustomerTrackEntity customerEntity)
        {
            service.Update(customerEntity);
        }

    }
}
