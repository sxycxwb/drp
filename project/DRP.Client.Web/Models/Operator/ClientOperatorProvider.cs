/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using DRP.Code;

namespace DRP.Client.Web
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
