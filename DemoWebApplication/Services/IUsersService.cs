namespace DemoWebApplication.Services
{
    using Data.Models;

    public interface IUsersService
    {
        void CreateUser(string username, string password);

        string GetUserId(string username, string password);

        User GetUser(string id);
    }
}
