/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 计费运营系统
 * Website：
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using System.Web.Mvc;


namespace DRP.Web.Areas.DrpServManage.Controllers
{
    public class CustomerController : ControllerBase
    {
        private CustomerApp customerApp = new CustomerApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = customerApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCustoemrJson(string name)
        {
            var rows = customerApp.GetList(name);
            var arr = new ArrayList();
            rows.ForEach(t => { arr.Add(new { id = t.F_Id, text = t.F_CompanyName }); });
            var data = arr;
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult RedictUserCenter(string keyValue)
        {
            string key = DESEncrypt.Encrypt(keyValue);
            string customerCenterUrl = Configs.GetValue("CustomerCenterUrl");
            return Redirect(customerCenterUrl + "?key=" + key);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = customerApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            customerApp.SubmitForm(customerEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitProduct(string productIds, string keyValue)
        {
            customerApp.SubmitProduct(productIds.Split(','), keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            customerApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Product()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProductJson(string keyValue)
        {
            var data = customerApp.GetProductJson(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SetProduct()
        {
            return Success("操作成功。");
        }


    }
}
