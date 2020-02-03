namespace DemoWebApplication.Controllers
{
    using CustomFramework.Mvc;
    using CustomFramework.Http;

    class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Login(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Register(HttpRequest request)
        {
            return this.View();
        }
    }
}
