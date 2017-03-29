/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/

using System.Web;

namespace DRP.Code
{
    public class OperatorProvider
    {
        public static OperatorProvider Provider
        {
            get { return new OperatorProvider(); }
        }
        private string LoginUserKey = "DRP_systemuserkey_2016";
        private string LoginProvider = Configs.GetValue("LoginProvider");

        public OperatorModel GetCurrent()
        {
            OperatorModel operatorModel = new OperatorModel();
            string md5UserCode = HttpContext.Current.Request["sid"];
            LoginUserKey = md5UserCode + "_" + LoginUserKey;//md5(登录名)+LoginUserKey
            string url = HttpContext.Current.Request.Url.ToString();
            if (LoginProvider == "Cookie")
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            else
            {
                operatorModel = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<OperatorModel>();
            }
            return operatorModel;
        }
        public void AddCurrent(OperatorModel operatorModel)
        {
            LoginUserKey = Md5.md5(operatorModel.UserCode,32).ToLower() + "_"+ LoginUserKey;
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
            string md5UserCode = HttpContext.Current.Request["sid"];
            LoginUserKey = md5UserCode + "_" + LoginUserKey;//md5(登录名)+LoginUserKey
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
