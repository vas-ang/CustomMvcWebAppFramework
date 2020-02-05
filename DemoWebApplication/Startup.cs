namespace DemoWebApplication
{
    using System.Collections.Generic;

    using CustomFramework.Http;

    using CustomFramework.Mvc.Contracts;

    class Startup : IMvcApplication
    {
        public void ConfigureServices()
        {
            using var db = new DemoDbContext();
            db.Database.EnsureCreated();
        }

        public void Configure(ICollection<HttpRoute> routeTable)
        {
        }
    }
}
