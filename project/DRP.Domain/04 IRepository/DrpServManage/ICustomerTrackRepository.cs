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
    public interface ICustomerTrackRepository : IRepositoryBase<CustomerTrackEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomerTrackEntity customerEntity, string keyValue);
    }
}
