namespace JxNet.Extensions.WebHost
{
    using Microsoft.AspNetCore.Hosting;
    using JxNet.Extensions.ApiBase;

    public class Program
    {
        public static void Main(string[] args)
        {
            ProgramBase.CreateBaseBuilder(args).UseStartup<Startup>().Build().Run();

            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    //.UseApplicationInsights()
            //    .Build();
            //host.Run();
        }
    }
}
