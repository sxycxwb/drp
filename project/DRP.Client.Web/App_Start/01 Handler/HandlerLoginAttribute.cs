using System;
using DRP.Code;
using System.Web.Mvc;
using DRP.Application.DrpServManage;
using DRP.Domain.Entity.DrpServManage;

namespace DRP.Client.Web
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        private CustomerApp customerApp = new CustomerApp();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string sid = filterContext.Controller.ValueProvider.GetValue("sid")==null?"": filterContext.Controller.ValueProvider.GetValue("sid").AttemptedValue;
            if (Ignore == false)
            {
                return;
            }

            #region 判断sid有效性

            if (!string.IsNullOrEmpty(sid))
            {
                sid = DESEncrypt.Decrypt(sid.ToUpper());
                if (!string.IsNullOrEmpty(sid))
                {
                    string customerId = sid.Split('|')[0];
                    string roleCode = sid.Split('|')[1];
                    if (ClientOperatorProvider.Provider.GetCurrent() == null)//缓存为空则重置缓存
                    {
                        var customerEntity = customerApp.GetForm(customerId);
                        if (customerEntity != null)
                            SetClientOperatorModel(customerEntity, roleCode);
                    }
                }
            }

            #endregion

            if (ClientOperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("DRP_login_error", "overdue");
                filterContext.HttpContext.Response.Redirect("/Login/Index");
                //filterContext.HttpContext.Response.Write("<script>top.location.href = '/Login/Index';</script>");
                return;
            }
        }

        private void SetClientOperatorModel(CustomerEntity customerEntity, string roleCode = "")
        {
            if (customerEntity != null)
            {
                ClientOperatorModel operatorModel = new ClientOperatorModel();
                operatorModel.UserId = customerEntity.F_Id;
                operatorModel.UserCode = customerEntity.F_Account;
                operatorModel.UserName = customerEntity.F_CompanyName;
                operatorModel.Email = customerEntity.F_Email;
                operatorModel.RoleId = customerEntity.F_RoleId;
                operatorModel.AccountBalance = customerEntity.F_AccountBalance;
                operatorModel.MobilePhone = customerEntity.F_MobilePhone;
                operatorModel.LoginIPAddress = Net.Ip;
                operatorModel.LoginIPAddressName = ""; //Net.GetLocation(operatorModel.LoginIPAddress);
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.LoginToken = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                operatorModel.IsSystem = false;
                operatorModel.RoleCode = roleCode;
                ClientOperatorProvider.Provider.AddCurrent(operatorModel);

            }
        }
    }
}