﻿using DRP.Application.DrpServManage;
using DRP.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class FeedeductionController : Controller
    {
        private FeedeductionApp feedeductionApp = new FeedeductionApp();
        // GET: UserCenter/Feededuction
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var customerId = ClientOperatorProvider.Provider.GetCurrent().UserId;
            var data = new
            {
                rows = feedeductionApp.GetList(pagination, keyword, customerId),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
    }
}