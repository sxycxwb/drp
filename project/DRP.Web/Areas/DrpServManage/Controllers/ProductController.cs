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
    public class ProductController : ControllerBase
    {
        private ProductApp productApp = new ProductApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = productApp.GetList(pagination, keyword),
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
            var data = productApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProductEntity productEntity, string keyValue)
        {
            productApp.SubmitForm(productEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            productApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }          

    }
}
