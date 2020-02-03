namespace CustomFramework.Mvc
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using CustomFramework.Http;
    using CustomFramework.Mvc.Contracts;

    public class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<HttpRoute>();
            application.ConfigureServices();
            application.Configure(routeTable);

            var httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsync();
        }
    }
}
