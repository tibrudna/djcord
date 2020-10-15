using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Models;

namespace tibrudna.djcort.tests.Services
{
    public class DatabaseFixture 
    {
        public Song Song { get; }
        public Song Song2 { get; }
        public Song Song3 { get; }
        public IQueryable<Song> Data { get; }
        public Mock<DbSet<Song>> MockSet {get; }
        public Mock<DatabaseContext> MockContext { get; }

        public DatabaseFixture()
        {
            Song = new Song { ID = "1234w", Title = "Something", Artist = "Someone" };
            Song2 = new Song { ID = "7345gt", Title = "Something", Artist = "Someone" };
            Song3 = new Song { ID = "abcdef", Title = "A song", Artist = "Someone" };

            Data = new List<Song>
            {
                Song,
                Song2,
                Song3,
            }.AsQueryable<Song>();

            MockSet = new Mock<DbSet<Song>>();
            MockSet.As<IQueryable<Song>>().SetupGet(m => m.Provider).Returns(Data.Provider);
            MockSet.As<IQueryable<Song>>().SetupGet(m => m.Expression).Returns(Data.Expression);
            MockSet.As<IQueryable<Song>>().SetupGet(m => m.ElementType).Returns(Data.ElementType);
            MockSet.As<IQueryable<Song>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());

            MockContext = new Mock<DatabaseContext>();
            MockContext.Object.songs = MockSet.Object;
        }
    }
}