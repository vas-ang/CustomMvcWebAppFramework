namespace DemoWebApplication.Controllers
{
    using CustomFramework.Http;

    using CustomFramework.Mvc;
    using CustomFramework.Mvc.Attributes;

    using Services;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            string id = this.usersService.GetUserId(username, password);

            if (id == null)
            {
                return this.Error("Invalid username/password.");
            }

            this.SignIn(id);

            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return this.Error("Invalid username/password!");
            }

            this.usersService.CreateUser(username, password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SignOut();

            return this.Redirect("/");
        }
    }
}
