using System.Threading.Tasks;
using Discord.Commands;
using tibrudna.djcort.src.Services;

namespace tibrudna.djcort.src.Modules
{
    /// <summary>Commands to controll the song queue.</summary>
    public class PlaylistModule : ModuleBase<SocketCommandContext>
    {
        private readonly PlaylistService playlistService;
        private readonly SongService songService;

        /// <summary>Creates a new instance of the PlaylistModule class.</summary>
        /// <param name="playlistService">The service to operate with the playlist.</param>
        /// <param name="songService">The service to operate with songs.</param>
        public PlaylistModule(PlaylistService playlistService, SongService songService)
        {
            this.playlistService = playlistService;
            this.songService = songService;
        }

        /// <summary>Adds a song to the queue. It doesn't persist the song.</summary>
        /// <param name="url">The url to the song.</param>
        [Command("add")]
        public async Task AddToPlaylist(string url)
        {
            var song = await songService.CreateNewSong(url);
            playlistService.Enqueue(song);
            await Context.Channel.SendMessageAsync("Song was added to queue.");
        }

        /// <summary>Loads all songs in the database into the queue.</summary>
        [Command("load")]
        public async Task LoadPlaylist()
        {
            await playlistService.LoadPlaylist();
            await Context.Channel.SendMessageAsync("Playlist was loaded");
        }
    }
}