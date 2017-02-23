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
using DRP.Data;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Domain.ViewModel;

namespace DRP.Application.DrpServManage
{
    public class ProfitApp
    {
        private IProfitRecordRepository service = new ProfitRecordRepository();
        public List<ProfitRecordEntity> GetList(Pagination pagination, string type,string agentId)
        {
            var expression = ExtLinq.True<ProfitRecordEntity>();
            if (!string.IsNullOrEmpty(type))
            {
                expression = expression.And(t => t.F_Type == type);
            }
            if (!OperatorProvider.Provider.GetCurrent().IsSystem) //不是超级管理员
            {
                var currentUserId = OperatorProvider.Provider.GetCurrent().UserId;
                expression = expression.And(t => t.F_ProfitPersonId == currentUserId);
            }
            else
            {
                if (!string.IsNullOrEmpty(agentId))//按代理人筛选数据
                    expression = expression.And(t => t.F_ProfitPersonId == agentId);
            }
            return service.FindList(expression, pagination).OrderByDescending(t => t.F_CreatorTime).ToList();
        }
    }
}
