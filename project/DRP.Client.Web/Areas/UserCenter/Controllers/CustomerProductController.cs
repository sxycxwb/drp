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
        private ProductApp productApp = new ProductApp();
        private ScheduleTaskApp scheduleTaskApp = new ScheduleTaskApp();

        // GET: UserCenter/CustomerProduct
        public ActionResult Index(string keyValue)
        {
            var productEntity = productApp.GetForm(keyValue);
            return View(productEntity);
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
            var customerProductEntity = customerProductApp.GetForm(keyValue);
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
            var customerProductEntity = customerProductApp.GetForm(keyValue);
            var customerId = customerProductEntity.F_CustomerId;
            var productId = customerProductEntity.F_ProductId;

            var productEntity = productApp.GetForm(productId);
            if (ClientOperatorProvider.Provider.GetCurrent().AccountBalance>= customerProductEntity.F_ChargeAmount)
            {
                scheduleTaskApp.ProfitCalculateTask(customerId, productId, productEntity.F_ChargeStyle);
                return Success("产品购买成功。");
            }
            else
            {
                return Error("余额不足，请充值。");
            }

        }
    }
}