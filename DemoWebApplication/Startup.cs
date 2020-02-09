namespace DemoWebApplication
{
    using System.Collections.Generic;

    using CustomFramework.Http;

    using CustomFramework.Mvc.Contracts;

    using Data;
    using Services;
    using Services.Implementations;

    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<IProblemsService, ProblemsService>();
        }

        public void Configure(ICollection<HttpRoute> routeTable)
        {
            using var db = new DemoDbContext();
            db.Database.EnsureCreated();
        }
    }
}
