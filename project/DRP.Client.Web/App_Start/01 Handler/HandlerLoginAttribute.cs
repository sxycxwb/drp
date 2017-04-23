using System;
using DRP.Code;
using System.Web.Mvc;
using DRP.Application.DrpServManage;
using DRP.Application.SystemManage;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.Entity.SystemManage;

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
        private UserApp userApp = new UserApp();
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
                    string customerCode = sid.Split('|')[0];
                    string roleCode = sid.Split('|')[1];
                    string systemUserCode = sid.Split('|')[2];
                    if (ClientOperatorProvider.Provider.GetCurrent() == null)//缓存为空则重置缓存
                    {
                        var customerEntity = customerApp.GetFormByCode(customerCode);
                        var userEntity = userApp.GetFormByUserCode(systemUserCode);
                        if (customerEntity != null)
                            SetClientOperatorModel(customerEntity, userEntity, roleCode);
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

        private void SetClientOperatorModel(CustomerEntity customerEntity, UserEntity systemUserEntity, string roleCode = "")
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
                operatorModel.SystemRoleCode = roleCode;

                if (systemUserEntity != null)
                {
                    operatorModel.SystemUserId = systemUserEntity.F_Id;
                    operatorModel.SystemUserCode = systemUserEntity.F_Account;
                    operatorModel.SystemUserName = systemUserEntity.F_RealName;
                }

                ClientOperatorProvider.Provider.AddCurrent(operatorModel);

            }
        }
    }
}