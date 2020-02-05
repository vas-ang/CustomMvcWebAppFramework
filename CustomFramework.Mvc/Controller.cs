namespace CustomFramework.Mvc
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using CustomFramework.Http;
    using CustomFramework.Http.Responses;

    using Contracts;

    public class Controller
    {
        private readonly IViewEngine viewEngine = new ViewEngine();

        public HttpRequest Request { get; set; }

        protected HttpResponse View(object model = null, [CallerMemberName]string viewName = null)
        {
            string layout = GetLayout();
            string mainBody = GetMainBody(viewName);

            string fullBody = layout.Replace("@RenderBody()", mainBody);

            string fullGeneratedBody = this.viewEngine.GetHtml(fullBody, model);

            return new HtmlResponse(fullGeneratedBody);
        }

        private string GetMainBody(string viewName)
        {
            string viewFolder = GetViewFolder();
            string viewFile = string.Concat(viewName, ".html");

            string bodyPath = Path.Combine("Views", viewFolder, viewFile);

            string mainBody = File.ReadAllText(bodyPath);

            return mainBody;
        }

        private static string GetLayout()
        {
            string layoutPath = Path.Combine("Views", "Shared", "_Layout.html");

            string layout = File.ReadAllText(layoutPath);

            return layout;
        }

        private string GetViewFolder()
        {
            string controllerName = this.GetType().Name;

            int endOfViewFolderName = controllerName.LastIndexOf("Controller");

            string viewFolder = controllerName.Substring(0, endOfViewFolderName);

            return viewFolder;
        }
    }
}
