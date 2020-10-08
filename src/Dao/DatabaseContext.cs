using Microsoft.EntityFrameworkCore;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.src.Dao
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Song> songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=172.18.0.3;Database=djcord;Username=postgres;Password=something");
    }
}