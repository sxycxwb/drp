﻿/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 计费运营系统
 * Website：
*********************************************************************************/
using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;
using System.Web.Mvc;


namespace DRP.Web.Areas.DrpServManage.Controllers
{
    public class ProfitController : ControllerBase
    {
        private ProfitApp profitApp = new ProfitApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string type,string agentId,string month)
        {
            var data = new
            {
                rows = profitApp.GetList(pagination, type, agentId, month),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTotalProfitJson(string type, string agentId, string month)
        {
            var obj = profitApp.GetTotalProfit(type, agentId, month);
            return Content(obj.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult CurrentIsSystem()
        {
            var data = new
            {
                flag = OperatorProvider.Provider.GetCurrent().IsSystem
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Withdrawals()//提现
        {
            return View();
        }

    }
}
