using tibrudna.djcort.src.Models;
using Xunit;

namespace tibrudna.djcort.tests.Model
{
    public class SongTest
    {
        public class EqualTest
        {
            private readonly Song song;
            private readonly Song compareSong;

            public EqualTest()
            {
                song = new Song { ID = "dQw4w9WgXcQ", Title = "Bob sings", Artist = "Bob" };
                compareSong = new Song { ID = "1234", Title = "Something", Artist = "Someone" };
            }


            [Fact]
            public void TestEqualsNull()
            {
                Assert.False(song.Equals(null));
            }

            [Fact]
            public void TestEqualsFalseObject()
            {
                Assert.False(song.Equals("hello"));
            }

            [Fact]
            public void TestEqualsDifferentSongs()
            {
                Assert.False(song.Equals(compareSong));
            }

            [Fact]
            public void TestEqualSongSameID()
            {
                compareSong.ID = song.ID;
                Assert.True(song.Equals(compareSong));
            }

            [Fact]
            public void TestEqualSameSong()
            {
                Assert.True(song.Equals(song));
            }
        }

        public class GetHashCodeTest
        {
            private readonly Song song;
            private readonly Song compareSong;

            public GetHashCodeTest()
            {
                song = new Song { ID = "dQw4w9WgXcQ", Title = "Bob sings", Artist = "Bob" };
                compareSong = new Song { ID = "1234", Title = "Something", Artist = "Someone" };
            }

            [Fact]
            public void TestGetHashCodeDependsOnId()
            {
                Assert.Equal(song.ID.GetHashCode(), song.GetHashCode());
            }

            [Fact]
            public void TestHahCodeDifferentObject()
            {
                Assert.NotEqual(song.GetHashCode(), compareSong.GetHashCode());
            }

            [Fact]
            public void TestHashCodeSameObjectID()
            {
                compareSong.ID = song.ID;
                Assert.Equal(song.GetHashCode(), compareSong.GetHashCode());
            }
        }

    }
}