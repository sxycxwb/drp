using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain.Entity.DrpServManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class CustomerProductController : ControllerBase
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

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult StopPayProduct(string keyValue)
        {
            CustomerProductEntity customerProductEntity = new CustomerProductEntity();
            customerProductEntity.F_Id = keyValue;
            customerProductEntity.F_Status = 2;
            customerProductApp.UpdateForm(customerProductEntity);
            return Success("产品停用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult StartPayProduct(string keyValue)
        {
            CustomerProductEntity customerProductEntity = new CustomerProductEntity();
            customerProductEntity.F_Id = keyValue;
            customerProductEntity.F_Status = 1;
            customerProductApp.UpdateForm(customerProductEntity);
            return Success("产品购买成功。");
        }
    }
}