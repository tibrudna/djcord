using System;
using Microsoft.EntityFrameworkCore;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Dao
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Song> songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            optionsBuilder.UseNpgsql($"Host={host};Database={database};Username={user};Password={password}");
        }
    }
}