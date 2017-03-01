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
        private IDrpWithDrawalsRecordRepository wdService = new WithDrawalsRecordRepository();
        private IRoleRepository roleServie = new RoleRepository();

        public List<ProfitRecordEntity> GetList(Pagination pagination, string type, string agentId, string month)
        {
            var expression = ExtLinq.True<ProfitRecordEntity>();
            DateTime dt = DateTime.ParseExact(month, "yyyy-MM", System.Globalization.CultureInfo.CurrentCulture);
            DateTime firstDay = dt.AddDays(-dt.Day + 1);
            DateTime lastDay = dt.AddMonths(1).AddDays(-dt.AddMonths(1).Day);
            expression = expression.And(t => t.F_CreatorTime >= firstDay && t.F_CreatorTime <= lastDay);
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
                if (!string.IsNullOrEmpty(agentId) && type == "agent")//按代理人筛选数据
                    expression = expression.And(t => t.F_ProfitPersonId == agentId);
            }
            return service.FindList(expression, pagination).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        class Profit
        {
            public string TotalProfit { get; set; }
        }

        public object GetTotalProfit(string type, string agentId, string month)
        {
            DateTime dt = DateTime.ParseExact(month, "yyyy-MM", System.Globalization.CultureInfo.CurrentCulture);
            DateTime firstDay = dt.AddDays(-dt.Day + 1);
            DateTime lastDay = dt.AddMonths(1).AddDays(-dt.AddMonths(1).Day);
            var dbRepository = new RepositoryBase();
            var sqlStr = $"SELECT sum(F_ProfitAmount) TotalProfit from drp_profitrecord where F_type='{type}' and F_CreatorTime>='{firstDay.ToString("yyyy-MM-dd")}' and F_CreatorTime<='{lastDay.ToString("yyyy-MM-dd")}' ";
            if (!string.IsNullOrEmpty(agentId) && type == "agent") //按代理人筛选数据
                sqlStr += $"and F_ProfitPersonId='{agentId}'";

            var obj = dbRepository.FindList<Profit>(sqlStr);
            return obj;
        }


        public List<WithDrawalsRecordEntity> GetWithdrawalsList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<WithDrawalsRecordEntity>();
            return wdService.FindList(expression, pagination).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public WithDrawalsRecordEntity GetWithdrawalsForm(string keyValue)
        {
            return wdService.FindEntity(keyValue);
        }

        public void WithdrawSubmitForm(WithDrawalsRecordEntity withDrawalsRecord)
        {
            withDrawalsRecord.Create();
            var currentUser = OperatorProvider.Provider.GetCurrent();
            withDrawalsRecord.F_WithdrawPersonId = currentUser.UserId;
            withDrawalsRecord.F_WithdrawPersonName = currentUser.UserName;
            var currentRole = roleServie.FindEntity(t => t.F_Id == currentUser.RoleId);
            withDrawalsRecord.F_Type = currentRole.F_EnCode;
            withDrawalsRecord.F_Status = 1;//为申请状态
            wdService.WithdrawSubmitForm(withDrawalsRecord);
        }
    }
}
