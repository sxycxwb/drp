/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: DRP
 * Description: DRP快速开发平台
 * Website：http://www.DRP.cn
*********************************************************************************/
using DRP.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRP.Web.Areas.ExampleManage.Controllers
{
    public class SendMailController : ControllerBase
    {
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SendMail(string account, string title, string content)
        {
            MailHelper mail = new MailHelper();
            mail.MailServer = Configs.GetValue("MailHost");
            mail.MailUserName = Configs.GetValue("MailUserName");
            mail.MailPassword = Configs.GetValue("MailPassword");
            mail.MailName = "DRP快速开发平台";
            mail.Send(account, title, content);
            return Success("发送成功。");
        }
    }
}
