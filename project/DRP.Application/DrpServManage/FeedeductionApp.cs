/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;
using DRP.Domain.Entity.SystemManage;
using DRP.Domain.IRepository.SystemManage;
using DRP.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;

namespace DRP.Application.DrpServManage
{
    public class FeedeductionApp
    {
        private IFeedeductionRecordRepository service = new FeedeductionRecordRepository();


        public List<FeeDeductionRecordEntity> GetList(Pagination pagination, string keyword, string customerId)
        {
            var expression = ExtLinq.True<FeeDeductionRecordEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                //expression = expression.And(t => t.F_Account.Contains(keyword));
                //expression = expression.Or(t => t.F_AccountCode.Contains(keyword));
                //expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_CustomerId == customerId);
            return service.FindList(expression, pagination).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

    }
}
