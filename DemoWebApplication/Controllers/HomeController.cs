namespace DemoWebApplication.Controllers
{
    using CustomFramework.Http;

    using CustomFramework.Mvc;
    using CustomFramework.Mvc.Attributes;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            return this.View();
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse ReceiveLogin()
        {
            return this.View();
        }

        public HttpResponse Register()
        {
            return this.View();
        }
    }
}
