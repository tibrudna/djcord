using System.Threading.Tasks;
using System.Linq;
using tibrudna.djcort.src.Dao;
using tibrudna.djcort.src.Models;
using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Services
{
    public class SongService
    {
        private readonly DatabaseContext database;

        public SongService(DatabaseContext database)
        {
            this.database = database;
        }

        public async Task Add(Song song)
        {
            if (database.songs.Any<Song>(s => s.ID.Equals(song.ID))) return;
            await database.songs.AddAsync(song);
            await database.SaveChangesAsync();
        }

        public async Task<Song> FindById(string id)
        {
            return await database.songs.SingleAsync<Song>(s => s.ID.Equals(id));
        }

        public async Task<string> GetStreamUrl(Song song)
        {
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(song.Url);
            return await video.GetUriAsync();
        }

        public async Task<Song> CreateNewSong(string url)
        {
            Song song = new Song();
            var youtube = YouTube.Default;
            var video = await youtube.GetVideoAsync(url);
            var task = video.GetUriAsync();

            if (video.Title.Contains('-'))
            {
                var titleParts = video.Title.Split("-");
                song.Artist = titleParts[0];
                song.Title = titleParts[1];
            }
            else
            {
                song.Title = video.Title;
            }

            song.ID = ParseID(url);
            song.StreamUrl = await task;

            return song;
        }

        private string ParseID(string url)
        {
            var pattern = "v=[\\w-]*";
            var match = Regex.Match(url, pattern);
            return match.Value.Remove(0,2);
        }

    }
}