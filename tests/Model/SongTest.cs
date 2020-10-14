using tibrudna.djcort.src.Models;
using Xunit;

namespace tibrudna.djcort.tests.Model
{
    public class SongTest
    {
        private readonly Song song;

        public SongTest()
        {
            song = new Song();
            song.Artist = "Bob";
            song.Title = "Bob sings";
            song.ID = "dQw4w9WgXcQ";
        }

        [Fact]
        public void TestEquals()
        {
            Assert.False(song.Equals(null));
            Assert.False(song.Equals("hello"));

            var song2 = new Song();
            song2.Artist = "Bob";
            song2.Title = "Bob sings";
            song2.ID = "L_jWHffIx5E";

            Assert.False(song.Equals(song2));

            song2.ID = song.ID;
            Assert.True(song.Equals(song2));
        }

        [Fact]
        public void TestGetHashCode()
        {
            Assert.Equal(song.ID.GetHashCode(), song.GetHashCode());
        }
    }
}