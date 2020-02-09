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

            IServiceCollection serviceCollection = new ServiceCollection();

            application.ConfigureServices(serviceCollection);

            application.Configure(routeTable);

            GenerateRouteTable(routeTable, serviceCollection);

            HttpServer httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsync();
        }

        public static void GenerateRouteTable(ICollection<HttpRoute> routeTable, IServiceCollection serviceCollection)
        {
            GenerateStaticFileRoutes(routeTable);

            GeneratePageRoutes(routeTable, serviceCollection);
        }

        private static void GeneratePageRoutes(ICollection<HttpRoute> routeTable, IServiceCollection serviceCollection)
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

                Controller controllerInstance = serviceCollection.CreateInstance(controller) as Controller;

                foreach (var method in methods)
                {
                    HttpMethodAttribute httpMethodAttribute = method.GetCustomAttributes()
                        .FirstOrDefault(a => a is HttpMethodAttribute) as HttpMethodAttribute;

                    if (httpMethodAttribute != null)
                    {
                        string path = string.IsNullOrEmpty(httpMethodAttribute.Path) ?
                            $"/{controllerRouteName}/{method.Name}" :
                            httpMethodAttribute.Path;

                        HttpRoute route = new HttpRoute(httpMethodAttribute.HttpMethod, path, CreateActionResult(method, controllerInstance, serviceCollection));

                        routeTable.Add(route);
                    }
                    else
                    {
                        HttpRoute route = new HttpRoute(HttpMethod.Get, $"/{controllerRouteName}/{method.Name}", CreateActionResult(method, controllerInstance, serviceCollection));

                        routeTable.Add(route);
                    }
                }
            }
        }

        private static Func<HttpRequest, HttpResponse> CreateActionResult(MethodInfo method, Controller controllerInstance, IServiceCollection serviceCollection)
        {
            return (request) =>
            {
                controllerInstance.Request = request;

                List<object> parameters = GetParameters(method, controllerInstance, serviceCollection);

                return method.Invoke(controllerInstance, parameters.ToArray()) as HttpResponse;
            };
        }

        private static List<object> GetParameters(MethodInfo method, Controller controllerInstance, IServiceCollection serviceCollection)
        {
            var parameters = new List<object>();

            var parameterInfos = method.GetParameters();

            foreach (var parameter in parameterInfos)
            {
                var parameterType = parameter.ParameterType;
                object obj;

                if (controllerInstance.Request.FormData.ContainsKey(parameter.Name))
                {
                    obj = Convert.ChangeType(controllerInstance.Request.FormData[parameter.Name], parameterType);
                }
                else
                {
                    var properties = parameterType.GetProperties();

                    obj = serviceCollection.CreateInstance(parameter.ParameterType);

                    foreach (var property in properties)
                    {
                        property.SetValue(obj, Convert.ChangeType(controllerInstance.Request.FormData[property.Name], property.PropertyType));
                    }
                }

                parameters.Add(obj);
            }

            return parameters;
        }

        private static void GenerateStaticFileRoutes(ICollection<HttpRoute> routeTable)
        {
            const string staitcFilesFolder = "wwwroot";

            string[] directoryPaths = Directory.GetFiles(staitcFilesFolder, "*", SearchOption.AllDirectories);

            foreach (var directoryPath in directoryPaths)
            {
                byte[] file = File.ReadAllBytes(directoryPath);

                string requestPath = directoryPath.Replace(staitcFilesFolder, string.Empty).Replace("\\", "/");

                HttpRoute route = new HttpRoute(HttpMethod.Get, requestPath, CreateStaticFileAction(requestPath, file));

                routeTable.Add(route);
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

            return (httpRequest) => new FileResponse(HttpResponseCode.Ok, file, mimeType);
        }
    }
}
