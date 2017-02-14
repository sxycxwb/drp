using DRP.Code;
using System.Web.Mvc;

namespace DRP.Client.Web
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (ClientOperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("DRP_login_error", "overdue");
                filterContext.HttpContext.Response.Redirect("/Login/Index");
                //filterContext.HttpContext.Response.Write("<script>top.location.href = '/Login/Index';</script>");
                return;
            }
        }
    }
}