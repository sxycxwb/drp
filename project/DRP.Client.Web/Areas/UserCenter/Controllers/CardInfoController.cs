using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DRP.Domain.Entity.DrpServManage;
using DRP.Application.DrpServManage;
using Newtonsoft.Json;
using DRP.Code;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class CardInfoController : ControllerBase
    {
        private CustomerBankApp customerBanApp = new CustomerBankApp();
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CustomerBankEntity customerBankEntity, string keyValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyValue))
                {
                    var cardEntity = customerBanApp.SeleceForm(customerBankEntity.F_BankAccountName);
                    if (cardEntity != null && !string.IsNullOrWhiteSpace(cardEntity.F_Id))
                    {
                        return Warning("该银行账户已经存在！");
                    }
                    else
                    {
                        customerBankEntity.F_CustomerId = ClientOperatorProvider.Provider.GetCurrent().UserId;
                        customerBanApp.SubmitForm(customerBankEntity);
                        return Success("绑定银行卡成功！");
                    }
                }
                else
                {
                    var cardEntity = customerBanApp.SeleceForm(customerBankEntity.F_BankAccountName);
                    cardEntity.F_BankAccountName = customerBankEntity.F_BankAccountName;
                    cardEntity.F_BankCardNo = customerBankEntity.F_BankCardNo;
                    cardEntity.F_Description = customerBankEntity.F_Description;
                    customerBanApp.UpdateForm(cardEntity, keyValue);
                    return Success("编辑银行卡成功！");
                }

            }
            catch (Exception ex)
            {
                Logger.Error("CardInfoController.SubmitForm绑定银行卡失败!参数customerBankEntity:" + JsonConvert.SerializeObject(customerBankEntity)
                    + "|||keyValue:" + keyValue + "||||异常：" + ex.Message + "||" + ex.StackTrace);
                return Error((string.IsNullOrWhiteSpace(keyValue) ? "绑定" : "编辑") + "银行卡失败！请重试！");
            }


        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = customerBanApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var customerId = ClientOperatorProvider.Provider.GetCurrent().UserId;
            var data = new
            {
                rows = customerBanApp.GetList(pagination, keyword, customerId),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            customerBanApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}