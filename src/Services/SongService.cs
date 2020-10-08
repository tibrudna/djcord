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
            if (await FindById(song.ID) != null) return;
            await this.database.AddAsync(song);
            await this.database.SaveChangesAsync();
        }

        public async Task<Song> FindById(string id)
        {
            return await this.database.FindAsync<Song>(id);
        }

        public async Task<Song> CreateNewSong(string url)
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

        private string ParseID(string url)
        {
            var pattern = "v=[\\w-]*";
            var match = Regex.Match(url, pattern);
            return match.Value.Remove(0,2);
        }

    }
}