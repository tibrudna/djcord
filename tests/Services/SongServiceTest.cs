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
        private DatabaseFixture fixture;
        private SongService songService;

        public SongServiceTest()
        {
            this.fixture = new DatabaseFixture();
            this.songService = new SongService(fixture.MockContext.Object);
        }

        [Fact]
        public void TestAddNull()
        {
            Assert.Throws<ArgumentException>(() => songService.Add(null));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestAddDuplicate()
        {
            Assert.Throws<DuplicateSongException>(() => songService.Add(fixture.Song));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestAdd()
        {
            fixture.MockSet.Setup(m => m.Add(It.IsAny<Song>()))
                            .Callback((Song newSong) => fixture.Data.Add(newSong));

            var newSong = new Song { ID = "987das", Title = "Something", Artist = "Someone" };

            songService.Add(newSong);

            Assert.Contains<Song>(newSong, fixture.Data);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void TestRemoveSongNull()
        {
            Assert.Throws<ArgumentException>(() => songService.RemoveSong(null));
            fixture.MockSet.Verify(m => m.Remove(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestRemoveSong()
        {
            fixture.MockSet.Setup(m => m.Remove(It.IsAny<Song>()))
                            .Callback((Song oldSong) => fixture.Data.Remove(oldSong));

            songService.RemoveSong(fixture.Song3);

            Assert.DoesNotContain<Song>(fixture.Song3, fixture.Data);
        }

        [Fact]
        public void TestRemoveSongNotInList()
        {
            var oldSong = new Song{ ID="what", Title="Something", Artist="Someone"};

            Assert.Throws<SongNotInDatabaseException>(() => songService.RemoveSong(oldSong));
            fixture.MockSet.Verify(m => m.Remove(oldSong), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestExistsNull()
        {
            Assert.Throws<ArgumentException>(() => songService.Exists(null));
        }

        [Fact]
        public void TestExistsEmptyId()
        {
            Assert.Throws<ArgumentException>(() => songService.Exists(""));
        }

        [Fact]
        public void TestExistNot()
        {
            Assert.False(songService.Exists("Someid"));
        }

        [Fact]
        public void TestExists()
        {
            Assert.True(songService.Exists(fixture.Song.ID));
        }

        [Fact]
        public void TestFindSongByIDNull()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByID(null));
        }

        [Fact]
        public void TestFindSongByIDEmpty()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByID(""));
        }

        [Fact]
        public void TestFindSongByIdNot()
        {
            Assert.Null(songService.FindSongByID("Someid"));
        }

        [Fact]
        public void TestFindSongByID()
        {
            Assert.Equal(fixture.Song, songService.FindSongByID(fixture.Song.ID));
        }

        [Fact]
        public void TestFindSongByTitleNull()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByTitle(null));
        }

        [Fact]
        public void TestFindSongByTitleEmpty()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByTitle(""));
        }

        [Fact]
        public void TestFindSongByTitleNot()
        {
            Assert.Empty(songService.FindSongByTitle("titlw"));
        }

        [Fact]
        public void TestFindSongByTitleWithTitle()
        {
            var result = songService.FindSongByTitle(fixture.Song.Title);

            Assert.Contains<Song>(fixture.Song, result);
            Assert.Contains<Song>(fixture.Song2, result);
            Assert.DoesNotContain<Song>(fixture.Song3, result);
        }

        [Fact]
        public void TestFindSongByTitleWithArtist()
        {
            var result = songService.FindSongByTitle(fixture.Song.Artist);

            Assert.Contains<Song>(fixture.Song, result);
            Assert.Contains<Song>(fixture.Song2, result);
            Assert.Contains<Song>(fixture.Song3, result);
        }

        [Fact]
        public void TestFindSongByTitleIngnoreCaseSensitivity()
        {
            var result = songService.FindSongByTitle(fixture.Song.Title.ToUpper());

            Assert.Contains<Song>(fixture.Song, result);
            Assert.Contains<Song>(fixture.Song2, result);
            Assert.DoesNotContain<Song>(fixture.Song3, result);
        }

        [Fact]
        public void TestGetAll()
        {
            Assert.Equal(fixture.Data, songService.GetAll());
        }
    }
}