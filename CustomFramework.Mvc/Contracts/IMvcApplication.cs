namespace CustomFramework.Mvc.Contracts
{
    using System.Collections.Generic;

    using CustomFramework.Http;

    public interface IMvcApplication
    {
        void ConfigureServices(IServiceCollection serviceCollection);

        void Configure(ICollection<HttpRoute> routeTable);
    }
}
