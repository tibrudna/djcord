using System;
using Microsoft.EntityFrameworkCore;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Dao
{
    /// <summary>Connector between the database and the Programm.</summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>Contains the songs in the database.</summary>
        public DbSet<Song> songs { get; set; }

        /// <summary>Configuration for the DbContext.</summary>
        /// <param name="optionsBuilder">Builder for the DbContext options.</param>
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