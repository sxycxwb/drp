/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 计费运营系统
 * Website：
*********************************************************************************/

using System.Collections.Generic;
using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using System.Web.Mvc;
using DRP.Domain.Entity.SystemManage;


namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class CustomerTrackController : ControllerBase
    {
        private CustomerTrackApp customerTrackApp = new CustomerTrackApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = customerTrackApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = customerTrackApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCurrentUser(string enCode)
        {
            List<object> list = new List<object>();

            list.Add(new { id = ClientOperatorProvider.Provider.GetCurrent().UserCode, text = ClientOperatorProvider.Provider.GetCurrent().UserName });
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(CustomerTrackEntity customerTrackEntity, string keyValue)
        {
            customerTrackApp.SubmitForm(customerTrackEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            customerTrackApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}
