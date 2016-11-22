using System;
using Hangfire;
using Microsoft.Owin;
using Owin;
using Hangfire.MySql;

[assembly: OwinStartup(typeof(DRP.Web.Startup))]

namespace DRP.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            //指定Hangfire使用内存存储后台任务信息 
            GlobalConfiguration.Configuration.UseStorage(new MySqlStorage("Data Source=124.193.171.214;port=3306;Initial Catalog=test_hangfiremonitor;user id=root;password=123abc..;Allow User Variables=True;"));
            //启用HangfireServer这个中间件（它会自动释放）

            ////使用内存，每次重启站点内容会丢失
            //GlobalConfiguration.Configuration.UseMemoryStorage();
            app.UseHangfireServer();
            //启用Hangfire的仪表盘（可以看到任务的状态，进度等信息）
            // Map Dashboard to the `http://<your-app>/hangfire` URL.
            app.UseHangfireDashboard("/jobs");
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromDays(1));
            RecurringJob.AddOrUpdate(() => Console.Write("Recurring"), Cron.Daily);
            // Hangfire.MySql.MySqlStorageConnection
        }
    }
}
