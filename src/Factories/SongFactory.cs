using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tibrudna.djcort.src.Models;
using VideoLibrary;

namespace tibrudna.djcort.src.Factories
{
    public class SongFactory
    {
        public static async Task<Song> CreateNewSong(string url)
        {
            Song song = new Song();
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(url);
            var task = video.GetUriAsync();

            var titleParts = video.Title.Split("-");
            song.Artist = titleParts[0];
            song.Title = titleParts[1];

            song.ID = ParseID(url);
            song.StreamUrl = await task;

            return song;
        }

        private static string ParseID(string url)
        {
            var pattern = "v=[\\w-]*";
            var match = Regex.Match(url, pattern);
            return match.Value.Remove(0,2);
        }
    }
}