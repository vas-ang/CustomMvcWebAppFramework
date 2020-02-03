namespace CustomFramework.Mvc.Contracts
{
    using System.Collections.Generic;

    using CustomFramework.Http;

    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(IList<HttpRoute> routeTable);
    }
}
