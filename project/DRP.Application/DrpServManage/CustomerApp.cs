/*******************************************************************************
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

namespace DRP.Application.DrpServManage
{
    public class CustomerApp
    {
        private ICustomerRepository service = new CustomerRepository();

        public List<CustomerEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CustomerEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_AccountCode.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t=>t.F_DeleteMark == false);
            return service.FindList(expression, pagination);
        }
        public CustomerEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                customerEntity.Modify(keyValue);
            }
            else
            {
                customerEntity.Create();             
            }
            service.SubmitForm(customerEntity, keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        
        public void UpdateForm(CustomerEntity customerEntity)
        {
            service.Update(customerEntity);
        }
    }
}
