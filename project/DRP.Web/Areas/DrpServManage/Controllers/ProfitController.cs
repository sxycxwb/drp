/*******************************************************************************
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
using DRP.Application.SystemManage;


namespace DRP.Web.Areas.DrpServManage.Controllers
{
    public class ProfitController : ControllerBase
    {
        private ProfitApp profitApp = new ProfitApp();
        private UserApp userApp = new UserApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string type, string agentId, string month)
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
        [HandlerAjaxOnly]
        public ActionResult GetCurrentBalance()
        {
            string currentId = OperatorProvider.Provider.GetCurrent().UserId;
            var user = userApp.GetForm(currentId);
            var userBalance = user == null ? 0 : user.F_AccountBalance;
            var data = new { balance = userBalance };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Withdrawals()//提现
        {
            return View();
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridWithdrawalsJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = profitApp.GetWithdrawalsList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetWithdrawalsFormJson(string keyValue)
        {
            var data = profitApp.GetWithdrawalsForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult WithdrawalsForm()//申请提现
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult WithdrawSubmitForm(WithDrawalsRecordEntity withDrawalsRecord)
        {
            profitApp.WithdrawSubmitForm(withDrawalsRecord);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult WithdrawalsDetails()//提现详情
        {
            return View();
        }

    }
}
