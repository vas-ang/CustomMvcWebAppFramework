using System.Threading.Tasks;
using System.Collections.Generic;

using CustomFramework.Http;
using CustomFramework.Http.Contracts;
using CustomFramework.Http.Enumerators;

namespace DemoWebApplication
{
    class Program
    {
        static async Task Main()
        {
            List<HttpRoute> httpRoutes = new List<HttpRoute>();

            httpRoutes.Add(new HttpRoute(HttpMethod.Get, "/", Index));

            IServerEntity server = new HttpServer(80, httpRoutes);

            await server.StartAsync();
        }

        static HttpResponse Index(HttpRequest request)
        {
            return new HttpResponse(HttpVersion.Http10, HttpResponseCode.Ok);
        }
    }
}
