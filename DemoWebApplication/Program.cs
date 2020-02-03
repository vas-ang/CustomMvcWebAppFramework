namespace DemoWebApplication
{
    using System.Threading.Tasks;

    using CustomFramework.Mvc;

    class Program
    {
        static async Task Main()
        {
            await WebHost.StartAsync(new Startup());
        }
    }
}