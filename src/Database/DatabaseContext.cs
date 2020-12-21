using System;
using Microsoft.EntityFrameworkCore;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Database
{
    ///<summary>Context for the database containing the songs.</summary>
    public class DatabaseContext : DbContext
    {
        ///<summary>The songs in the database.</summary>
        public DbSet<Song> Songs { get; set; }

        ///<summary>Sets the settings for the database connection.</summary>
        ///<param name="optionsBuilder">Builder for the options of the DbContext.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var db = Environment.GetEnvironmentVariable("DB_DATABASE");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            optionsBuilder.UseNpgsql($"Host={host};Database={db};Username={user};Password={password}");
        }
    }
}