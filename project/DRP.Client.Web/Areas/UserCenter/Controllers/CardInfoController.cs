using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DRP.Domain.Entity.DrpServManage;
using DRP.Application.DrpServManage;
using Newtonsoft.Json;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class CardInfoController : ControllerBase
    {
        private CustomerBankApp cardApp = new CustomerBankApp();
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CustomerBankEntity customerBankEntity, string keyValue)
        {
            try
            {
                var cardEntity = cardApp.SeleceForm(customerBankEntity.F_BankAccountName);
                if (cardEntity != null && !string.IsNullOrWhiteSpace(cardEntity.F_Id))
                {
                    return Warning("该银行账户已经存在！");
                }
                else
                {
                    customerBankEntity.F_CustomerId = ClientOperatorProvider.Provider.GetCurrent().UserId;
                    cardApp.SubmitForm(customerBankEntity, keyValue);
                    return Success("绑定银行卡成功！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CardInfoController.SubmitForm绑定银行卡失败!参数customerBankEntity:" + JsonConvert.SerializeObject(customerBankEntity)
                    + "|||keyValue:" + keyValue + "||||异常：" + ex.Message + "||" + ex.StackTrace);
                return Error("绑定银行卡失败！请重试！");
            }


        }
    }
}