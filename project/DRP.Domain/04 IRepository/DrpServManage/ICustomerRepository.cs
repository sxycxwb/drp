/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/

using System.Collections.Generic;
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;

namespace DRP.Domain.IRepository.DrpServManage
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomerEntity customerEntity, string keyValue);
        void SubmitProduct(List<CustomerProductEntity> customerProductList, string keyValue);
    }
}
