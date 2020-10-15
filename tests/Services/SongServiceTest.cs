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
        private readonly DatabaseFixture fixture;
        private readonly SongService songService;

        public SongServiceTest()
        {
            fixture = new DatabaseFixture();
            songService = new SongService(fixture.MockContext.Object);
        }

        [Fact]
        public void TestAddDuplicateID()
        {
            Assert.Throws<DuplicateSongException>(() => songService.Add(fixture.Song));

            fixture.MockSet.Verify(m => m.Add(fixture.Song), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestAddDuplicateTitleAndArtist()
        {
            var searchSong = new Song { ID = "qwert1", Title = "something", Artist = "someone" };

            songService.Add(searchSong);

            fixture.MockSet.Verify(m => m.Add(searchSong), Times.Once);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void TestAddNull()
        {
            Assert.Throws<ArgumentException>(() => songService.Add(null));

            fixture.MockContext.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestAddSongEmtpyTitle()
        {
            var newSong = new Song { ID = "1234", Title = null, Artist = "Someone" };
            Assert.Throws<ArgumentException>(() => songService.Add(newSong));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);

            newSong.Title = "";
            Assert.Throws<ArgumentException>(() => songService.Add(newSong));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);

        }

        [Fact]
        public void TestAddSongEmptyID()
        {
            var newSong = new Song { ID = null, Title = "Something", Artist = "Someone" };
            Assert.Throws<ArgumentException>(() => songService.Add(newSong));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);

            newSong.ID = "";
            Assert.Throws<ArgumentException>(() => songService.Add(newSong));
            fixture.MockSet.Verify(m => m.Add(null), Times.Never);
            fixture.MockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void TestExistsValidArgument()
        {
            string newSongID = null;
            Assert.Throws<ArgumentException>(() => songService.Exists(newSongID));

            newSongID = "";
            Assert.Throws<ArgumentException>(() => songService.Exists(newSongID));
        }

        [Fact]
        public void TestExistsFinds()
        {
            Assert.True(songService.Exists(fixture.Song.ID));
        }

        [Fact]
        public void TestExistsNotFind()
        {
            var newSongID = "what";
            Assert.False(songService.Exists(newSongID));
        }


        [Fact]
        public void TestSongFindByIDValidArgument()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByID(null));
            Assert.Throws<ArgumentException>(() => songService.FindSongByID(""));
        }

        [Fact]
        public void TestFindSongByIDNotFound()
        {
            var foundSong = songService.FindSongByID("8452wg");
            Assert.Null(foundSong);
        }

        [Fact]
        public void TestFindSongByIDFound()
        {
            var foundSong = songService.FindSongByID(fixture.Song.ID);
            Assert.Equal(fixture.Song, foundSong);
        }

        [Fact]
        public void TestfindSongByTitleValidArgument()
        {
            Assert.Throws<ArgumentException>(() => songService.FindSongByTitle(null));
            Assert.Throws<ArgumentException>(() => songService.FindSongByTitle(""));
        }

        [Fact]
        public void TestFindSongByTitleNotFound()
        {
            var songs = songService.FindSongByTitle("never");
            Assert.Empty(songs);
        }

        [Fact]
        public void TestFindSongByTitleFound()
        {
            var songs = songService.FindSongByTitle("Something");
            Assert.Contains<Song>(fixture.Song, songs);
            Assert.Contains<Song>(fixture.Song2, songs);
            Assert.DoesNotContain<Song>(fixture.Song3, songs);
        }

        [Fact]
        public void TestGetAllSongs()
        {
            var songs = songService.GetAll();
            Assert.Equal(fixture.Data.ToList<Song>(), songs);
        }

        // TODO: Add tests for GetStreamURL

        // TODO: Add tests for CreateNewSong
    }
}