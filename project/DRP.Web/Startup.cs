using System;
using System.Configuration;
using DRP.Code;
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
            var connectString = ConfigurationManager.ConnectionStrings["DRPHangfireDbContext"].ConnectionString;
            connectString = DESEncrypt.Decrypt(connectString);
            GlobalConfiguration.Configuration.UseStorage(new MySqlStorage($"{connectString} Allow User Variables=True;",new MySqlStorageOptions()));
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
            RecurringJob.AddOrUpdate(() => Console.Write("EveryMonth Exceute!"), "1 0 1 * *");//每个月凌晨0点1分
            // Hangfire.MySql.MySqlStorageConnection
        }
    }
}
