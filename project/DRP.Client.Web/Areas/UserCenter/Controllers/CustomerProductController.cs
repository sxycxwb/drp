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
        public ActionResult Default(string keyValue)
        {
            var productEntity = productApp.GetForm(keyValue);
            return View(productEntity);
        }

        // GET: UserCenter/CustomerProduct
        public ActionResult ProductInfo(string keyValue)
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
        public ActionResult StopPayProduct(string keyValue)
        {
            var customerProductEntity = customerProductApp.GetForm(keyValue);
            customerProductEntity.F_Status = 2;
            customerProductApp.UpdateForm(customerProductEntity);
            return Success("产品停用成功。");
        }
       
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult StartPayProduct(string keyValue)
        {
            var customerProductEntity = customerProductApp.GetForm(keyValue);
            var customerId = customerProductEntity.F_CustomerId;
            var productId = customerProductEntity.F_ProductId;

            var productEntity = productApp.GetForm(productId);
            ClientOperatorModel customer = ClientOperatorProvider.Provider.GetCurrent();
            if (customer.AccountBalance >= customerProductEntity.F_ChargeAmount)
            {
                scheduleTaskApp.ProfitCalculateTask(customerId, productId, productEntity.F_ChargeStyle);
                customer.AccountBalance = customer.AccountBalance - customerProductEntity.F_ChargeAmount;
                ClientOperatorProvider.Provider.AddCurrent(customer);
                return Success("产品购买成功。");
            }
            else
            {
                return Error("余额不足，请充值。");
            }

        }
    }
}