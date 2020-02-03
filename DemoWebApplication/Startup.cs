namespace DemoWebApplication
{
    using System.Collections.Generic;

    using CustomFramework.Http;
    using CustomFramework.Http.Elements;

    using CustomFramework.Mvc.Contracts;

    using Controllers;

    class Startup : IMvcApplication
    {
        public void ConfigureServices()
        {
            using var db = new DemoDbContext();
            db.Database.EnsureCreated();
        }

        public void Configure(IList<HttpRoute> routeTable)
        {
            routeTable.Add(new HttpRoute(HttpMethod.Get, "/", new HomeController().Index));
            routeTable.Add(new HttpRoute(HttpMethod.Get, "/Home/Login", new HomeController().Login));
            routeTable.Add(new HttpRoute(HttpMethod.Post, "/Home/Login", new HomeController().Login));
            routeTable.Add(new HttpRoute(HttpMethod.Get, "/Home/Register", new HomeController().Register));
            routeTable.Add(new HttpRoute(HttpMethod.Post, "/Home/Login", new HomeController().Register));
        }
    }
}
