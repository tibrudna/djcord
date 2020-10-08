using System.Threading.Tasks;
using Discord.Commands;
using tibrudna.djcort.src.Models;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src.Modules
{
    [Group("db")]
    public class SongModule : ModuleBase<SocketCommandContext>
    {
        private readonly SongService songService;

        public SongModule(SongService songService)
        {
            this.songService = songService;
        }

        [Command("add")]
        public async Task AddToDb(string url)
        {
            var song = await songService.CreateNewSong(url);
            await songService.Add(song);
            await Context.Channel.SendMessageAsync("Song was added to the database");
        }

        [Command("find")]
        public async Task FindById(string id)
        {
            var song = await songService.FindById(id);
            await Context.Channel.SendMessageAsync(song.Title);
        }
    }
}