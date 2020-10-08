using System.Threading.Tasks;
using Discord.Commands;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src.Modules
{
    public class PlaylistModule : ModuleBase<SocketCommandContext>
    {
        private readonly PlaylistService playlistService;
        private readonly SongService songService;

        public PlaylistModule(PlaylistService playlistService, SongService songService)
        {
            this.playlistService = playlistService;
            this.songService = songService;
        }

        [Command("add")]
        public async Task AddToPlaylist(string url)
        {
            var song = await songService.CreateNewSong(url);
            playlistService.Enqueue(song);
            await Context.Channel.SendMessageAsync("Song was added to queue.");
        }
    }
}