/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: DRP
 * Description: DRP快速开发平台
 * Website：http://www.DRP.cn
*********************************************************************************/
using DRP.Application.SystemManage;
using DRP.Code;
using DRP.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace DRP.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
