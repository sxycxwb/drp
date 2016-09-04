using System.Web.Mvc;

namespace DRP.Web.Areas.ReportManage
{
    public class DrpServManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DrpServManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              this.AreaName + "_Default",
              this.AreaName + "/{controller}/{action}/{id}",
              new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
              new string[] { "DRP.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
