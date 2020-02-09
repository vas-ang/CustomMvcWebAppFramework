namespace DemoWebApplication.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    using Data;
    using Data.Models;

    public class UsersService : IUsersService
    {
        private readonly DemoDbContext db;

        public UsersService(DemoDbContext db)
        {
            this.db = db;
        }

        public void CreateUser(string username, string password)
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Password = Hash(password)
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = Hash(password);

            return this.db.Users
                .Where(x => x.Username == username && x.Password == passwordHash)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public User GetUser(string id)
        {
            return this.db.Users.Find(id);
        }

        private static string Hash(string input)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();

            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
