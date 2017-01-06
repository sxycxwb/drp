/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 计费运营系统
 * Website：
*********************************************************************************/
using DRP.Domain.Entity.SystemSecurity;
using DRP.Application.SystemSecurity;
using System;
using System.Web.Mvc;
using DRP.Code;
using DRP.Application;
using DRP.Application.DrpServManage;
using DRP.Application.SystemManage;
using DRP.Domain.Entity.DrpServManage;
using DRP.Domain.IRepository.DrpServManage;
using DRP.Repository.DrpServManage;

namespace DRP.Client.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            //var test = string.Format("{0:E2}", 1);
            return View();
        }
        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public ActionResult OutLogin()
        {
            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleName = "系统登录",
                F_Type = DbLogType.Exit.ToString(),
                F_Account = ClientOperatorProvider.Provider.GetCurrent().UserCode,
                F_NickName = ClientOperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = "安全退出系统",
            });
            Session.Abandon();
            Session.Clear();
            ClientOperatorProvider.Provider.RemoveCurrent();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckLogin(string username, string password, string code)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = "用户中心登录";
            logEntity.F_Type = DbLogType.Login.ToString();
            try
            {
                if (Session["DRP_session_verifycode"].IsEmpty() || Md5.md5(code.ToLower(), 16) != Session["DRP_session_verifycode"].ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }

                CustomerEntity customerEntity = new CustomerApp().CheckLogin(username, password);
                SetClientOperatorModel(customerEntity);
                if (customerEntity != null)
                {

                    logEntity.F_Account = customerEntity.F_Account;
                    logEntity.F_NickName = customerEntity.F_CompanyName;
                    logEntity.F_Result = true;
                    logEntity.F_Description = "客户登录用户中心成功";
                    new LogApp().WriteDbLog(logEntity);
                }
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.F_Account = username;
                logEntity.F_NickName = username;
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        private RoleAuthorizeApp roleAuthorizeApp = new RoleAuthorizeApp();
        private CustomerApp customerApp = new CustomerApp();
        /// <summary>
        /// 系统后台跳转登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemUserLogin(string key)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = "系统后台管理员用户登录用户中心";
            logEntity.F_Type = DbLogType.Login.ToString();
            #region 判断后台操作用户权限
            var systemUser = ClientOperatorProvider.Provider.GetSystemCurrent();
            //判断是否是后台用户跳转过来的
            if (systemUser == null)
                throw new Exception("非法请求！");
            //判断后台用户有没有权限查看用户中心页面，有则跳转，没有则提示错误信息 
            if (!roleAuthorizeApp.SystemUserToUseCenterValidate(systemUser.RoleId))
                throw new Exception("没有请求权限！");
            #endregion

            var customerId = DESEncrypt.Decrypt(key);
            var customerEntity = customerApp.GetForm(customerId);
            SetClientOperatorModel(customerEntity);
            //记录日志
            if (customerEntity != null)
            {
                logEntity.F_Account = systemUser.UserCode;
                logEntity.F_NickName = systemUser.UserName;
                logEntity.F_Result = true;
                logEntity.F_Description = "系统后台管理员登录用户中心成功";
                new LogApp().WriteDbLog(logEntity);
            }
            return RedirectToAction("Index", "Home");
        }

        private void SetClientOperatorModel(CustomerEntity customerEntity)
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
                ClientOperatorProvider.Provider.AddCurrent(operatorModel);

            }
        }
    }
}
