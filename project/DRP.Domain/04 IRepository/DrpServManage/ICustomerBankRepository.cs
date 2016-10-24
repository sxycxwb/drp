/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DRP.Code;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;

namespace DRP.Domain.IRepository.DrpServManage
{
    public interface ICustomerBankRepository : IRepositoryBase<CustomerBankEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomerBankEntity customerEntity, string keyValue);
        void UpdateForm(CustomerBankEntity customerBankEntity, string keyValue);
    }
}
