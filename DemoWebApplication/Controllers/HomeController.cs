namespace DemoWebApplication.Controllers
{
    using CustomFramework.Http;

    using CustomFramework.Mvc;
    using CustomFramework.Mvc.Attributes;

    using Services;
    using Data.Models;

    public class HomeController : Controller
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserLoggedIn())
            {
                User user = this.usersService.GetUser(this.User);

                return this.View(user.Username);
            }

            return this.View();
        }
    }
}
