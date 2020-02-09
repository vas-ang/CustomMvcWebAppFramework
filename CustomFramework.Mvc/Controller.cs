namespace CustomFramework.Mvc
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using CustomFramework.Http;
    using CustomFramework.Http.Responses;

    using Contracts;

    public abstract class Controller
    {
        private readonly IViewEngine viewEngine = new ViewEngine();

        public HttpRequest Request { get; set; }

        protected HttpResponse View(object model = null, [CallerMemberName]string viewName = null)
        {
            string layout = GetLayout();
            string mainBody = GetMainBody(viewName);

            string fullBody = layout.Replace("@RenderBody()", mainBody);

            string fullGeneratedBody = this.viewEngine.GetHtml(fullBody, model, this.User);

            return new HtmlResponse(fullGeneratedBody);
        }

        protected HttpResponse Redirect(string path)
        {
            return new RedirectResponse(path);
        }

        protected HttpResponse Error(string message, [CallerMemberName]string viewName = null)
        {
            string layout = GetLayout();
            string fullBody = layout.Replace("@RenderBody()", message);

            string fullGeneratedBody = this.viewEngine.GetHtml(fullBody, null, this.User);

            return new HtmlResponse(fullGeneratedBody);
        }

        protected bool IsUserLoggedIn()
        {
            return this.User != null;
        }

        protected void SignIn(string userId)
        {
            this.Request.SessionData["UserId"] = userId;
        }

        protected void SignOut()
        {
            this.Request.SessionData["UserId"] = null;
        }

        public string User =>
            this.Request.SessionData.ContainsKey("UserId") ?
                this.Request.SessionData["UserId"] : null;


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
