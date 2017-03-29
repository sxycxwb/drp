/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using System;
using System.Web;

namespace DRP.Code
{
    public class ClientOperatorProvider
    {
        public static ClientOperatorProvider Provider
        {
            get { return new ClientOperatorProvider(); }
        }
        private string LoginUserKey = "DRP_clientuserkey_2016";
        private string LoginProvider = Configs.GetValue("LoginProvider");

        private string SystemLoginUserKey = "DRP_systemuserkey_2016";
        private string SystemLoginProvider = Configs.GetValue("SystemLoginProvider");

        public ClientOperatorModel GetCurrent()
        {
            string key = string.Empty;
            string sid = HttpContext.Current.Request["sid"];
            string customerId = "", roleCode = "";
            if (!string.IsNullOrEmpty(sid))
            {
                sid = DESEncrypt.Decrypt(sid.ToUpper());
                if (!string.IsNullOrEmpty(sid))
                {
                    key = sid.Split('|')[0] + "|" + sid.Split('|')[1];
                    customerId = sid.Split('|')[0];
                    roleCode = sid.Split('|')[1];
                }
            }
            string url = HttpContext.Current.Request.Url.ToString();
            LoginUserKey = string.IsNullOrEmpty(key) ? LoginUserKey : (key + "_") + LoginUserKey;

            ClientOperatorModel operatorModel = new ClientOperatorModel();
            if (LoginProvider == "Cookie")
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey).ToString()).ToObject<ClientOperatorModel>();
            }
            else
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<ClientOperatorModel>();
            }
            return operatorModel;
        }

        public OperatorModel GetSystemCurrent()
        {
            OperatorModel operatorModel = new OperatorModel();
            if (SystemLoginProvider == "Cookie")
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetCookie(SystemLoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            else
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetSession(SystemLoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            return operatorModel;
        }

        public void AddCurrent(ClientOperatorModel operatorModel)
        {
            string key = operatorModel.UserId + "|" + operatorModel.RoleCode;

            if (operatorModel.RoleCode != "customer")
                LoginUserKey = key + "_" + LoginUserKey;

            if (LoginProvider == "Cookie")
            {
                WebHelper.WriteCookie(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()), 60);
            }
            else
            {
                WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()));
            }
            WebHelper.WriteCookie("DRP_mac", Md5.md5(Net.GetMacByNetworkInterface().ToJson(), 32));
            WebHelper.WriteCookie("DRP_licence", Licence.GetLicence());
        }
        public void RemoveCurrent()
        {
            string key = string.Empty;
            string sid = HttpContext.Current.Request["sid"];
            if (string.IsNullOrEmpty(sid))
                key = sid.Split('|')[0] + "|" + sid.Split('|')[1];

            LoginUserKey = string.IsNullOrEmpty(key) ? LoginUserKey : (key + "_") + LoginUserKey;

            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(LoginUserKey.Trim());
            }
            else
            {
                WebHelper.RemoveSession(LoginUserKey.Trim());
            }
        }
    }
}
