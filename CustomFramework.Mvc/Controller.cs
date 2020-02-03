namespace CustomFramework.Mvc
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using CustomFramework.Http;
    using CustomFramework.Http.Responses;

    public class Controller
    {
        public HttpResponse View([CallerMemberName]string viewName = null)
        {
            string layout = File.ReadAllText(Path.Combine("Views", "Shared", "_Layout.html"));
            string controllerFullName = this.GetType().Name;
            string controllerName = controllerFullName.Substring(0, controllerFullName.Length - 10);
            string mainBody = File.ReadAllText(Path.Combine("Views", controllerName, string.Concat(viewName, ".html")));
            string fullBody = layout.Replace("@RenderBody()", mainBody);

            return new HtmlResponse(fullBody);
        }
    }
}
