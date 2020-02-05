namespace CustomFramework.Mvc
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using CustomFramework.Http;
    using CustomFramework.Http.Elements;
    using CustomFramework.Http.Responses;

    using Contracts;
    using Attributes;

    public class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<HttpRoute>();
            application.ConfigureServices();

            application.Configure(routeTable);

            GenerateRouteTable(routeTable);

            HttpServer httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsync();
        }

        public static void GenerateRouteTable(ICollection<HttpRoute> routeTable)
        {
            GenerateStaticFileRoutes(routeTable);

            GeneratePageRoutes(routeTable);
        }

        private static void GeneratePageRoutes(ICollection<HttpRoute> routeTable)
        {
            var controllers = Assembly.GetEntryAssembly()
                            .GetTypes()
                            .Where(t => t.IsSubclassOf(typeof(Controller)))
                            .ToArray();

            foreach (var controller in controllers)
            {
                string controllerFullName = controller.Name;

                int endOfControllerName = controllerFullName.IndexOf("Controller");

                string controllerRouteName = controllerFullName.Substring(0, endOfControllerName);

                var methods = controller.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => !m.IsSpecialName && !m.IsConstructor && !m.IsVirtual)
                    .ToArray();

                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes();

                    var methodName = method.Name;

                    Controller controllerInstance = Activator.CreateInstance(controller) as Controller;

                    if (attributes.Any(a => a is HttpMethodAttribute))
                    {
                        var httpMethodAttribute = attributes.First(a => a is HttpMethodAttribute) as HttpMethodAttribute;

                        routeTable.Add(new HttpRoute(httpMethodAttribute.HttpMethod, string.IsNullOrEmpty(httpMethodAttribute.Path) ? $"/{controllerRouteName}/{methodName}" : httpMethodAttribute.Path, CreateActionResult(method, controllerInstance)));
                    }
                    else
                    {
                        routeTable.Add(new HttpRoute(HttpMethod.Get, $"/{controllerRouteName}/{methodName}", CreateActionResult(method, controllerInstance)));
                    }
                }
            }
        }

        private static Func<HttpRequest, HttpResponse> CreateActionResult(MethodInfo method, Controller controllerInstance)
        {
            return (request) =>
            {
                controllerInstance.Request = request;

                return method.Invoke(controllerInstance, new object[] { }) as HttpResponse;
            };
        }

        private static void GenerateStaticFileRoutes(ICollection<HttpRoute> routeTable)
        {
            const string staitcFilesFolder = "wwwroot";
            string[] paths = Directory.GetFiles(staitcFilesFolder, "*", SearchOption.AllDirectories);

            foreach (var path in paths)
            {
                byte[] file = File.ReadAllBytes(path);

                routeTable.Add(new HttpRoute(HttpMethod.Get, path.Replace("wwwroot", string.Empty).Replace("\\", "/"), CreateStaticFileAction(path, file)));
            }
        }

        private static Func<HttpRequest, HttpResponse> CreateStaticFileAction(string path, byte[] file)
        {
            string mimeType = Path.GetExtension(path) switch
            {
                ".html" => "text/html",
                ".js" => "text/javascript",
                ".css" => "text/css",
                ".jpeg" => "image/jpeg",
                ".jpg" => "image/jpeg",
                ".ico" => "image/x-icon",
                _ => "text/txt",
            };

            return (httpRequest) => new FileResponse(file, mimeType);
        }
    }
}
