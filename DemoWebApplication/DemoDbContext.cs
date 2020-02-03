﻿using Microsoft.EntityFrameworkCore;

using DemoWebApplication.Models;

namespace DemoWebApplication
{
    class DemoDbContext : DbContext
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=DemoDb;Integrated Security=True;";

        public DbSet<User> Users { get; set; }

        public DbSet<Problem> Problems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
