using DRP.Application.DrpServManage;
using DRP.Code;
using DRP.Domain;
using DRP.Domain.Entity.DrpServManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRP.Client.Web.Areas.UserCenter.Controllers
{
    public class AccountInfoController : ControllerBase
    {
        [HttpGet]
        public virtual ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SubmitPwd(string oldpwd, string newpwd)
        {
            string UserCode = string.Empty;
            try
            {
                UserCode = ClientOperatorProvider.Provider.GetCurrent().UserCode;
                CustomerEntity customerEntity = new CustomerApp().CheckLogin(UserCode, oldpwd);
                if (customerEntity != null)
                {
                    customerEntity.F_Password = Md5.md5(DESEncrypt.Encrypt(newpwd.ToLower(), ConstantUtility.CUSTOMER_MD5_SECRETKEY).ToLower(), 32).ToLower();
                    new CustomerApp().UpdateForm(customerEntity);
                    return Success("修改密码成功！");
                }
                else
                {
                    return Warning("原密码错误！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AccountInfoController.SubmitPwd修改密码失败!oldpwd:" + oldpwd
                                   + "|||newpwd:" + newpwd + "|||usercode:" + UserCode + "||||异常：" + ex.Message + "||" + ex.StackTrace);
                return Error(ex.Message);
            }
        }
    }
}