using Hangfire;
using Microsoft.Owin;
using Owin;
//using Hangfire.MySql;

[assembly: OwinStartup(typeof(DRP.Web.Startup))]

namespace DRP.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            //指定Hangfire使用内存存储后台任务信息
            //GlobalConfiguration.Configuration.UseStorage(new MySqlStorage("Data Source=111.203.235.204;port=3306;Initial Catalog=hangfiremonitor;user id=drp;password=484950;Allow User Variables=True;"));
            //启用HangfireServer这个中间件（它会自动释放）
            app.UseHangfireServer();
            //启用Hangfire的仪表盘（可以看到任务的状态，进度等信息）
            app.UseHangfireDashboard();

           // Hangfire.MySql.MySqlStorageConnection
        }
    }
}
