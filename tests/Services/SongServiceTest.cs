using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Exceptions;
using tibrudna.djcort.src.Models;
using tibrudna.djcort.src.Services;
using Xunit;

namespace tibrudna.djcort.tests.Services
{
    public class SongServiceTest
    {
        public class AddTest
        {
            private Song song;
            private IQueryable<Song> data;
            private Mock<DbSet<Song>> mockSet;
            private Mock<DatabaseContext> mockContext;
            private SongService songService;

            public AddTest()
            {
                song = new Song { ID = "1234w", Title = "Something", Artist = "Someone" };
                data = new List<Song>
                {
                    song,
                }.AsQueryable();

                mockSet = new Mock<DbSet<Song>>();
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<Song>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

                mockContext = new Mock<DatabaseContext>();
                mockContext.Object.songs = mockSet.Object;

                songService = new SongService(mockContext.Object);
            }

            [Fact]
            public void TestAddDuplicateID()
            {
                Assert.Throws<DuplicateSongException>(() => songService.Add(song));

                mockSet.Verify(m => m.Add(song), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);
            }

            [Fact]
            public void TestAddDuplicateTitleAndArtist()
            {
                var searchSong = new Song { ID = "qwert1", Title = "something", Artist = "someone" };

                songService.Add(searchSong);

                mockSet.Verify(m => m.Add(searchSong), Times.Once);
                mockContext.Verify(m => m.SaveChanges(), Times.Once);
            }

            [Fact]
            public void TestAddNull()
            {
                Assert.Throws<ArgumentException>(() => songService.Add(null));

                mockSet.Verify(m => m.Add(null), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);
            }

            [Fact]
            public void TestAddSongEmtpyTitle()
            {
                var newSong = new Song { ID="1234", Title=null, Artist="Someone"};
                Assert.Throws<ArgumentException>(() => songService.Add(newSong));
                mockSet.Verify(m => m.Add(null), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);

                newSong.Title = "";
                Assert.Throws<ArgumentException>(() => songService.Add(newSong));
                mockSet.Verify(m => m.Add(null), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);
                
            }

            [Fact]
            public void TestAddSongEmptyID()
            {
                var newSong = new Song { ID=null, Title="Something", Artist="Someone"};
                Assert.Throws<ArgumentException>(() => songService.Add(newSong));
                mockSet.Verify(m => m.Add(null), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);

                newSong.ID = "";
                Assert.Throws<ArgumentException>(() => songService.Add(newSong));
                mockSet.Verify(m => m.Add(null), Times.Never);
                mockContext.Verify(m => m.SaveChanges(), Times.Never);
            }
        }
        
        public class ExistsTest
        {
            private Song song;
            private IQueryable<Song> data;
            private Mock<DbSet<Song>> mockSet;
            private Mock<DatabaseContext> mockContext;
            private SongService songService;

            public ExistsTest()
            {
                song = new Song { ID = "1234w", Title = "Something", Artist = "Someone" };
                data = new List<Song>
                {
                    song,
                }.AsQueryable();

                mockSet = new Mock<DbSet<Song>>();
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<Song>>().SetupGet(m => m.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<Song>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

                mockContext = new Mock<DatabaseContext>();
                mockContext.Object.songs = mockSet.Object;

                songService = new SongService(mockContext.Object);
            }

            [Fact]
            public void TestExistsEmptyID()
            {
                string newSongID = null;
                Assert.Throws<ArgumentException>(() => songService.Exists(newSongID));

                newSongID = "";
                Assert.Throws<ArgumentException>(() => songService.Exists(newSongID));
            }

            [Fact]
            public void TestExistsFinds()
            {
                Assert.True(songService.Exists(song.ID));
            }

            [Fact]
            public void TestExistsNotFind()
            {
                var newSongID = "what";
                Assert.False(songService.Exists(newSongID));
            }
        }

        // TODO: Add tests for GetStreamURL

        // TODO: Add tests for CreateNewSong
    }
}