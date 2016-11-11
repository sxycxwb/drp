using DRP.Application.DrpServManage;
using DRP.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class CustomerProductController : Controller
    {
        private CustomerProductApp customerProductApp = new CustomerProductApp();

        // GET: UserCenter/CustomerProduct
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson()
        {
            var customerId = ClientOperatorProvider.Provider.GetCurrent().UserId;
            var data = customerProductApp.GetProductJson(customerId);
            return Content(data.ToJson());
        }
    }
}